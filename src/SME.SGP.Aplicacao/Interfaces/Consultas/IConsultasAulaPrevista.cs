using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasAulaPrevista
    {
        Task<AulasPrevistasDadasAuditoriaDto> BuscarPorId(long id);

        Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId, int semestre = 0);
        Task<int> ObterAulasDadas(Turma turma, string componenteCurricularCodigo, int bimestre);
    }
}
