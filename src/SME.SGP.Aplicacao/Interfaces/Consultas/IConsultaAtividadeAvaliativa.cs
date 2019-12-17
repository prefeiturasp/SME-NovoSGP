using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaAtividadeAvaliativa
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>> ListarPaginado(FiltroAtividadeAvaliativaDto filtro);

        Task<(IEnumerable<AtividadeAvaliativa>, int quantidadeBimestres, PeriodoEscolar periodoAtual)> ObterAvaliacoesEBimestres(string turmaId, string disciplinaId, int anoLetivo, int? bimestre, ModalidadeTipoCalendario modalidadeTipoCalendario);

        Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id);

        Task<IEnumerable<AtividadeAvaliativaExistenteRetornoDto>> ValidarAtividadeAvaliativaExistente(FiltroAtividadeAvaliativaExistenteDto filtroAtividadeAvaliativaExistenteDto);

        Task<IEnumerable<TurmaRetornoDto>> ObterTurmasCopia(string turmaId, string disciplinaId);
    }
}