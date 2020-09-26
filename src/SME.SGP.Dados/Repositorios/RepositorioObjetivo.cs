using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivo : RepositorioBase<RecuperacaoParalelaObjetivo>, IRepositorioObjetivo
    {
        public RepositorioObjetivo(ISgpContext database) : base(database)
        { }

        public async Task<IEnumerable<ObjetivoDto>> Listar(long periodoId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("o.id,");
            query.AppendLine("o.eixo_id,");
            query.AppendLine("o.nome,");
            query.AppendLine("o.descricao,");
            query.AppendLine("o.ordem");
            query.AppendLine("from recuperacao_paralela_objetivo o ");
            query.AppendLine("inner join recuperacao_paralela_eixo e on o.eixo_id = e.id ");
            query.AppendLine("where (o.dt_fim is null or o.dt_fim <= now())");
            query.AppendLine("and o.excluido = false");
            //se não for encaminhamento, não traz os específicos do período
            if (periodoId != (int)PeriodoRecuperacaoParalela.Encaminhamento)
                query.AppendLine("and e.recuperacao_paralela_periodo_id is null");
            else
                query.AppendLine("and o.EhEspecifico = false");

            var listaRetorno = await database.Conexao.QueryAsync<ObjetivoDto>(query.ToString());

            return listaRetorno;
        }
    }
}