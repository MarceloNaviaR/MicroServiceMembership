using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroserviceMembership.Domain.Entities;

namespace MicroserviceMembership.Domain.Ports
{
    public interface IMembershipRepository
    {
        Task<Membership?> GetByIdAsync(int id);
        Task<IEnumerable<Membership>> GetAllAsync();
        Task<int> AddAsync(Membership membership);
        Task<bool> UpdateAsync(Membership membership);
        Task<bool> DeleteAsync(int id);
    }
}