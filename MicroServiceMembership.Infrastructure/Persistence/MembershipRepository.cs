using Dapper;
using Npgsql;
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
                var sqlMembership = @"
                    INSERT INTO public.memberships 
                    (name, price, description, monthly_sessions, created_at, created_by) 
                    VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @CreatedBy) 
                    RETURNING id;";
                var membershipId = await connection.ExecuteScalarAsync<int>(sqlMembership, membership, transaction);

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
                SELECT id, 
                       name, 
                       price, 
                       description, 
                       monthly_sessions AS MonthlySessions, 
                       created_at AS CreatedAt, 
                       created_by AS CreatedBy, 
                       last_modification AS LastModification, 
                       last_modified_by AS LastModifiedBy
                FROM public.memberships 
                WHERE id = @Id;";
            return await connection.QuerySingleOrDefaultAsync<Membership>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                SELECT id, 
                       name, 
                       price, 
                       description, 
                       monthly_sessions AS MonthlySessions, 
                       created_at AS CreatedAt, 
                       created_by AS CreatedBy, 
                       last_modification AS LastModification, 
                       last_modified_by AS LastModifiedBy
                FROM public.memberships;";
            return await connection.QueryAsync<Membership>(sql);
        }

        public async Task<bool> UpdateAsync(Membership membership)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            var sql = @"
                UPDATE public.memberships SET 
                    name = @Name, 
                    price = @Price, 
                    description = @Description, 
                    monthly_sessions = @MonthlySessions, 
                    last_modification = @LastModification, 
                    last_modified_by = @LastModifiedBy 
                WHERE id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, membership);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using IDbConnection connection = new NpgsqlConnection(_connectionString);
            var sql = "DELETE FROM public.memberships WHERE id = @Id;";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}