using Polly.Registry;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaMetricasFake : ServicoMensageria<MetricaMensageria>, IServicoMensageriaMetricas
    {

        public ServicoMensageriaMetricasFake(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria,
            IReadOnlyPolicyRegistry<string> registry)
            : base(conexaoRabbit, servicoTelemetria, registry)
        {
        }

        public Task Concluido(string rota)
            => Task.CompletedTask;

        public Task Erro(string rota)
            => Task.CompletedTask;

        public Task Obtido(string rota)
            => Task.CompletedTask;

        public Task Publicado(string rota)
            => Task.CompletedTask;
    }
}
