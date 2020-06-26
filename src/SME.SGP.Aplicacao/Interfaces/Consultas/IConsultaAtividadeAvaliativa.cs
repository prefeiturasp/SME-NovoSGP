using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaAtividadeAvaliativa
    {
        Task<PaginacaoResultadoDto<AtividadeAvaliativaCompletaDto>> ListarPaginado(FiltroAtividadeAvaliativaDto filtro);

        Task<IEnumerable<AtividadeAvaliativa>> ObterAvaliacoesNoBimestre(string turmaCodigo, string disciplinaId, DateTime periodoInicio, DateTime periodoFim);

        Task<AtividadeAvaliativaCompletaDto> ObterPorIdAsync(long id);

        Task<IEnumerable<TurmaRetornoDto>> ObterTurmasCopia(string turmaId, string disciplinaId);

        Task<IEnumerable<AtividadeAvaliativaExistenteRetornoDto>> ValidarAtividadeAvaliativaExistente(FiltroAtividadeAvaliativaExistenteDto dto);

        Task<bool> AtividadeAvaliativaDentroPeriodo(AtividadeAvaliativa atividadeAvaliativa);

        Task<bool> AtividadeAvaliativaDentroPeriodo(string turmaId, DateTime dataAula);
    }
}