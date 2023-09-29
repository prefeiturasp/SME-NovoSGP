using Elastic.Apm.Api;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface IServicoTelemetria
    {
        ITransaction Iniciar(string nome, string tipo);
        void Finalizar(ITransaction transacao);

        Task<K> RegistrarComRetornoAsync<T,K>(Func<Task<K>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        Task<K> RegistrarComRetornoAsync<K>(Func<Task<K>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        K RegistrarComRetorno<T,K>(Func<K> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        K RegistrarComRetorno<K>(Func<K> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
        void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor);
        Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "");
    }
}
