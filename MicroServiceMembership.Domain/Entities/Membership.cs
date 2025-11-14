namespace MicroserviceMembership.Domain.Entities
{
    public class Membership
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public short MonthlySessions { get; set; }

        // Propiedad de navegación para las disciplinas que incluye esta membresía
        public List<MembershipDiscipline> Disciplines { get; set; } = new();

        // Campos de Auditoría
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModification { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}