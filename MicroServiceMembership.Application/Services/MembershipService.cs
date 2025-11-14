using MicroserviceMembership.Application.Common;
using MicroserviceMembership.Application.Interfaces;
using MicroserviceMembership.Domain.Entities;
using MicroserviceMembership.Domain.Ports;
using MicroserviceMembership.Domain.Rules;

namespace MicroserviceMembership.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<Result<int>> CreateAsync(Membership membership)
        {
            var nameError = MembershipValidationRules.ValidateName(membership.Name);
            if (nameError != null) return Result<int>.Failure(nameError);

            var priceError = MembershipValidationRules.ValidatePrice(membership.Price);
            if (priceError != null) return Result<int>.Failure(priceError);

            var sessionsError = MembershipValidationRules.ValidateSessions(membership.MonthlySessions);
            if (sessionsError != null) return Result<int>.Failure(sessionsError);

            membership.CreatedAt = DateTime.UtcNow;
            membership.CreatedBy = "user_placeholder"; // Reemplazar con lógica de usuario real
            foreach (var discipline in membership.Disciplines)
            {
                discipline.CreatedAt = DateTime.UtcNow;
                discipline.CreatedBy = "user_placeholder";
            }

            var newId = await _membershipRepository.AddAsync(membership);
            return Result<int>.Success(newId);
        }

        public async Task<Result> UpdateAsync(Membership membership)
        {
            var nameError = MembershipValidationRules.ValidateName(membership.Name);
            if (nameError != null) return Result.Failure(nameError);

            var existingMembership = await _membershipRepository.GetByIdAsync(membership.Id);
            if (existingMembership is null) return Result.Failure($"No se encontró la membresía con ID {membership.Id}.");

            membership.LastModification = DateTime.UtcNow;
            membership.LastModifiedBy = "user_placeholder";

            var success = await _membershipRepository.UpdateAsync(membership);
            return success ? Result.Success() : Result.Failure("Error al actualizar la membresía.");
        }

        public async Task<Result<Membership>> GetByIdAsync(int id)
        {
            var membership = await _membershipRepository.GetByIdAsync(id);
            return membership != null ? Result<Membership>.Success(membership) : Result<Membership>.Failure($"No se encontró la membresía con ID {id}.");
        }

        public async Task<Result<IEnumerable<Membership>>> GetAllAsync()
        {
            var memberships = await _membershipRepository.GetAllAsync();
            return Result<IEnumerable<Membership>>.Success(memberships);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var success = await _membershipRepository.DeleteAsync(id);
            return success ? Result.Success() : Result.Failure("Error al eliminar la membresía.");
        }
    }
}