using System;
using System.Runtime.CompilerServices;

namespace SME.SGP.TesteIntegracao
{
    public static class NpgsqlInitializer
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
