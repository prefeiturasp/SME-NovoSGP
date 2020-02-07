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

        public async Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(string turmaId, long periodoId)
        {
            var query = new StringBuilder();
            query.AppendLine(MontaCamposCabecalho());
            query.AppendLine("from recuperacao_paralela rec ");
            query.AppendLine("inner join recuperacao_paralela_periodo_objetivo_resposta recRel on rec.id = recRel.recuperacao_paralela_id   ");
            query.AppendLine("where rec.turma_recuperacao_paralela_id = @turmaId ");
            query.AppendLine("and recRel.periodo_recuperacao_paralela_id = @periodoId ");
            query.AppendLine("and rec.excluido = false ");
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { turmaId, periodoId });
        }

        public async Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto>> ListarTotalAlunosSeries(long dreId, long ueId, int cicloId, int turmaId, int ano)
        {
            //TODO: colocar os wheres
            string query = @"select
	                            count(aluno_id) as total,
	                            turma.ano,
	                            tipo_ciclo.descricao as Ciclo
                            from recuperacao_paralela rp
	                            inner join turma on rp.turma_id = turma.turma_id
	                            inner join tipo_ciclo_ano tca on turma.modalidade_codigo = tca.modalidade and turma.ano = tca.ano
	                            inner join tipo_ciclo on tca.tipo_ciclo_id = tipo_ciclo.id
	                            inner join recuperacao_paralela_periodo_objetivo_resposta rpp on rp.id = rpp.recuperacao_paralela_id
	                            where rpp.objetivo_id = 4
                            group by
	                            turma.nome,
	                            turma.ano,
	                            tipo_ciclo.descricao";
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalelaTotalAlunosAnoDto>(query.ToString(), new { turmaId });
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