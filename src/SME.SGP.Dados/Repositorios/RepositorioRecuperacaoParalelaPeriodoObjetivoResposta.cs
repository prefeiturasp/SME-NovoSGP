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

        public async Task Excluir(long RecuperacaoParalelaId, long PeriodoId)
        {
            var query = new StringBuilder();
            query.AppendLine("delete from  recuperacao_paralela_periodo_objetivo_resposta");
            query.AppendLine("where recuperacao_paralela_id = @RecuperacaoParalelaId");
            query.AppendLine("and periodo_recuperacao_paralela_id = @PeriodoId");
            await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { RecuperacaoParalelaId, PeriodoId });
        }

        public async Task<RecuperacaoParalelaPeriodoObjetivoResposta> Obter(long recuperacaoParalelId, long objetivoId, long periodoRecuperacaoParalelaId)
        {
            var query = @"select * from recuperacao_paralela_periodo_objetivo_resposta rppor 
                            where rppor.recuperacao_paralela_id = @recuperacaoParalelId
                        and rppor.objetivo_id = @objetivoId and rppor.periodo_recuperacao_paralela_id = @periodoRecuperacaoParalelaId";
            
            var parametros = new
            {
                recuperacaoParalelId,
                objetivoId,
                periodoRecuperacaoParalelaId
            };
           
            return await database.Conexao.QueryFirstOrDefaultAsync<RecuperacaoParalelaPeriodoObjetivoResposta>(query, parametros);
        }
    }
}