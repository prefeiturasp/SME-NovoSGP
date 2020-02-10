using Dapper;
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

        public async Task<PaginacaoResultadoDto<TipoAvaliacao>> ListarPaginado(string nome, string descricao, bool? situacao, Paginacao paginacao)
        {
            if (!string.IsNullOrEmpty(nome)) nome = $"%{nome.ToLowerInvariant()}%";
            if (!string.IsNullOrEmpty(descricao)) descricao = $"%{descricao.ToLowerInvariant()}%";

            StringBuilder query = new StringBuilder();
            MontaCabecalho(query, false);
            MontaFromWhere(query, nome, descricao, situacao, 0);

            if (paginacao == null)
                paginacao = new Paginacao(1, 10);
            if (paginacao.QuantidadeRegistros != 0)
                query.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            query.Append(";");

            MontaCabecalho(query, true);
            MontaFromWhere(query, nome, descricao, situacao, 0);
            query.Append(";");
            var retornoPaginado = new PaginacaoResultadoDto<TipoAvaliacao>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new
            {
                nome,
                descricao,
                situacao
            }))
            {
                retornoPaginado.Items = multi.Read<TipoAvaliacao>().ToList();
                retornoPaginado.TotalRegistros = multi.ReadFirst<int>();
            }

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);
            return retornoPaginado;
        }

        public async Task<bool> VerificarSeJaExistePorNome(string nome, string descricao, bool situacao, long id)
        {
            var query = new StringBuilder();
            MontaCabecalho(query, false);
            MontaFromWhere(query, nome, descricao, situacao, id);
            var resultado = (await database.Conexao.QueryAsync<TipoAvaliacao>(query.ToString(), new { nome, descricao, situacao, id }));
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
                query.AppendLine("excluido,");
                query.AppendLine("avaliacoes_necessarias_bimestre");
            }
        }

        private static void MontaFromWhere(StringBuilder query, string nome, string descricao, bool? situacao, long id)
        {
            query.AppendLine("from tipo_avaliacao");

            query.AppendLine("where excluido = false");
            if (situacao.HasValue)
            {
                query.AppendLine("and situacao = @situacao");
            }
            if (!string.IsNullOrEmpty(nome))
            {
                query.AppendLine("and lower(f_unaccent(nome)) LIKE f_unaccent(@nome) ");
                if (id > 0)
                    query.AppendLine("and id <> @id");
            }
            if (!string.IsNullOrEmpty(descricao))
            {
                query.AppendLine("and lower(f_unaccent(descricao)) LIKE f_unaccent(@descricao) ");
            }
        }

        public async Task<TipoAvaliacao> ObterTipoAvaliacaoBimestral()
        {
            var query = "select * from tipo_avaliacao where codigo = @tipoAvaliacao";
            return await database.Conexao.QueryFirstAsync<TipoAvaliacao>(query, new { tipoAvaliacao = TipoAvaliacaoCodigo.AvaliacaoBimestral });
        }
    }
}