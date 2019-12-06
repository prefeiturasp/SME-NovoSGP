using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaAtividadeAvaliativa
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>> ListarPaginado(FiltroAtividadeAvaliativaDto filtro);

        IEnumerable<AtividadeAvaliativa> ObterAvaliacoesDoBimestre(string turmaId, string disciplinaId, int anoLetivo, int bimestre, ModalidadeTipoCalendario modalidade);

        AtividadeAvaliativaCompletaDto ObterPorId(long id);
    }
}