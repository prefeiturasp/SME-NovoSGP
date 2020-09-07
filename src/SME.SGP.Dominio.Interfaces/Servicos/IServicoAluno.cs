using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAluno
    {
        Task<MarcadorFrequenciaDto> ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolar bimestre, bool ehInfantil = false);
    }
}