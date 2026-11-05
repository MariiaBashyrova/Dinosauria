using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dinosauria.Infrastructure.Data;

/// <summary>Stellt sicher, dass das Datenbankschema beim Programmstart existiert.</summary>
public class DatenbankInitialisierer
{
    /// <summary>Erzeugt die Datenbank, falls sie noch nicht angelegt wurde.</summary>
    public static void Initialisieren(DinosauriaDbContext context)
    {
       
        context.Database.EnsureCreated();
    }
}
