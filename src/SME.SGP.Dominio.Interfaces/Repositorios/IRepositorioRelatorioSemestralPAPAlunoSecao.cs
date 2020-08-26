using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioSemestralPAPAlunoSecao
    {
        Task SalvarAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno);

        Task RemoverAsync(RelatorioSemestralPAPAlunoSecao secaoRelatorioAluno);
    }
}