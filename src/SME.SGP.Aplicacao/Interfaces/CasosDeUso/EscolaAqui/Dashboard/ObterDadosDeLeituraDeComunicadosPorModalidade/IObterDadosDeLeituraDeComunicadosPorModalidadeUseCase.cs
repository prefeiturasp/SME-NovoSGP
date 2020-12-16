using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade
{
    public interface IObterDadosDeLeituraDeComunicadosPorModalidadeUseCase
    {
        Task<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> Executar(ObterDadosDeLeituraDeComunicadosDto obterDadosDeLeituraDeComunicadosDto);
    }

}
