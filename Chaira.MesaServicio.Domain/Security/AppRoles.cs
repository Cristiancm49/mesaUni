using System.Globalization;
using System.Text;

namespace Chaira.MesaServicio.Domain.Security
{
    public static class AppRoles
    {
        public const string Administrador = "Administrador";
        public const string Administrativo = "Administrativo";
        public const string Tecnico = "Tecnico";
        public const string Usuario = "Usuario";

        public const string AdministrativoAdministrador = Administrativo + "," + Administrador;
        public const string TecnicoAdministrativoAdministrador = Tecnico + "," + Administrativo + "," + Administrador;
        public const string Todos = Usuario + "," + Tecnico + "," + Administrativo + "," + Administrador;

        public static string Normalize(string? roleName)
        {
            var normalized = RemoveDiacritics(roleName).Trim().ToUpperInvariant();

            return normalized switch
            {
                "ADMINISTRADOR" => Administrador,
                "ADMINISTRATIVO" => Administrativo,
                "TECNICO" => Tecnico,
                "USUARIO" => Usuario,
                _ => Usuario
            };
        }

        private static string RemoveDiacritics(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalizedString.Length);

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

