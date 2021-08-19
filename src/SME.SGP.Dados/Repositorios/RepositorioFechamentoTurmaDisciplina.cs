using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurmaDisciplina : RepositorioBase<FechamentoTurmaDisciplina>,
        IRepositorioFechamentoTurmaDisciplina
    {
        private readonly IRepositorioTurma repositorioTurma;

        public RepositorioFechamentoTurmaDisciplina(ISgpContext database, IRepositorioTurma repositorioTurma) :
            base(database)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<int>> ObterDisciplinaIdsPorTurmaIdBimestre(long turmaId, int bimestre)
        {
            var query = new StringBuilder(
                @"select disciplina_id as ComponenteCurricularId from fechamento_turma_disciplina ftd 
                            inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                            left join periodo_escolar pe on ft.periodo_escolar_id = pe.id 
                            where ft.turma_id = @turmaId");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");

            return await database.Conexao.QueryAsync<int>(query.ToString(), new { turmaId, bimestre });
        }

        public async Task<bool> AtualizarSituacaoFechamento(long fechamentoTurmaDisciplinaId, int situacaoFechamento)
        {
            var query = @"update fechamento_turma_disciplina 
                             set situacao = @situacaoFechamento
                         where id = @fechamentoTurmaDisciplinaId";

            await database.Conexao.ExecuteAsync(query, new { fechamentoTurmaDisciplinaId, situacaoFechamento });
            return true;
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId,
            long[] disciplinasId, int bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*, fa.*, ft.*, p.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id 
                        inner join turma t on t.id = ft.turma_id
                        inner join fechamento_aluno fa on f.id = fa.fechamento_turma_disciplina_id
                        where not f.excluido
                            and t.id = @turmaId ");

            if (disciplinasId != null && disciplinasId.Length > 0)
                query.AppendLine("and f.disciplina_id = ANY(@disciplinasId)");

            if (bimestre > 0)
                query.AppendLine("and p.bimestre = @bimestre ");
            else
                query.AppendLine("and ft.periodo_escolar_id is null");

            IList<FechamentoTurmaDisciplina> fechammentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();

            await database.Conexao
                .QueryAsync<FechamentoTurmaDisciplina, FechamentoAluno, FechamentoTurma, PeriodoEscolar,
                    FechamentoTurmaDisciplina>
                (query.ToString(), (fechamentoTurmaDiscplina, fechamentoAluno, fechamentoTurma, periodoEscolar) =>
                {
                    var fechamentoTurmaDisciplinaLista =
                        fechammentosTurmaDisciplina.FirstOrDefault(ftd => ftd.Id == fechamentoTurmaDiscplina.Id);
                    if (fechamentoTurmaDisciplinaLista == null)
                    {
                        if (periodoEscolar != null)
                            fechamentoTurma.AdicionarPeriodoEscolar(periodoEscolar);

                        fechamentoTurmaDiscplina.FechamentoTurma = fechamentoTurma;

                        fechamentoTurmaDisciplinaLista = fechamentoTurmaDiscplina;
                        fechammentosTurmaDisciplina.Add(fechamentoTurmaDiscplina);
                    }

                    fechamentoTurmaDisciplinaLista.FechamentoAlunos.Add(fechamentoAluno);
                    return fechamentoTurmaDiscplina;
                }, new { turmaId, disciplinasId, bimestre });

            return fechammentosTurmaDisciplina;
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(string turmaCodigo,
            long[] disciplinasId, int bimestre = 0)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);

            if (turma == null)
                return Enumerable.Empty<FechamentoTurmaDisciplina>();

            return await ObterFechamentosTurmaDisciplinas(turma.Id, disciplinasId, bimestre);
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo,
            long disciplinaId, int bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id
                        inner join turma t on t.id = ft.turma_id
                        where not f.excluido
                          and t.turma_id = @turmaCodigo
                          and f.disciplina_id = @disciplinaId ");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurmaDisciplina>(query.ToString(),
                new { turmaCodigo, disciplinaId, bimestre });
        }

        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno,
            long fechamentoTurmaDisciplinaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, 
                                 n.nota as Nota, 
                                 n.conceito_id as ConceitoId, 
                                 fa.aluno_codigo as CodigoAluno, 
                                 n.sintese_id as SinteseId,
                                 n.criado_em CriadoEm,
                                 n.criado_rf CriadoRf,
                                 n.criado_por CriadoPor,
                                 n.alterado_em AlteradoEm,
                                 n.alterado_rf AlteradoRf,
                                 n.alterado_por AlteradoPor
                         from fechamento_nota n
                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                        where not n.excluido
                            and fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                            and fa.aluno_codigo = @codigoAluno ";

            return await database.Conexao.QueryAsync<FechamentoNotaDto>(query,
                new { codigoAluno, fechamentoTurmaDisciplinaId });
        }

        public override FechamentoTurmaDisciplina ObterPorId(long id)
        {
            var query = @"select ftd.*, ft.* 
                            from fechamento_turma_disciplina ftd
                          inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                          where ftd.id = @id";

            return database.Conexao.Query<FechamentoTurmaDisciplina, FechamentoTurma, FechamentoTurmaDisciplina>(query
                , (fechamentoTurmaDisciplina, fechamentoTurma) =>
                {
                    fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
                    return fechamentoTurmaDisciplina;
                }
                , new { id }).FirstOrDefault();
        }

        public async Task<SituacaoFechamento> ObterSituacaoFechamento(long turmaId, long componenteCurricularId,
            long periodoEscolarId)
        {
            var query = @"select ftd.situacao
                         from fechamento_turma_disciplina ftd
                        inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                        where ftd.disciplina_id = @componenteCurricularId
                          and ft.turma_id = @turmaId
                          and ft.periodo_escolar_id = @periodoEscolarId ";

            return (SituacaoFechamento)await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaId, componenteCurricularId, periodoEscolarId });
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>>
            ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(int anoLetivo)
        {
            var sqlQuery = @"select distinct ftd.*
	                         from fechamento_turma_disciplina ftd 
		                        inner join fechamento_turma ft
			                        on ftd.fechamento_turma_id = ft.id
		                        inner join turma t
			                        on ft.turma_id = t.id
                             where t.ano_letivo = @anoLetivo and
	                              ftd.situacao = @situacao and
	                              not ftd.excluido and
	                              not ft.excluido;";

            return await database.Conexao.QueryAsync<FechamentoTurmaDisciplina>(sqlQuery,
                new { anoLetivo, situacao = SituacaoFechamento.EmProcessamento });
        }

        public async Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamento(long ueId,
            int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = @"select
                                    ftd.situacao as Situacao, count(ftd.id) as Quantidade, t.ano as Ano, t.modalidade_codigo  as Modalidade
                                    from fechamento_turma_disciplina ftd
                                    inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                                    inner join turma t on ft.turma_id = t.id
                                    inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                    inner join ue on ue.id = t.ue_id 
                                    where 1=1 ";

            var queryBuilder = new StringBuilder(sqlQuery);

            if (ano > 0)
            {
                queryBuilder.Append(" and t.ano_letivo = @ano ");
            }

            if (ueId > 0)
            {
                queryBuilder.Append(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                queryBuilder.Append(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                queryBuilder.Append(" and t.modalidade_codigo = @modalidade ");
            }

            if (semestre > 0)
            {
                queryBuilder.Append(" and t.semestre = @semestre ");
            }

            if (bimestre >= 0) 
            {
                queryBuilder.Append(" and pe.bimestre = @bimestre ");
            }

            queryBuilder.Append(@"group by ftd.situacao, t.ano , t.modalidade_codigo order by t.ano;");


            return await database.Conexao.QueryAsync<FechamentoSituacaoQuantidadeDto>(queryBuilder.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        public async Task<IEnumerable<FechamentoPendenciaQuantidadeDto>> ObterSituacaoPendenteFechamento(long ueId,
            int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            
            var campoAnoOuTurma = ueId > 0 ? "t.nome " : "t.ano ";
            
            var sqlQuery = $@"select cast({campoAnoOuTurma} as varchar(255)) as Ano , t.modalidade_codigo as Modalidade,  count(pf.id) as Quantidade
                                from pendencia_fechamento pf
                                inner join fechamento_turma_disciplina ftd  on ftd.id = pf.fechamento_turma_disciplina_id 
                                inner join fechamento_turma ft  on ft.id = ftd.fechamento_turma_id 
                                inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                inner join turma t on ft.turma_id = t.id 
                                inner join ue on ue.id = t.ue_id where 1=1 ";

            var queryBuilder = new StringBuilder(sqlQuery);

            if (ano > 0)
            {
                queryBuilder.Append(" and t.ano_letivo = @ano ");
            }

            if (ueId > 0)
            {
                queryBuilder.Append(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                queryBuilder.Append(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                queryBuilder.Append(" and t.modalidade_codigo = @modalidade ");
            }

            if (semestre > 0)
            {
                queryBuilder.Append(" and t.semestre = @semestre ");
            }

            if (bimestre >= 0)
            {
                queryBuilder.Append(" and pe.bimestre = @bimestre ");
            }

            var orderBy = ueId > 0 ? "order by t.nome " : "order by t.ano ";
            var gruopBy = ueId > 0 ? "group by t.nome" : "group by t.ano";
            
            queryBuilder.Append($"{gruopBy} , t.modalidade_codigo {orderBy};");


            return await database.Conexao.QueryAsync<FechamentoPendenciaQuantidadeDto>(queryBuilder.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }
        public async Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamentoPorEstudante(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = @"select
                                    distinct ftd.situacao as Situacao, count(ftd.id) as Quantidade, t.ano as Ano, t.modalidade_codigo  as Modalidade
                                    from fechamento_turma_disciplina ftd
                                    inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                                    inner join turma t on ft.turma_id = t.id
                                    inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                    inner join ue on ue.id = t.ue_id 
                                    where 1=1 ";

            var queryBuilder = new StringBuilder(sqlQuery);

            if (ano > 0)
            {
                queryBuilder.Append(" and t.ano_letivo = @ano ");
            }

            if (ueId > 0)
            {
                queryBuilder.Append(" and t.ue_id = @ueId ");    
            }
            
            if (dreId > 0)
            {
                queryBuilder.Append(" and ue.dre_id = @dreId ");    
            }

            if (modalidade > 0)
            {
                queryBuilder.Append(" and t.modalidade_codigo = @modalidade ");   
                
            }
            
            if (semestre > 0)
            {
                queryBuilder.Append(" and t.semestre = @semestre ");   
                
            }
            
            if (bimestre > 0)
            {
                queryBuilder.Append(" and pe.bimestre = @bimestre ");   
                
            }
            
            queryBuilder.Append(@"group by ftd.situacao, t.ano , t.modalidade_codigo order by t.ano;");
            
            
            return await database.Conexao.QueryAsync<FechamentoSituacaoQuantidadeDto>(queryBuilder.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade, 
                semestre,
                bimestre
            });
        }

        public async Task<IEnumerable<TurmaFechamentoDisciplinaDto>> ObterTotalDisciplinasPorTurma(int anoLetivo, int bimestre)
        {
            var query = new StringBuilder(@"select cfct.turma_id as TurmaId, count(componente_curricular_id) as QuantidadeDisciplinas 
                from consolidado_fechamento_componente_turma cfct 
                inner join turma t on cfct.turma_id = t.id
                where t.ano_letivo = @anoLetivo ");

            if(bimestre >= 0)
                query.AppendLine(" and cfct.bimestre = @bimestre ");

            query.AppendLine("group by cfct.turma_id order by 1");
                

            return await database.Conexao.QueryAsync<TurmaFechamentoDisciplinaDto>(query.ToString(), new { anoLetivo, bimestre });
        }
    }
}