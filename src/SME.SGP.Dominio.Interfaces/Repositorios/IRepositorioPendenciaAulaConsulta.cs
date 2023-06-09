using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAulaConsulta
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, long dreId, long ueId, int anoLetivo,bool exibirRegistroSemPendencia = true);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa(long dreId, long ueId, int anoLetivo,bool exibirRegistroSemPendencia = true, TipoAvaliacaoCodigo tipoAtividadeAvaliativaIgnorada = TipoAvaliacaoCodigo.AtividadeClassroom);
        Task<long[]> ListarPendenciasPorAulaId(long aulaId);
        Task<long[]> ListarPendenciasPorAulasId(long[] aulasId);
        Task<PendenciaAulaDto> PossuiPendenciasPorAulaId(long aulaId, bool ehInfantil, Usuario usuarioLogado, long? disciplinaIdTerritorio = null);
        Task<bool> PossuiPendenciasPorAulasId(long[] aulasId, bool ehInfantil, long[] componentesCurricularesId);
        Task<long> ObterPendenciaPorDescricaoTipo(string descricao, TipoPendencia tipoPendencia);
        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulaId(long aulaId);
        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulasId(long[] aulasId);
        Task<bool> PossuiAtividadeAvaliativaSemNotaPorAulasId(long[] aulasId);
        Task<Turma> ObterTurmaPorPendencia(long pendenciaId);
        Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId);
        Task<long> ObterPendenciaAulaPorTurmaIdDisciplinaId(string turmaId, string disciplinaId, string professorRf, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaAulaIdPorAulaId(long aulaId, TipoPendencia tipoPendencia);
        Task<IEnumerable<long>> ObterPendenciaIdPorAula(long aulaId, TipoPendencia tipoPendencia);
        Task<bool> PossuiPendenciasPorTipo(string disciplinaId, string turmaId, TipoPendencia tipoPendenciaAula, int bimestre,bool professorCj, bool professorNaoCj, string professorRf = "");
        
        Task<IEnumerable<PossuiPendenciaDiarioBordoDto>> TurmasPendenciaDiarioBordo(IEnumerable<long> aulasId, string turmaId, int bimestre);

        Task<IEnumerable<long>> TrazerAulasComPendenciasDiarioBordo(string componenteCurricularId, string professorRf, bool ehGestor, string turma, int anoLetivo);        
        Task<long> ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarTurma(long componenteCurricularId, string codigoRf, long periodoEscolarId, string codigoTurma = "");        
        Task<IEnumerable<PendenciaAulaProfessorDto>> ObterPendenciaIdPorComponenteProfessorEBimestre(string componenteCurricularId, string codigoRf, long periodoEscolarId, TipoPendencia tipoPendencia, string turmaCodigo, long ueId);
        Task<IEnumerable<long>> ObterPendenciasAulaPorDreUeTipoModalidade(long dreId, long ueId, TipoPendencia tipoPendencia, Modalidade modalidade);
        Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE);
        Task<long[]> ObterPendenciasAulaDiarioClassePorTurmaDisciplinaPeriodo(string turmaId, string disciplinaId, DateTime periodoInicio, DateTime periodoFim);
    }
}