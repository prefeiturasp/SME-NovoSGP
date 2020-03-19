using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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

        public async Task<IEnumerable<AtividadeAvaliativaRegencia>> ObterAvaliacoesBimestrais(long tipoCalendarioId, string turmaId, string disciplinaId, int bimestre)
        {
            var query = @"select aar.* 
                          from atividade_avaliativa_regencia aar 
                         inner join atividade_avaliativa aa on aa.id = aar.atividade_avaliativa_id 
                         inner join tipo_avaliacao t on t.id = aa.tipo_avaliacao_id and t.codigo = 1
                         inner join periodo_escolar p on p.periodo_inicio <= aa.data_avaliacao and p.periodo_fim >= aa.data_avaliacao 
                         where not aa.excluido 
                           and aar.disciplina_contida_regencia_id = @disciplinaId
                           and aa.turma_id = @turmaId
                           and p.bimestre = @bimestre 
                           and p.tipo_calendario_id = @tipoCalendarioId ";

            return await database.Conexao.QueryAsync<AtividadeAvaliativaRegencia>(query, new { tipoCalendarioId, disciplinaId, turmaId, bimestre });
        }
    }
}