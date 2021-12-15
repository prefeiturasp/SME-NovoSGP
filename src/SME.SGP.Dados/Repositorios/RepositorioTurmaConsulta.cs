using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTurmaConsulta : IRepositorioTurmaConsulta
    {
        private readonly ISgpContextConsultas contexto;

        public RepositorioTurmaConsulta(ISgpContextConsultas contexto)
        {
            this.contexto = contexto;
        }

        public async Task<Turma> ObterPorCodigo(string turmaCodigo)
        {
            return await contexto.Conexao.QueryFirstOrDefaultAsync<Turma>("select * from turma where turma_id = @turmaCodigo", new { turmaCodigo });
        }

    }
}
