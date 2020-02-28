namespace AisBenefits.Infrastructure.Permissions
{
    public static class RolePermissions
    {
        public const string SuperAdministrate = nameof(SuperAdministrate);

        public const string AdministrateRoles = nameof(AdministrateRoles);
        public const string AdministarateCatalog = nameof(AdministarateCatalog);
        public const string AdministrateUsers = nameof(AdministrateUsers);
        public const string AdministratePersonInfos = nameof(AdministratePersonInfos);
        public const string AdministratePayouts = nameof(AdministratePayouts);
        public const string AdministrateReestr = nameof(AdministrateReestr);
        public const string WatchReports = nameof(WatchReports);

        public const string DoNothing = nameof(DoNothing);

        public static readonly Permission[] All = new Permission[]
        {
            new Permission{Id = AdministrateRoles, Title = "Администрирование ролей пользователей системы" },
            new Permission{Id = AdministarateCatalog, Title = "Администрирование справочников" },
            new Permission{Id = AdministrateUsers, Title = "Администрирование пользователей системы" },
            new Permission{Id = AdministratePersonInfos, Title = "Администрирование назначений" },
            new Permission{Id = AdministratePayouts, Title = "Администрирование выплат" },
            new Permission{Id = AdministrateReestr, Title = "Администрирование реестра"},
            new Permission{Id = WatchReports, Title = "Выгрузка отчётов"}
        };
    }

    public class Permission
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
