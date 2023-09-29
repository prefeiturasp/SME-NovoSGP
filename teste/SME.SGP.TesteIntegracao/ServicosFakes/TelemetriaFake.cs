using System;
using System.Threading.Tasks;
using Elastic.Apm.Api;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class TelemetriaFake : IServicoTelemetria
    {
        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            acao();
        }

        public T RegistrarComRetorno<T>(Func<T> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }

        public Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }

        public Task<T> RegistrarComRetornoAsync<T>(Func<Task<T>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            return acao();
        }

        public ITransaction Iniciar(string nome, string tipo)
            => null;

        public void Finalizar(ITransaction transacao)
        {
        }
    }
}
