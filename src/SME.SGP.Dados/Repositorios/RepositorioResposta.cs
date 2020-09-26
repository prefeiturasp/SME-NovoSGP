using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioResposta : RepositorioBase<RecuperacaoParalelaResposta>, IRepositorioResposta
    {
        public RepositorioResposta(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<RespostaDto>> Listar(long periodoId)
        {
            var query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("r.id,");
            query.AppendLine("r.nome,");
            query.AppendLine("r.descricao,");
            query.AppendLine("r.sim,");
            query.AppendLine("ob.id as objetivoId");
            query.AppendLine("from recuperacao_paralela_resposta r");
            query.AppendLine("inner join recuperacao_paralela_objetivo_resposta o on r.id = o.resposta_id");
            query.AppendLine("inner join recuperacao_paralela_objetivo ob on o.objetivo_id = ob.id");
            query.AppendLine("inner join recuperacao_paralela_eixo e on ob.eixo_id = e.id");
            query.AppendLine("where r.excluido = false");
            query.AppendLine("and o.excluido = false");
            query.AppendLine("and (r.dt_fim is null or r.dt_fim <= now())");
            //se não for encaminhamento, não traz os específicos do período
            if (periodoId != (int)PeriodoRecuperacaoParalela.Encaminhamento)
                query.AppendLine("and e.recuperacao_paralela_periodo_id is null");
            query.AppendLine("order by ob.ordem, r.ordem");
            return await database.Conexao.QueryAsync<RespostaDto>(query.ToString());
        }
    }
}