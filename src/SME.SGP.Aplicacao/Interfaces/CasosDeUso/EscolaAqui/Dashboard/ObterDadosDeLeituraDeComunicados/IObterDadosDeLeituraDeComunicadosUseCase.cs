using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados
{
    public interface IObterDadosDeLeituraDeComunicadosUseCase
    {
        Task<IEnumerable<DadosDeLeituraDoComunicadoDto>> Executar(ObterDadosDeLeituraDeComunicadosDto obterDadosDeLeituraDeComunicadosDto);
    }
}
