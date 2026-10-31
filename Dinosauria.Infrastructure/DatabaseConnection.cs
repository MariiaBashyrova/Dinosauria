using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Infrastructure;

public static class DatabaseConnection
{
    public const string ConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=DinosauriaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
}
