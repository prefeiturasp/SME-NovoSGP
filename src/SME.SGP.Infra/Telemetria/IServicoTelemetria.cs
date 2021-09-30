using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public interface IServicoTelemetria
    {
        Task<dynamic> RegistrarAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor);

        dynamic Registrar<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor);

    }


}
