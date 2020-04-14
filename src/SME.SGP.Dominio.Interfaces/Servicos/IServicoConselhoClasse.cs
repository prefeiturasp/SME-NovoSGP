using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoConselhoClasse
    {
        Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno);
    }
}