using Prometheus;

namespace SME.SGP.Dados.Cache
{
    public class MetricasCache : IMetricasCache
    {
        private readonly Counter metricaAcessos = Metrics
            .CreateCounter("SGP_cache_hit_total", "Numero de acessos ao cache", new string[] { "nome", "metodo", "status" });

        public MetricasCache()
        {
        }

        public void Hit(string nomeChave, string metodo = "GET")
            => metricaAcessos.WithLabels(nomeChave, metodo, "HIT").Inc();

        public void Miss(string nomeChave, string metodo = "GET")
            => metricaAcessos.WithLabels(nomeChave, metodo, "MISS").Inc();

        public void Fail(string nomeChave, string metodo = "GET")
            => metricaAcessos.WithLabels(nomeChave, metodo, "FAIL").Inc();
    }
}
