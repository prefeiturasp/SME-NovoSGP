namespace SME.SGP.Dados.Mapeamentos
{
    public abstract class SimpleMap<T> : EntityMap<T>
        where T : class
    {
        protected SimpleMap()
        {
            Map("Id", "id");
        }
    }
}