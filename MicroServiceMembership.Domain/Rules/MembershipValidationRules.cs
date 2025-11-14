using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceMembership.Domain.Rules
{
    public static class MembershipValidationRules
    {
        public static string? ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "El nombre de la membresía no puede estar vacío.";
            if (name.Length > 50)
                return "El nombre de la membresía no puede exceder los 50 caracteres.";
            return null; // Sin error
        }

        public static string? ValidatePrice(decimal price)
        {
            if (price <= 0)
                return "El precio debe ser un valor positivo.";
            return null; // Sin error
        }

        public static string? ValidateSessions(short sessions)
        {
            if (sessions <= 0)
                return "El número de sesiones mensuales debe ser al menos 1.";
            return null; // Sin error
        }
    }
}