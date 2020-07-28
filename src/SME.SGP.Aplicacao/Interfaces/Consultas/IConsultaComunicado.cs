using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaComunicado
    {
        Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id);

        Task<PaginacaoResultadoDto<ComunicadoDto>> ListarPaginado(FiltroComunicadoDto filtro);

        Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string codigoTurma, int anoLetivo);
    }
}