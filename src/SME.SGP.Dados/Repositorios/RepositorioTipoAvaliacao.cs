using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoAvaliacao : RepositorioBase<TipoAvaliacao>, IRepositorioTipoAvaliacao
    {
        public RepositorioTipoAvaliacao(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<TipoAvaliacao>> ListarPaginado(string nome, Paginacao paginacao)
        {
            if (!string.IsNullOrEmpty(nome)) nome = $"%{nome.ToLowerInvariant()}%";
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query, false);
            MontaFromWhere(query, nome, 0);

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);
            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            query.Append(";");

            MontaCabecalho(query, true);
            MontaFromWhere(query, nome, 0);
            query.Append(";");
            var retornoPaginado = new PaginacaoResultadoDto<TipoAvaliacao>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new
            {
                nome
            }))
            {
                retornoPaginado.Items = multi.Read<TipoAvaliacao>().ToList();
                retornoPaginado.TotalRegistros = multi.ReadFirst<int>();
            }

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        public async Task<bool> VerificarSeJaExistePorNome(string nome, long id)
        {
            var query = new StringBuilder();
            MontaCabecalho(query, false);
            MontaFromWhere(query, nome, id);
            var resultado = (await database.Conexao.QueryAsync<TipoAvaliacao>(query.ToString(), new { nome, id }));
            return resultado.Any();
        }

        private static void MontaCabecalho(StringBuilder query, bool ehCount)
        {
            if (ehCount)
                query.AppendLine("select count(*)");
            else
            {
                query.AppendLine("select id,");
                query.AppendLine("nome,");
                query.AppendLine("descricao,");
                query.AppendLine("situacao,");
                query.AppendLine("criado_em,");
                query.AppendLine("criado_por,");
                query.AppendLine("alterado_em,");
                query.AppendLine("alterado_por,");
                query.AppendLine("criado_rf,");
                query.AppendLine("alterado_rf,");
                query.AppendLine("alterado_rf,");
                query.AppendLine("excluido");
            }
        }

        private static void MontaFromWhere(StringBuilder query, string nome, long id)
        {
            query.AppendLine("from tipo_avaliacao");
            query.AppendLine("where situacao = true");
            query.AppendLine("and excluido = false");
            if (!string.IsNullOrEmpty(nome))
            {
                query.AppendLine("and lower(f_unaccent(nome)) LIKE f_unaccent(@nome) ");
                if (id > 0)
                    query.AppendLine("and id <> @id");
            }
        }
    }
}