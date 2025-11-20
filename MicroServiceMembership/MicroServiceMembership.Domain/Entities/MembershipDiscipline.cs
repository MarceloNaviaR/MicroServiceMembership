using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceMembership.Domain.Entities
{
    public class MembershipDiscipline
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public int DisciplineId { get; set; }
        public string? DisciplineName { get; set; }

        // Campos de Auditoría para la relación
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModification { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}