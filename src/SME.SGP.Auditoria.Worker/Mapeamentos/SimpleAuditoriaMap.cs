using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public abstract class SimpleAuditoriaMap<T> : EntityAuditoriaMap<T>
        where T : class
    {
        protected SimpleAuditoriaMap()
        {
            Map("Id", "id");
        }
    }
}
