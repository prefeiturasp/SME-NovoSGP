using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurma : IRepositorioConsolidacaoFrequenciaTurma
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoFrequenciaTurma(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoFrequenciaTurma consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano)
        {
            var query = @"delete from consolidacao_frequencia_turma
                        where turma_id in (
                            select id from turma where ano_letivo = @ano)";

            await database.Conexao.ExecuteScalarAsync(query, new { ano });
        }

        public async Task<long> InserirConsolidacaoDashBoard(ConsolidacaoDashBoardFrequencia consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task AlterarConsolidacaoDashboardTurmaMesPeriodoAno(long id, int quantidadePresente, int quantidadeAusente, int quantidadeRemoto)
        {
            string query = @"update consolidado_dashboard_frequencia 
                                    set quantidade_presencas = @quantidadePresente, 
                                    quantidade_ausencias = @quantidadeAusente,
                                    quantidade_remotos = @quantidadeRemoto
                                    where id = @id";

            await database.Conexao.ExecuteAsync(query, new { id, quantidadePresente, quantidadeAusente, quantidadeRemoto});
        }
        
        public async Task<ConsolidacaoDashBoardFrequencia> ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipo(long turmaId, DateTime dataAula, Modalidade modalidadeCodigo, int anoLetivo, long dreId, long ueId, TipoPeriodoDashboardFrequencia tipo)
        {
            var query = new StringBuilder(@"SELECT id, 
			                                     turma_nome, 
			                                     turma_id,
			                                     turma_ano, 
			                                     semestre, 
			                                     data_aula, 
			                                     modalidade_codigo, 
			                                     data_inicio_semana, 
			                                     data_fim_semana, 
			                                     mes, 
			                                     tipo, 
			                                     ano_letivo, 
			                                     dre_id, 
			                                     ue_id, 
			                                     dre_codigo, 
			                                     dre_abreviacao, 
			                                     quantidade_presencas, 
			                                     quantidade_ausencias, 
			                                     quantidade_remotos, 
			                                     criado_em
                                    FROM consolidado_dashboard_frequencia
                                    where turma_id = @turmaId 
                                       and data_aula = @dataAula 
                                       and modalidade_codigo = @modalidadeCodigo 
                                       and ano_letivo = @anoLetivo 
                                       and dre_id = @dreId 
                                       and ue_id = @ueId
                                       and tipo = @tipo ");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidacaoDashBoardFrequencia>(query.ToString(), new { turmaId, dataAula, modalidadeCodigo = (int)modalidadeCodigo, anoLetivo, dreId, ueId, tipo = (int)tipo});
        }
        
        public async Task<long> SalvarConsolidacaoDashBoardFrequencia(ConsolidacaoDashBoardFrequencia consolidacaoDashBoardFrequencia)
        {
            if (consolidacaoDashBoardFrequencia.Id > 0)
                await database.Conexao.UpdateAsync(consolidacaoDashBoardFrequencia);
            else
                consolidacaoDashBoardFrequencia.Id = (long)(await database.Conexao.InsertAsync(consolidacaoDashBoardFrequencia));

            return consolidacaoDashBoardFrequencia.Id;
        }
        
        public async Task<long> SalvarConsolidacaoFrequenciaTurma(ConsolidacaoFrequenciaTurma consolidacaoFrequenciaTurma)
        {
	        if (consolidacaoFrequenciaTurma.Id > 0)
		        await database.Conexao.UpdateAsync(consolidacaoFrequenciaTurma);
	        else
		        consolidacaoFrequenciaTurma.Id = (long)(await database.Conexao.InsertAsync(consolidacaoFrequenciaTurma));

	        return consolidacaoFrequenciaTurma.Id;
        }

        public async Task<ConsolidacaoFrequenciaTurma> ObterConsolidacaoDashboardPorTurmaETipoPeriodo(long turmaId, TipoConsolidadoFrequencia tipoConsolidacao, DateTime? periodoInicio, DateTime? periodoFim)
        {
            var query = new StringBuilder(@"SELECT id, 
			                                       turma_id,
			                                       quantidade_acima_minimo_frequencia,
			                                       quantidade_abaixo_minimo_frequencia,
			                                       tipo_consolidacao,
			                                       periodo_inicio,
			                                       periodo_fim
                                    FROM consolidacao_frequencia_turma
									where turma_id = @turmaId 
									      and tipo_consolidacao = @tipo ");

            if (periodoInicio.HasValue && periodoFim.HasValue)
	            query.Append(" and periodo_inicio = @periodoInicio and periodo_fim = @periodoFim ");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidacaoFrequenciaTurma>(query.ToString(), new { turmaId, tipo = (int)tipoConsolidacao, periodoInicio, periodoFim });

        }
    }
}