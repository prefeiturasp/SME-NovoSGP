using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosRelatorioSemestralPAPAluno
    {
        Task<RelatorioSemestralPAPAluno> Salvar(string alunoCodigo, string turmaCodigo, int semestre, RelatorioSemestralAlunoPersistenciaDto relatorioSemestralAlunoDto);
    }
}
