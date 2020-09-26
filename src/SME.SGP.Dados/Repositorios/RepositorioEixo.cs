using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEixo : RepositorioBase<RecuperacaoParalelaEixo>, IRepositorioEixo
    {
        public RepositorioEixo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<EixoDto>> Listar(long RecuperacaoParalelaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            query.AppendLine("id,");
            query.AppendLine("recuperacao_paralela_periodo_id as PeriodoId,");
            query.AppendLine("descricao");
            query.AppendLine("from recuperacao_paralela_eixo");
            query.AppendLine("where (dt_fim is null or dt_fim <= now())");
            query.AppendLine("and excluido = false");
            //se não for encaminhamento, não traz os específicos do período
            if (RecuperacaoParalelaId != (int)PeriodoRecuperacaoParalela.Encaminhamento)
                query.AppendLine("and recuperacao_paralela_periodo_id is null");

            var listaRetorno = await database.Conexao.QueryAsync<EixoDto>(query.ToString(), new
            {
                RecuperacaoParalelaId
            });

            return listaRetorno;
        }
    }
}