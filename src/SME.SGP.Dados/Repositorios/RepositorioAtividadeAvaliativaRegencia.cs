using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeAvaliativaRegencia : RepositorioBase<AtividadeAvaliativaRegencia>, IRepositorioAtividadeAvaliativaRegencia
    {
        public RepositorioAtividadeAvaliativaRegencia(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AtividadeAvaliativaRegencia>> Listar(long idAtividadeAvaliativa)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT id,");
            query.AppendLine("atividade_avaliativa_id,");
            query.AppendLine("disciplina_contida_regencia_id,");
            query.AppendLine("criado_em,");
            query.AppendLine("criado_por,");
            query.AppendLine("alterado_em,");
            query.AppendLine("alterado_por,");
            query.AppendLine("criado_rf,");
            query.AppendLine("alterado_rf,");
            query.AppendLine("excluido");
            query.AppendLine("FROM atividade_avaliativa_regencia");
            query.AppendLine("WHERE atividade_avaliativa_id = @idAtividadeAvaliativa");
            query.AppendLine("AND excluido = false");

            return await database.Conexao.QueryAsync<AtividadeAvaliativaRegencia>(query.ToString(), new
            {
                idAtividadeAvaliativa
            });
        }
    }
}