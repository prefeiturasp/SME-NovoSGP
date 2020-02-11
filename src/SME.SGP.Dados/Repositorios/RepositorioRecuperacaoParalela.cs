using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
            query.AppendLine("where rec.turma_recuperacao_paralela_id = @turmaId ");
            query.AppendLine("and recRel.periodo_recuperacao_paralela_id = @periodoId ");
            query.AppendLine("and rec.excluido = false ");
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalela>(query.ToString(), new { turmaId = turmaId.ToString(), periodoId });
        }

        public async Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto>> ListarTotalAlunosSeries(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, int? ano)
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
                            group by
	                            turma.nome,
	                            turma.ano,
	                            tipo_ciclo.descricao";
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalelaTotalAlunosAnoDto>(query.ToString(), new { turmaId });
        }

        public async Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto>> ListarTotalEstudantesPorFrequencia(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, int? ano)
        {
            //TODO: colocar os wheres
            string query = @"select
	                            count(aluno_id) as total,
	                            turma.ano,
	                            tipo_ciclo.descricao,
	                            resposta.nome as frequencia
                            from recuperacao_paralela rp
	                            inner join turma on rp.turma_id = turma.turma_id
	                            inner join tipo_ciclo_ano tca on turma.modalidade_codigo = tca.modalidade and turma.ano = tca.ano
	                            inner join tipo_ciclo on tca.tipo_ciclo_id = tipo_ciclo.id
	                            inner join recuperacao_paralela_periodo_objetivo_resposta rpp on rp.id = rpp.recuperacao_paralela_id
	                            inner join resposta on rpp.resposta_id = resposta.id
	                            where rpp.objetivo_id = 4
                            group by
	                            turma.nome,
	                            turma.ano,
	                            tipo_ciclo.descricao,
	                            resposta.nome";
            return await database.Conexao.QueryAsync<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto>(query, new { turmaId });
        }

        public async Task<PaginacaoResultadoDto<RetornoRecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, int? ano, int? pagina)
        {
            //a paginação desse ítem é diferente das outras, pois ela é determinada pela paginação da coluna pagina
            //ela não tem uma quantidade exata de ítens por página, apenas os objetivos daquele eixo, podendo variar para cada um
            if (pagina == 0) pagina = 1;
            //TODO: colocar os wheres
            //TODO: periodo 1 olhar bimestre 1,2...
            StringBuilder query = new StringBuilder();
            query.AppendLine("select");
            MontarCamposResumo(query);
            MontarFromResumo(query);
            //MontarWhere(query, periodo, dreId, ueId, cicloId, turmaId, ano, pagina);
            query.AppendLine("group by");
            query.AppendLine("turma.nome,");
            query.AppendLine("turma.ano,");
            query.AppendLine("tipo_ciclo.descricao,");
            query.AppendLine("resposta.nome,");
            query.AppendLine("o.nome,");
            query.AppendLine("e.descricao,");
            query.AppendLine("o.ordem,");
            query.AppendLine("tipo_ciclo.descricao,");
            query.AppendLine("e.id,");
            query.AppendLine("o.id,");
            query.AppendLine("resposta.id;");
            query.AppendLine("select max(pagina) from objetivo;");

            var parametros = new { dreId, ueId, cicloId, turmaId, ano, pagina };
            var retorno = new PaginacaoResultadoDto<RetornoRecuperacaoParalelaTotalResultadoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), parametros))
            {
                retorno.Items = multi.Read<RetornoRecuperacaoParalelaTotalResultadoDto>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = retorno.TotalRegistros;

            return retorno;
        }

        private static void MontarCamposResumo(StringBuilder query)
        {
            query.AppendLine("tipo_ciclo.descricao as ciclo,");
            query.AppendLine("count(aluno_id) as total,");
            query.AppendLine("turma.ano,");
            query.AppendLine("tipo_ciclo.descricao,");
            query.AppendLine("resposta.nome as resposta,");
            query.AppendLine("o.nome as objetivo,");
            query.AppendLine("e.descricao as eixo,");
            query.AppendLine("e.id as eixoId,");
            query.AppendLine("o.id as objetivoId,");
            query.AppendLine("resposta.id as respostaId");
        }

        private static void MontarFromResumo(StringBuilder query)
        {
            query.AppendLine("from recuperacao_paralela rp");
            query.AppendLine("inner join turma on rp.turma_id = turma.turma_id");
            query.AppendLine("inner join tipo_ciclo_ano tca on turma.modalidade_codigo = tca.modalidade and turma.ano = tca.ano");
            query.AppendLine("inner join tipo_ciclo on tca.tipo_ciclo_id = tipo_ciclo.id");
            query.AppendLine("inner join recuperacao_paralela_periodo_objetivo_resposta rpp on rp.id = rpp.recuperacao_paralela_id");
            query.AppendLine("inner join resposta on rpp.resposta_id = resposta.id");
            query.AppendLine("inner join objetivo o on rpp.objetivo_id = o.id");
            query.AppendLine("inner join eixo e on o.eixo_id = e.id");
            query.AppendLine("where e.excluido = false");
            query.AppendLine("and o.excluido = false");
            query.AppendLine("and resposta.excluido = false");
        }

        private static void MontarWhere(StringBuilder query, int? periodo, string dreId, string ueId, int? cicloId, string turmaId, int? ano, int? pagina)
        {
            //para grafico deve enviar nulo
            if (pagina.HasValue)
                query.AppendLine("and o.pagina = @pagina");
            if (periodo.HasValue && (PeriodoRecuperacaoParalela)periodo == PeriodoRecuperacaoParalela.Encaminhamento)
                query.AppendLine("and e.id NOT IN (2)");
            else
            {
                query.AppendLine("and e.id NOT IN (1)");
            }
            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and turma.");
            query.AppendLine("count(aluno_id) as total,");
            query.AppendLine("turma.ano,");
            query.AppendLine("tipo_ciclo.descricao,");
            query.AppendLine("resposta.nome as resposta,");
            query.AppendLine("o.nome as objetivo,");
            query.AppendLine("e.descricao as eixo,");
            query.AppendLine("e.id as eixoId,");
            query.AppendLine("o.id as objetivoId,");
            query.AppendLine("resposta.id as respostaId");
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