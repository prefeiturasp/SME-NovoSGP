namespace SME.SGP.Dados.Cache
{
    public interface IMetricasCache
    {
        void Hit(string nomeChave, string metodo = "GET");
        void Miss(string nomeChave, string metodo = "GET");
        void Fail(string nomeChave, string metodo = "GET");
    }
}
