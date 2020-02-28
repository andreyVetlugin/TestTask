using System;

namespace AisBenefits.Infrastructure.Permissions
{
    public static class RolePermissionsHelper
    {
        public static string[] Resolve(string permissions) =>
            permissions.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        public static string Create(params string[] permissions) =>
            string.Join(" ", permissions);
    }
}
