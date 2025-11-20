using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MicroserviceMembership.Application.Common;
using MicroserviceMembership.Domain.Entities;

namespace MicroserviceMembership.Application.Interfaces
{
    public interface IMembershipService
    {
        Task<Result<Membership>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Membership>>> GetAllAsync();
        Task<Result<int>> CreateAsync(Membership membership);
        Task<Result> UpdateAsync(Membership membership);
        Task<Result> DeleteAsync(int id);
    }
}