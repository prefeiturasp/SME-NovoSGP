using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDashBoardFrequencia : IRepositorioDashBoardFrequencia
    {
        private readonly ISgpContextConsultas database;

        public RepositorioDashBoardFrequencia(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
        {
            var query = new StringBuilder(@"select x.ano as ano, ");

            if (visaoDre)
                query.AppendLine("x.DescricaoAnoTurma, x.abreviacao as DreAbreviacao ");
            else if (ueId == -99)
                query.AppendLine("x.DescricaoAnoTurma ");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("x.DescricaoAnoTurma ");

            query.AppendLine(@" , count(*) filter(where x.QuantidadePresencas > 0) as Presentes
                                , count(*) filter(where x.QuantidadeRemotos > 0) as Remotos
                                , count(*) filter(where x.QuantidadeAusencias > 0 and x.QuantidadePresencas=0 and x.QuantidadeRemotos=0) as Ausentes
                                , x.ModalidadeCodigo                                
                            from
                            ( ");

            query.AppendLine("select rfa.codigo_aluno, t.ano, ");

            if (visaoDre)
                query.AppendLine("dre.dre_id as DescricaoAnoTurma, dre.abreviacao, ");
            else if (ueId == -99)
                query.AppendLine("t.ano as DescricaoAnoTurma, ");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("t.nome as DescricaoAnoTurma, ");

            query.AppendLine(@"t.modalidade_codigo as ModalidadeCodigo,
                               count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
                               count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                               count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos
                          from registro_frequencia_aluno rfa
                         inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id
                         inner join aula a on a.id = rf.aula_id
                         inner join turma t on t.turma_id = a.turma_id
                         inner join ue on ue.id = t.ue_id
                         inner join dre on dre.id = ue.dre_id
                         where t.ano_letivo = @anoLetivo
                           and not rfa.excluido
                           and t.modalidade_codigo = @modalidade ");

            if (dreId != -99)
                query.AppendLine("and dre.id = @dreId ");

            if (ueId != -99)
                query.AppendLine("and ue.id = @ueId ");

            if (semestre > 0)
                query.AppendLine("and t.semestre = @semestre ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and a.data_aula = @dataInicio ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine("and a.data_aula between @dataInicio and @dataFim ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine(@"and extract(month from a.data_aula) = @mes 
                                   and extract(year from a.data_aula) = @anoLetivo ");

            query.AppendLine("group by rfa.codigo_aluno, t.ano, ");

            if (visaoDre)
                query.AppendLine(" dre.dre_id, dre.abreviacao, t.modalidade_codigo");
            else if (ueId == -99)
                query.AppendLine(" t.ano, t.modalidade_codigo");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine(" t.ano, t.nome, t.modalidade_codigo");

            query.AppendLine(@") as x ");

            if (visaoDre)
                query.AppendLine("group by x.ano, x.DescricaoAnoTurma, x.abreviacao, x.ModalidadeCodigo");
            else if (ueId == -99)
                query.AppendLine("group by x.ano, x.DescricaoAnoTurma, x.ModalidadeCodigo");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("group by x.ano, x.DescricaoAnoTurma, x.ModalidadeCodigo");

            return await database.Conexao.QueryAsync<FrequenciaAlunoDashboardDto>(query.ToString(), new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                semestre,
                dataInicio,
                datafim,
                mes
            });
        }

        public async Task<DadosParaConsolidacaoDashBoardFrequenciaDto> ObterDadosParaConsolidacao(int anoLetivo, long turmaId, int modalidade, int tipoPeriodo, DateTime dataInicio, DateTime datafim, int mes)
        {
            var query = new StringBuilder(@"select 
 	                                           count(*) filter(where x.QuantidadePresencas > 0) as Presentes
 	                                         , count(*) filter(where x.QuantidadeRemotos > 0) as Remotos
 	                                         , count(*) filter(where x.QuantidadeAusencias > 0 and x.QuantidadePresencas=0 and x.QuantidadeRemotos=0) as Ausentes 	                            
                                         from
                                             ( 
		                                        select rfa.codigo_aluno,			   
                                                       count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
			                                           count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                                                       count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos              
                                                  from registro_frequencia_aluno rfa
                                                 inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id
                                                 inner join aula a on a.id = rf.aula_id
                                                 inner join turma t on t.turma_id = a.turma_id
                                                 inner join ue on ue.id = t.ue_id
                                                 inner join dre on dre.id = ue.dre_id
                                                 where t.ano_letivo = @anoLetivo
                                                   and not rfa.excluido
                                                   and t.modalidade_codigo = @modalidade 
                                                   and t.id = @turmaId ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and a.data_aula = @dataInicio ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine("and a.data_aula between @dataInicio and @dataFim ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine(@"and extract(month from a.data_aula) = @mes 
                                   and extract(year from a.data_aula) = @anoLetivo ");

            query.AppendLine(@"group by rfa.codigo_aluno
                               ) as x ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                modalidade,
                tipoPeriodo,
                dataInicio,
                datafim,
                mes
            };

            return await database.Conexao.QueryFirstOrDefaultAsync<DadosParaConsolidacaoDashBoardFrequenciaDto>(query.ToString(), parametros);
        }
        public async Task<long> Inserir(ConsolidacaoDashBoardFrequencia consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }
    }
}
