// *** ESTAS LÍNEAS SOLUCIONAN TODOS LOS ERRORES DE CÓDIGO ***
using Dapper;
using Npgsql;

// Usings para tus propias clases y para la configuración
using MicroserviceMembership.Domain.Entities;
using MicroserviceMembership.Domain.Ports;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MicroserviceMembership.Infrastructure.Persistence
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly string _connectionString;

        public MembershipRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no fue encontrada.");
        }

        public async Task<int> AddAsync(Membership membership)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                var sqlMembership = @"INSERT INTO public.memberships (name, price, description, monthly_sessions, created_at, created_by) VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @CreatedBy) RETURNING id;";
                var membershipId = await connection.ExecuteScalarAsync<int>(sqlMembership, membership, transaction);

                foreach (var discipline in membership.Disciplines)
                {
                    discipline.MembershipId = membershipId;
                    var sqlDiscipline = @"INSERT INTO public.membership_disciplines (membership_id, discipline_id, discipline_name, created_at, created_by) VALUES (@MembershipId, @DisciplineId, @DisciplineName, @CreatedAt, @CreatedBy);";
                    await connection.ExecuteAsync(sqlDiscipline, discipline, transaction);
                }

                transaction.Commit();
                return membershipId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<Membership?> GetByIdAsync(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT m.*, md.*
                FROM public.memberships m
                LEFT JOIN public.membership_disciplines md ON m.id = md.membership_id
                WHERE m.id = @Id;";

            var membershipDictionary = new Dictionary<int, Membership>();

            var memberships = await connection.QueryAsync<Membership, MembershipDiscipline, Membership>(
                sql,
                (membership, discipline) =>
                {
                    if (!membershipDictionary.TryGetValue(membership.Id, out var currentMembership))
                    {
                        currentMembership = membership;
                        membershipDictionary.Add(currentMembership.Id, currentMembership);
                    }
                    if (discipline != null) { currentMembership.Disciplines.Add(discipline); }
                    return currentMembership;
                },
                new { Id = id },
                splitOn: "id");

            return memberships.FirstOrDefault();
        }

        public Task<bool> UpdateAsync(Membership membership) => throw new NotImplementedException();
        public Task<IEnumerable<Membership>> GetAllAsync() => throw new NotImplementedException();
        public Task<bool> DeleteAsync(int id) => throw new NotImplementedException();
    }
}