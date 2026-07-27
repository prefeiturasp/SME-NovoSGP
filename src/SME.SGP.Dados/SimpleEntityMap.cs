namespace SME.SGP.Dados
{
    public abstract class SimpleEntityMap<T> : EntityMap<T> where T : class
    {
        protected SimpleEntityMap()
        {
            Map("Id", "id");
        }
    }
}