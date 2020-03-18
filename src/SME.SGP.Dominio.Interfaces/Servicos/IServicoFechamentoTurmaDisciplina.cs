using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoTurmaDisciplina
    {
        Task<AuditoriaFechamentoTurmaDto> Salvar(long id, FechamentoTurmaDisciplinaDto entidadeDto);
        Task<AuditoriaFechamentoTurmaDto> SalvarAnotacaoAluno(AnotacaoAlunoDto anotacaoAluno);
    }
}
