using Elastic.Apm.Api;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface IServicoTelemetria
    {
        ITransaction Iniciar(string nome, string tipo);
        void Finalizar(ITransaction transacao);

        Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
    }
}
