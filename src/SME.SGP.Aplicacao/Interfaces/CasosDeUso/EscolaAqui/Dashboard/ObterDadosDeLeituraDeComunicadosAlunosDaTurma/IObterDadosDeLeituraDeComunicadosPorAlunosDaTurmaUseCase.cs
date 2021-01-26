using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorAlunosDaTurma
{
    public interface IObterDadosDeLeituraDeComunicadosPorAlunosDaTurmaUseCase
    {
        Task<IEnumerable<DadosLeituraAlunosComunicadoPorTurmaDto>> Executar(FiltroDadosDeLeituraDeComunicadosPorAlunosTurmaDto filtroDadosDeLeituraDeComunicadosPorAlunosTurmaDto);
    }
}
