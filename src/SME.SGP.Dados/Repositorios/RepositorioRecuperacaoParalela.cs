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

        public async Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(long turmaId, int periodoId)
        {
            var query = new StringBuilder();
            query.AppendLine(MontaCamposCabecalhoSimples());
            query.AppendLine("from recuperacao_paralela rec ");
            query.AppendLine("left join recuperacao_paralela_periodo_objetivo_resposta recRel on rec.id = recRel.id  ");
            query.AppendLine("where rec.turma_id = @turmaId ");
            query.AppendLine("and rec.periodo_recuperacao_paralela_id = @periodoId ");
            query.AppendLine("and rec.excluido = false ");
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { turmaId, periodoId });
        }

        private string MontaCamposCabecalhoSimples()
        {
            return @"select
                        rec.id ,
	                    rec.aluno_id AS AlunoId,
	                    rec.turma_id AS TurmaId,
	                    recRel.objetivo_id AS ObjetivoId,
	                    recRel.resposta_id AS RespostaId,
	                    recRel.periodo_recuperacao_paralela_id AS PeriodoRecuperacaoParalelaId ";
        }
    }
}