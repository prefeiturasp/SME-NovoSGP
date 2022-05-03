using System;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class TelemetriaFake : IServicoTelemetria
    {
        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            acao();
        }

        public dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }

        public Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }

        public Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }
    }
}
