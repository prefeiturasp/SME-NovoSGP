using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaDiarioBordoConsulta
    {
        Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId);
        Task<IEnumerable<PendenciaUsuarioDto>> ObterIdPendenciaDiarioBordoPorAulaId(long aulaId);
        Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> ObterPendenciasDiarioPorPendencia(long pendenciaId, string codigoRf);
        Task<bool> VerificarSeExistePendenciaDiarioComPendenciaId(long pendenciaId);
        Task<IEnumerable<AulaComComponenteDto>> ListarPendenciasDiario(string turmaId, long[] componentesCurricularesId);
        Task<long> ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolar(long componenteCurricularId, string codigoRf, long periodoEscolarId);
        Task<IEnumerable<long>> TrazerAulasComPendenciasDiarioBordo(string componenteCurricularId, string professorRf, bool ehGestor, string turma);
        Task<IEnumerable<PossuiPendenciaDiarioBordoDto>> TurmasPendenciaDiarioBordo(IEnumerable<long> aulasId, string turmaId, int bimestre);
        Task<Turma> ObterTurmaPorPendenciaDiario(long pendenciaId);
        Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE);
    }
}
