using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.NpgSql
{
    public static partial class NpgSql
    {
        public static BenefitsContext CreateContext(string connectionString)
        {
            return new BenefitsContext(new DbContextOptionsBuilder()
                .UseNpgsql(connectionString).Options);
        }
    }
}
