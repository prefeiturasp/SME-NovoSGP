using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivo : RepositorioBase<Objetivo>, IRepositorioObjetivo
    {
        public RepositorioObjetivo(ISgpContext database) : base(database)
        { }

        public async Task<IEnumerable<ObjetivoDto>> ListarObjetivos()
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("eixo_id,");
            query.AppendLine("nome,");
            query.AppendLine("descricao");
            query.AppendLine("from objetivo");
            query.AppendLine("where (dt_fim is null or dt_fim <= now())");
            query.AppendLine("and excluido = false");
            var listaRetorno = await database.Conexao.QueryAsync<ObjetivoDto>(query.ToString());

            return listaRetorno;
        }
    }
}