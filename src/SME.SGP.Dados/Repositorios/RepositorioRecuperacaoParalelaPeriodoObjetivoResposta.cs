using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRecuperacaoParalelaPeriodoObjetivoResposta : RepositorioBase<RecuperacaoParalelaPeriodoObjetivoResposta>, IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta
    {
        public RepositorioRecuperacaoParalelaPeriodoObjetivoResposta(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task Excluir(long RecuperacaoParalelaId)
        {
            var query = new StringBuilder();
            query.AppendLine("delete from  recuperacao_paralela_periodo_objetivo_resposta ");
            query.AppendLine("where recuperacao_paralela_id = @RecuperacaoParalelaId  ");
            await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { RecuperacaoParalelaId });
        }
    }
}