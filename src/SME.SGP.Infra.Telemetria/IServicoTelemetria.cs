using Elastic.Apm.Api;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface IServicoTelemetria
    {
        ITransaction Iniciar(string nome, string tipo);
        void Finalizar(ITransaction transacao);

        Task<T> RegistrarComRetornoAsync<T>(Func<Task<T>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        T RegistrarComRetorno<T>(Func<T> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
    }
}
