using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma
{
    public interface IObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase
    {
        Task<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> Executar(FiltroDadosDeLeituraDeComunicadosPorModalidadeDto filtroDadosDeLeituraDeComunicadosPorModalidadeDto);
    }
}
