using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Infrastructure.Data;

public class DatenbankInitialisierer
{
    public static void Initialisieren(DinosauriaDbContext context)
    {
       
        context.Database.EnsureCreated();
    }
}
