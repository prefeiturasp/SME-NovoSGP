﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaAulaConsulta : IRepositorioPendenciaAulaConsulta
    {
        private readonly ISgpContextConsultas database;
        private readonly IRepositorioDreConsulta repositorioDre;

        public RepositorioPendenciaAulaConsulta(ISgpContextConsultas database, IRepositorioDreConsulta repositorioDre)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, long dreId, long ueId, int anoLetivo)
        {
            var listaRetorno = new List<Aula>();
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select distinct a.id as Id,");
            sqlQuery.AppendLine("                a.disciplina_id DisciplinaId,");
            sqlQuery.AppendLine("                a.turma_id TurmaId,");
            sqlQuery.AppendLine("                a.professor_rf ProfessorRf,");
            sqlQuery.AppendLine("                a.tipo_calendario_id TipoCalendarioId,");
            sqlQuery.AppendLine("                a.data_aula DataAula,");
            sqlQuery.AppendLine("                a.aula_cj AulaCJ,");
            sqlQuery.AppendLine("                t.id Id,");
            sqlQuery.AppendLine("                t.modalidade_codigo ModalidadeCodigo");
            sqlQuery.AppendLine("  	from aula a");
            sqlQuery.AppendLine("  		inner join tipo_calendario tc");
            sqlQuery.AppendLine("  			on a.tipo_calendario_id = tc.id");
            if (tipoPendenciaAula == TipoPendencia.Frequencia)
            {
                sqlQuery.AppendLine("  		inner join componente_curricular cc");
                sqlQuery.AppendLine("            on cc.permite_registro_frequencia and a.disciplina_id::int8 = cc.id");
            }
            sqlQuery.AppendLine("  		inner join turma t");
            sqlQuery.AppendLine("  			on a.turma_id = t.turma_id");
            sqlQuery.AppendLine("  		inner join ue");
            sqlQuery.AppendLine("  			on t.ue_id = ue.id");

            sqlQuery.AppendLine("  		left join pendencia_aula pa");
            sqlQuery.AppendLine("  			on pa.aula_id = a.id");
            sqlQuery.AppendLine("  		left join pendencia p");
            sqlQuery.AppendLine("  			on p.id = pa.pendencia_id");
            sqlQuery.AppendLine("  			and not p.excluido");
            sqlQuery.AppendLine("  			and p.tipo = @tipo");

            sqlQuery.AppendLine($"  	left join {tabelaReferencia} tf");
            sqlQuery.AppendLine($"  		on tf.aula_id = a.id");

            sqlQuery.AppendLine("  where not a.excluido");
            sqlQuery.AppendLine("	and a.data_aula < @hoje");
            sqlQuery.AppendLine("	and ue.dre_id = @dreId");
            sqlQuery.AppendLine("   and t.modalidade_codigo = ANY(@modalidades)");

            sqlQuery.AppendLine("	and p.id is null");
            sqlQuery.AppendLine("	and tf.id is null");

            if (ueId > 0)
                sqlQuery.AppendLine("    and ue.id = @ueId ");

            return await database.Conexao.QueryAsync<Aula, Turma, Aula>(sqlQuery.ToString(), (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            }, new
            {
                hoje = DateTime.Today.Date,
                tipo = tipoPendenciaAula,
                modalidades,
                dreId,
                ueId,
            }, splitOn: "Id", commandTimeout: 60);
        }

        public async Task<bool> PossuiPendenciasPorTipo(string disciplinaId, string turmaId, TipoPendencia tipoPendenciaAula, int bimestre, bool professorCj, bool professorTitular, string professorRf = "")
        {
            var sqlQuery = new StringBuilder(@"select 1 
                          from pendencia_aula pa
                         inner join pendencia p on p.id = pa.pendencia_id 
                         inner join aula a on a.id = pa.aula_id 
                         inner join tipo_calendario tc on tc.id = a.tipo_calendario_id 
                         inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id ");

            if (professorTitular)
                sqlQuery.AppendLine(" and not a.aula_cj ");
            else if (professorCj)
                sqlQuery.AppendLine(" and a.aula_cj  and  a.professor_rf =@professorRf");

            if (tipoPendenciaAula == TipoPendencia.Frequencia)
            {
                sqlQuery.AppendLine(" inner join componente_curricular cc ");
                sqlQuery.AppendLine("    on cc.permite_registro_frequencia and a.disciplina_id::int8 = cc.id ");
            }

            sqlQuery.AppendLine(@" where not p.excluido 
                           and p.tipo = @tipo
                           and pe.bimestre = @bimestre
                           and a.turma_id = @turmaId
                           and a.disciplina_id = @disciplinaId 
                           and a.data_aula between pe.periodo_inicio and pe.periodo_fim
                           limit 1");

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sqlQuery.ToString(),
            new
            {
                turmaId,
                disciplinaId,
                tipo = (int)tipoPendenciaAula,
                bimestre,
                professorRf
            }, commandTimeout: 60);
        }

        public async Task<IEnumerable<PossuiPendenciaDiarioBordoDto>> TurmasPendenciaDiarioBordo(IEnumerable<long> aulasId, string turmaId, int bimestre)
        {
            var sqlQuery = new StringBuilder(@"select DISTINCT a.turma_id as TurmaId, a.aula_cj as AulaCJ
                                                  from aula a
                                                  inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id ");

            sqlQuery.AppendLine(@" where a.data_aula between pe.periodo_inicio and pe.periodo_fim
                                    and a.turma_id = @turmaId and pe.bimestre = @bimestre and a.id = ANY(@aulas) ");

            return await database.Conexao.QueryAsync<PossuiPendenciaDiarioBordoDto>(sqlQuery.ToString(),
               new
               {
                   turmaId,
                   aulas = aulasId.ToArray(),
               }, commandTimeout: 60);
        }

        public async Task<IEnumerable<long>> TrazerAulasComPendenciasDiarioBordo(string componenteCurricularId, string professorRf, 
            bool ehGestor, string codigoTurma, int anoLetivo)
        {
            var disciplinaId = Convert.ToInt32(componenteCurricularId);

            var sqlQuery = new StringBuilder(@"select distinct aula_id
                                                from pendencia_diario_bordo pdb
                                                    inner join aula a on a.id = pdb.aula_id
                                                    inner join tipo_calendario tc on tc.id = a.tipo_calendario_id");

            sqlQuery.AppendLine(ehGestor
                ? " where a.turma_id = @codigoTurma and tc.ano_letivo = @anoLetivo"
                : " where pdb.componente_curricular_id = @disciplinaId and pdb.professor_rf = @professorRf and tc.ano_letivo = @anoLetivo");
            
            return await database.Conexao.QueryAsync<long>(sqlQuery.ToString(), new { codigoTurma, anoLetivo, disciplinaId, professorRf },
                commandTimeout: 60);
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa(long dreId, long ueId, int anoLetivo)
        {
            var sqlQuery = @"select distinct a.id, a.turma_id, a.disciplina_id, a.professor_rf,
                                    a.tipo_calendario_id, a.data_aula
	                from atividade_avaliativa aa
	                inner join dre on dre.dre_id = aa.dre_id
	                inner join atividade_avaliativa_disciplina aad
		                on aa.id = aad.atividade_avaliativa_id
	                inner join aula a
		                on aa.turma_id = a.turma_id and
		                    aa.data_avaliacao::date = a.data_aula::date and
		                    aad.disciplina_id = a.disciplina_id and 
		                    a.professor_rf = aa.professor_rf 
	                inner join turma t on t.turma_id = a.turma_id
                    inner join ue u on u.id = t.ue_id 

	                left join notas_conceito nc 
		                on nc.atividade_avaliativa = aa.id
	
	                left join pendencia_aula pa 
		                on pa.aula_id = a.id
	                left join pendencia p 
		                on p.id = pa.pendencia_id 
		                and not p.excluido 
		                and p.tipo = @tipo
                where not aa.excluido 
                    and not a.excluido 
                    and dre.id = @dreId
                    and a.data_aula::date < @hoje
                    and t.ano_letivo = @anoLetivo

                    and nc.id is null 
                    and p.id is null ";

            if (ueId > 0)
                sqlQuery += " and u.id = @ueId";

            return await database.Conexao
                .QueryAsync<Aula>(sqlQuery.ToString(), new
                {
                    anoLetivo,
                    hoje = DateTime.Today.Date,
                    tipo = TipoPendencia.Avaliacao,
                    dreId,
                    ueId
                }, commandTimeout: 120);

        }
        public async Task<long[]> ListarPendenciasPorAulaId(long aulaId)
        {
            var sql = @"select p.tipo 
                        from pendencia_aula pa
                       inner join pendencia p on p.id = pa.pendencia_id and not p.excluido
                       where pa.aula_id = @aula 
                       group by p.tipo";

            return (await database.Conexao.QueryAsync<long>(sql.ToString(), new { aula = aulaId })).AsList().ToArray();
        }

        public async Task<long[]> ListarPendenciasPorAulasId(long[] aulas)
        {
            var sql = @"select p.tipo 
                        from pendencia_aula pa 
                       inner join pendencia p on p.id = pa.pendencia_id and not p.excluido
                       where pa.aula_id =ANY(@aulas) 
                       group by p.tipo";

            return (await database.Conexao.QueryAsync<long>(sql.ToString(), new { aulas })).AsList().ToArray();
        }

        public async Task<Turma> ObterTurmaPorPendencia(long pendenciaId)
        {
            var query = @"select t.* 
                         from pendencia_aula pa
                        inner join aula a on a.id = pa.aula_id
                        inner join turma t on t.turma_id = a.turma_id
                        where pa.pendencia_id = @pendenciaId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { pendenciaId });
        }

        public async Task<Turma> ObterTurmaPorPendenciaDiario(long pendenciaId)
        {
            var query = @"select t.* 
                         from pendencia_diario_bordo pdb
                        inner join aula a on a.id = pdb.aula_id 
                        inner join turma t on t.turma_id = a.turma_id
                        where pdb.pendencia_id = @pendenciaId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { pendenciaId });
        }


        public async Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId)
        {
            var query = @"select distinct a.data_aula as DataAula, pa.Motivo, (a.tipo_aula = @tipoAulaReposicao) ehReposicao, a.turma_id TurmaId, a.ue_id UeId, 
                                          a.disciplina_id DisciplinaId, aa.nome_avaliacao as TituloAvaliacao, pe.bimestre, t.modalidade_codigo ModalidadeCodigo, t.nome NomeTurma
                           from pendencia_aula pa
                           join pendencia p on p.id = pa.pendencia_id
                           join aula a on a.id = pa.aula_id
                           join periodo_escolar pe on a.tipo_calendario_id = pe.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim
                           join turma t on t.turma_id = a.turma_id
                           left join atividade_avaliativa aa on aa.turma_id = a.turma_id 
   									and aa.data_avaliacao::date = a.data_aula::date    									
									and a.professor_rf = aa.professor_rf and p.tipo = @tipoPendenciaAvaliacao                           
                          where pa.pendencia_id = @pendenciaId
                          order by a.data_aula desc";

            return await database.Conexao.QueryAsync<PendenciaAulaDto>(query, new { pendenciaId, tipoAulaReposicao = (int)TipoAula.Reposicao, tipoPendenciaAvaliacao = (int)TipoPendencia.Avaliacao });
        }

        public async Task<long> ObterPendenciaAulaPorTurmaIdDisciplinaId(string turmaId, string disciplinaId, string professorRf, TipoPendencia tipoPendencia)
        {
            var query = @"select p.id 
                            from pendencia p 
                           inner join pendencia_aula pa on p.id = pa.pendencia_id 
                           inner join aula a on pa.aula_id = a.id 
                           where not p.excluido
                             and a.turma_id = @turmaId 
                             and a.disciplina_id = @disciplinaId
                             and a.professor_rf = @professorRf
                             and p.tipo = @tipoPendencia";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, disciplinaId, tipoPendencia, professorRf });
        }

        public async Task<long> ObterPendenciaAulaIdPorAulaId(long aulaId, TipoPendencia tipoPendencia)
        {
            var query = @"select pa.id 
                            from pendencia_aula pa  
                           inner join pendencia p on p.id = pa.pendencia_id and not p.excluido
                           where pa.aula_id  = @aulaId
                            and p.tipo = @tipoPendencia";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { aulaId, tipoPendencia });
        }

        public async Task<IEnumerable<long>> ObterPendenciaIdPorAula(long aulaId, TipoPendencia tipoPendencia)
        {
            var query = @"select p.id 
                            from pendencia_aula pa  
                           inner join pendencia p on p.id = pa.pendencia_id and not p.excluido
                           where pa.aula_id  = @aulaId
                            and p.tipo = @tipoPendencia";
            return await database.Conexao.QueryAsync<long>(query, new { aulaId, tipoPendencia });
        }

        public async Task<bool> PossuiPendenciasPorAulasId(long[] aulasId, bool ehInfantil, long[] componentesCurricularesId)
        {
            string sql;
            if (ehInfantil)
            {
                sql = @"SELECT 1  
                        FROM aula
                        INNER JOIN turma ON aula.turma_id = turma.turma_id
                        LEFT JOIN registro_frequencia rf ON aula.id = rf.aula_id
                        LEFT JOIN pendencia_diario_bordo pdb ON pdb.aula_id = aula.id and pdb.componente_curricular_id = any(@componentesCurricularesId)
                        WHERE NOT aula.excluido
                        AND aula.id = ANY(@aulas)
                        AND aula.data_aula::date < @hoje
                        AND (rf.id is null or pdb.id is not null) ";
            }
            else
            {
                sql = @"SELECT 1 FROM aula
                        INNER JOIN turma ON aula.turma_id = turma.turma_id
                        INNER JOIN componente_curricular cc ON cc.id = aula.disciplina_id::bigint
	                    LEFT JOIN registro_frequencia rf ON aula.id = rf.aula_id
                        WHERE NOT aula.excluido
	                    AND aula.id = ANY(@aulas)
                        AND aula.data_aula::date < @hoje
                        AND rf.id is null 
                        AND cc.permite_registro_frequencia ";
            }

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { aulas = aulasId, hoje = DateTime.Today.Date, componentesCurricularesId }));

        }

        public async Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulasId(long[] aulasId)
        {
            var sql = @"select 1
                            from aula a
                            inner join atividade_avaliativa aa on a.id = ANY(@aulas)
                            and aa.turma_id = a.turma_id
                            and not a.excluido
                            and a.data_aula::date < @hoje
                            and aa.data_avaliacao::date = a.data_aula::date
                            inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = aa.id
                            and aad.disciplina_id = a.disciplina_id
                            left join notas_conceito n on aa.id = n.atividade_avaliativa and n.id is null;";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { aulas = aulasId, hoje = DateTime.Today.Date }));
        }

        public async Task<bool> PossuiAtividadeAvaliativaSemNotaPorAulasId(long[] aulasId)
        {
            var sql = @"select 1 from     
                            (select a.id aula_id,aad.atividade_avaliativa_id,max(n.id) nota_id
                                from aula a
                                inner join atividade_avaliativa aa on a.id = ANY(@aulas) and aa.turma_id = a.turma_id and not a.excluido
                                and a.data_aula::date < @hoje
                                and aa.data_avaliacao::date = a.data_aula::date
                                inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = aa.id
                                and aad.disciplina_id = a.disciplina_id and not aad.excluido 
                                left join notas_conceito n on aa.id = n.atividade_avaliativa
                                group by a.id,aad.atividade_avaliativa_id) a where a.nota_id is null;";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { aulas = aulasId, hoje = DateTime.Today.Date }));
        }

        public async Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulaId(long aulaId)
        {
            var sql = @"select
	                       1
                        from
	                        atividade_avaliativa aa
                        inner join atividade_avaliativa_disciplina aad on
	                        aad.atividade_avaliativa_id = aa.id
                        left join notas_conceito n on
	                        aa.id = n.atividade_avaliativa
                        inner join aula a on
	                        aa.turma_id = a.turma_id
	                        and aa.data_avaliacao::date = a.data_aula::date
	                        and aad.disciplina_id = a.disciplina_id
                        where
	                        not a.excluido
	                        and a.id = @aula
                            and a.data_aula::date < @hoje
	                        and n.id is null";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { aula = aulaId, hoje = DateTime.Today.Date }));
        }

        public async Task<PendenciaAulaDto> PossuiPendenciasPorAulaId(long aulaId, bool ehInfantil, Usuario usuarioLogado)
        {

            var sql = new StringBuilder();
            if (ehInfantil)
            {
                sql.AppendLine($@"select
	                          CASE WHEN rf.id is null and cc.permite_registro_frequencia THEN 1
                                    ELSE 0
                              END PossuiPendenciaFrequencia,
                              CASE WHEN pdb.id is not null THEN 1
                                    ELSE 0
                               END PossuiPendenciaDiarioBordo 
                           from
	                            aula
                            inner join turma on 
	                            aula.turma_id = turma.turma_id
                            inner join componente_curricular cc on 
	                        	aula.disciplina_id = cc.id::varchar
	                        left join registro_frequencia rf on
	                            aula.id = rf.aula_id
                            left join pendencia_diario_bordo pdb on
                                aula.id =  pdb.aula_id");

                if (usuarioLogado.EhProfessorInfantilOuCjInfantil())
                {
                    sql.AppendLine(@" and pdb.professor_rf = @usuarioLogadoRf");
                }

                sql.AppendLine(@"  where
	                            not aula.excluido
	                            and aula.id = @aula
                                and aula.data_aula::date < @hoje
                                and (rf.id is null or pdb.id is not null)");
            }
            else
            {
                sql.AppendLine($@"select
	                          CASE WHEN rf.id is null and cc.permite_registro_frequencia and turma.etapa_eja = 0 THEN 1
                                    ELSE 0
                              END PossuiPendenciaFrequencia,
                               0 PossuiPendenciaPlanoAula 
                           from
	                            aula
                            inner join turma on 
	                            aula.turma_id = turma.turma_id
                            inner join componente_curricular cc on 
	                        	aula.disciplina_id = cc.id::varchar
	                        left join registro_frequencia rf on
	                            aula.id = rf.aula_id                            
                            where
	                            not aula.excluido
	                            and aula.id = @aula
                                and aula.data_aula::date < @hoje
                                and rf.id is null");
            }

            return (await database.Conexao.QueryFirstOrDefaultAsync<PendenciaAulaDto>(sql.ToString(), new { aula = aulaId, hoje = DateTime.Today.Date, usuarioLogadoRf = usuarioLogado.CodigoRf }));
        }

        public async Task<IEnumerable<PendenciaAulaProfessorDto>> ObterPendenciaIdPorComponenteProfessorEBimestre(long componenteCurricularId, string codigoRf, long periodoEscolarId, TipoPendencia tipoPendencia, string turmaCodigo, long ueId)
        {
            try
            {
                var sql = @"select distinct pa.pendencia_id PendenciaId, a.id AulaId, u.rf_codigo CodigoRfProfessor
                        from pendencia p 
                        join pendencia_aula pa on pa.pendencia_id = p.id 
                        join pendencia_usuario pu on pu.pendencia_id = p.id 
                        join usuario u on u.id = pu.usuario_id 
                        join aula a on a.id = pa.aula_id 
                        join turma t on t.turma_id = a.turma_id 
                        join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id
                        join componente_curricular cc on cc.id = a.disciplina_id::int8 
                        where u.rf_codigo = @codigoRf and cc.id = @componenteCurricularId 
                        and pe.id = @periodoEscolarId and p.tipo = @tipoPendencia 
                        and t.turma_id = @turmaCodigo and t.ue_id = @ueId and not p.excluido  
                        order by pa.pendencia_id, a.id";

                return (await database.Conexao.QueryAsync<PendenciaAulaProfessorDto>(sql, new { componenteCurricularId, codigoRf, periodoEscolarId, tipoPendencia, turmaCodigo, ueId }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolar(long componenteCurricularId, string codigoRf, long periodoEscolarId)
        {
            try
            {
                var sql = @"select p.Id from pendencia p 
                        join pendencia_diario_bordo pdb on pdb.pendencia_id = p.id 
                        join pendencia_usuario pu on pu.pendencia_id = p.id 
                        join usuario u on u.id = pu.usuario_id 
                        join aula a on a.id = pdb.aula_id 
                        join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id
                        join componente_curricular cc on cc.id = pdb.componente_curricular_id
                        where u.rf_codigo = @codigoRf and cc.id = @componenteCurricularId 
                        and pe.id = @periodoEscolarId and p.tipo = @tipoPendencia 
                        order by p.criado_em desc";

                var retorno =  (await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { componenteCurricularId, codigoRf, periodoEscolarId, tipoPendencia = (int)TipoPendencia.DiarioBordo }, commandTimeout: 60));
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> ObterPendenciaPorDescricaoTipo(string descricao, TipoPendencia tipoPendencia)
        {
            var sql = $@"select p.Id from pendencia p where lower(descricao) = lower(@descricao) and tipo = @tipoPendencia and not excluido ";

            return (await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { descricao, tipoPendencia }));
        }

        public async Task<IEnumerable<long>> ObterPendenciasAulaPorDreUeTipoModalidade(long dreId, long ueId, TipoPendencia tipoPendencia, Modalidade modalidade)
        {
            var sql = $@" select p.id from pendencia p 
                                inner join pendencia_aula pa on pa.pendencia_id = p.id
                                inner join aula a on a.id = pa.aula_id 
                                inner join turma t on a.turma_id = t.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                                where p.tipo = @tipoPendencia and u.id = @ueId and d.id = @dreId
                                and t.modalidade_codigo = @modalidade and not p.excluido";

            return (await database.Conexao.QueryAsync<long>(sql, new { dreId, ueId, tipoPendencia, modalidade }));
        }
    }
}
