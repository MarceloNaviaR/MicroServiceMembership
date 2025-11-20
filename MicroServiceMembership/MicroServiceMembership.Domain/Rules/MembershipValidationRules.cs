using MicroserviceMembership.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MicroserviceMembership.Domain.Rules
{

    public static class MembershipValidationRules
    {
        private static readonly Regex AllowedCharsRegex = new Regex("^[a-zA-Z0-9 ñáéíóúÁÉÍÓÚüÜ]+$");

        public static string? ValidateName(string name)
        {
            int letterCount = name.Count(char.IsLetter);
            int digitCount = name.Count(char.IsDigit);
            if (string.IsNullOrWhiteSpace(name))
                return "El nombre de la membresía no puede estar vacío.";
            if (!AllowedCharsRegex.IsMatch(name))
                return "El nombre de la membresía contiene caracteres no permitidos.";
            if (digitCount > 2)
                return "El nombre no puede contener más de 2 números.";
            if (digitCount > 0 && letterCount < 3)
                return "Si el nombre contiene números, debe estar acompañado por al menos 5 letras.";
            if (letterCount < 3)
                return "El nombre debe contener al menos 3 letras.";
            if (name.Length > 20)
                return "El nombre de la membresía no puede exceder los 20 caracteres.";
            return null; // Sin error
        }
        public static string? ValidateDescription(string description)
        {
            int letterCount = description.Count(char.IsLetter);
            int digitCount = description.Count(char.IsDigit);
            if (string.IsNullOrWhiteSpace(description))
                return "El nombre de la descripción no puede estar vacío.";
            if (!AllowedCharsRegex.IsMatch(description  ))
                return "La descripción contiene caracteres no permitidos.";
            if (digitCount > 2)
                return "La descripción no puede contener más de 2 números.";
            if (digitCount > 0 && letterCount < 3)
                return "Si la descripción contiene números, debe estar acompañado por al menos 5 letras.";
            if (letterCount < 3)
                return "La descripción debe contener al menos 3 letras.";
            if (description.Length > 50)
                return "La descripción no puede exceder los 50 caracteres.";
            return null; // Sin error
        }

        public static string? ValidatePrice(decimal price)
        {
            if (price < 0)
                return "El precio debe ser mayor a cero.";
            return null; // Sin error
        }

        public static string? ValidateSessions(short sessions)
        {
            if (sessions < 0)
                return "El número de sesiones mensuales debe ser al menos 1.";
            return null; // Sin error
        }
    }
}