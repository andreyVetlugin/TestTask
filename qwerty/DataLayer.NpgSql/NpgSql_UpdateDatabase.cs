using Microsoft.EntityFrameworkCore;

namespace DataLayer.NpgSql
{
    public static partial class NpgSql
    {
        public static void UpdateDatabase(string connectionString)
        {
            using (var context = CreateContext(connectionString))
                context.Database.Migrate();
        }
    }
}
