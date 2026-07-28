namespace SME.SGP.Auditoria.Worker.Mapeamentos
{
    public abstract class SimpleEntityMap<T> : EntityMap<T> where T : class
    {
        protected SimpleEntityMap()
        {
            Map("Id", "id");
        }
    }
}
