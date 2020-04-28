using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralAluno : RepositorioBase<RelatorioSemestralAluno>, IRepositorioRelatorioSemestralAluno
    {
        public RepositorioRelatorioSemestralAluno(ISgpContext database) : base(database)
        {
        }

        public Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new System.NotImplementedException();
        }
    }
}