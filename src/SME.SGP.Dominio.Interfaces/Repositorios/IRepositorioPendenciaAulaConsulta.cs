﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAulaConsulta
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, long dreId, long ueId, int anoLetivo);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa(long dreId, long ueId, int anoLetivo);
        Task<long[]> ListarPendenciasPorAulaId(long aulaId);
        Task<long[]> ListarPendenciasPorAulasId(long[] aulasId);
        Task<PendenciaAulaDto> PossuiPendenciasPorAulaId(long aulaId, bool ehInfantil, Usuario usuarioLogado);
        Task<bool> PossuiPendenciasPorAulasId(long[] aulasId, bool ehInfantil, long[] componentesCurricularesId);
        Task<long> ObterPendenciaPorDescricaoTipo(string descricao, TipoPendencia tipoPendencia);
        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulaId(long aulaId);
        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulasId(long[] aulasId);
        Task<bool> PossuiAtividadeAvaliativaSemNotaPorAulasId(long[] aulasId);
        Task<Turma> ObterTurmaPorPendencia(long pendenciaId);

        Task<Turma> ObterTurmaPorPendenciaDiario(long pendenciaId);
        Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId);
        Task<long> ObterPendenciaAulaPorTurmaIdDisciplinaId(string turmaId, string disciplinaId, string professorRf, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaAulaIdPorAulaId(long aulaId, TipoPendencia tipoPendencia);
        Task<IEnumerable<long>> ObterPendenciaIdPorAula(long aulaId, TipoPendencia tipoPendencia);
        Task<bool> PossuiPendenciasPorTipo(string disciplinaId, string turmaId, TipoPendencia tipoPendenciaAula, int bimestre,bool professorCj, bool professorNaoCj, string professorRf = "");
        
        Task<IEnumerable<PossuiPendenciaDiarioBordoDto>> TurmasPendenciaDiarioBordo(IEnumerable<long> aulasId, string turmaId, int bimestre);

        Task<IEnumerable<long>> TrazerAulasComPendenciasDiarioBordo(string componenteCurricularId, string professorRf, bool ehGestor, string turma, int anoLetivo);        
        Task<long> ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolar(long componenteCurricularId, string codigoRf, long periodoEscolarId);        
        Task<IEnumerable<PendenciaAulaProfessorDto>> ObterPendenciaIdPorComponenteProfessorEBimestre(long componenteCurricularId, string codigoRf, long periodoEscolarId, TipoPendencia tipoPendencia, string turmaCodigo, long ueId);
        Task<IEnumerable<long>> ObterPendenciasAulaPorDreUeTipoModalidade(long dreId, long ueId, TipoPendencia tipoPendencia, Modalidade modalidade);
    }
}