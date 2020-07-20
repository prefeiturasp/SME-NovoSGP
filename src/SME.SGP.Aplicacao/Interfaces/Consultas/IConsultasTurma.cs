using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasTurma
    {
        Task<Turma> ObterPorCodigo(string codigoTurma);
        Task<Turma> ObterComUeDrePorCodigo(string codigoTurma);
        Task<Turma> ObterComUeDrePorId(long turmaId);
        Task<bool> TurmaEmPeriodoAberto(string codigoTurma, DateTime dataReferencia, int bimestre = 0, TipoCalendario tipoCalendario = null);
        Task<bool> TurmaEmPeriodoAberto(long turmaId, DateTime dataReferencia, int bimestre = 0, TipoCalendario tipoCalendario = null);
        Task<bool> TurmaEmPeriodoAberto(Turma turma, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false, TipoCalendario tipoCalendario = null);
        Task<IEnumerable<PeriodoEscolarAbertoDto>> PeriodosEmAbertoTurma(string turmaCodigo, DateTime dataReferencia, bool ehAnoLetivo = false);
        Task<IEnumerable<AlunoDadosBasicosDto>> ObterDadosAlunos(string turmaCodigo, int anoLetivo, PeriodoEscolar periodoEscolar = null);
        Task<TipoCalendarioSugestaoDto> ObterSugestaoTipoCalendarioPorTurma(string turmaCodigo);
        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);
    }
}
