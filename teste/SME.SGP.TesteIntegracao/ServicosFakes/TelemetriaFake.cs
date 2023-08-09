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


        public Task<T> RegistrarComRetornoAsync<T1, T2, T3, T>(T1 t1, T2 t2, T3 t3, Func<T1, T2, T3, Task<T>> acao, string acaoNome, string telemetriaNome,
            string telemetriaValor, string parametros = "")
        {
            return acao(t1, t2, t3);
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

        public ITransaction? Iniciar(string nome, string tipo)
            => null;

        public void Finalizar(ITransaction? transacao)
        { 
        }
    }
}
