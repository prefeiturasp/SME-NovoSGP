using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRecuperacaoParalela : RepositorioBase<RecuperacaoParalela>, IRepositorioRecuperacaoParalela
    {
        public RepositorioRecuperacaoParalela(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(long turmaId, long periodoId)
        {
            var query = new StringBuilder();
            query.AppendLine(MontaCamposCabecalho());
            query.AppendLine("from recuperacao_paralela rec ");
            query.AppendLine("inner join recuperacao_paralela_periodo_objetivo_resposta recRel on rec.id = recRel.recuperacao_paralela_id   ");
            query.AppendLine("where rec.turma_id = @turmaId ");
            query.AppendLine("and recRel.periodo_recuperacao_paralela_id = @periodoId ");
            query.AppendLine("and rec.excluido = false ");
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { turmaId, periodoId });
        }

        private string MontaCamposCabecalho()
        {
            return @"select
                        rec.id ,
	                    rec.aluno_id AS AlunoId,
	                    rec.turma_id AS TurmaId,
                        rec.criado_por,
                        rec.criado_em,
                        rec.criado_rf,
                        rec.alterado_por,
                        rec.alterado_em,
                        rec.alterado_rf,
	                    recRel.objetivo_id AS ObjetivoId,
	                    recRel.resposta_id AS RespostaId,
	                    recRel.periodo_recuperacao_paralela_id AS PeriodoRecuperacaoParalelaId ";
        }
    }
}