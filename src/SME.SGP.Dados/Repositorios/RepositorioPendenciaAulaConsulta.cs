using Dapper;
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

        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, long dreId, int anoLetivo)
        {
            var listaRetorno = new List<Aula>();
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select distinct a.id as Id,");
            sqlQuery.AppendLine("                a.disciplina_id DisciplinaId,");
            sqlQuery.AppendLine("                a.turma_id TurmaId,");
            sqlQuery.AppendLine("                a.professor_rf ProfessorRf,");
            sqlQuery.AppendLine("                t.id Id,");
            sqlQuery.AppendLine("                t.modalidade_codigo ModalidadeCodigo");
            sqlQuery.AppendLine("  	from aula a");
            sqlQuery.AppendLine("  		inner join tipo_calendario tc");
            sqlQuery.AppendLine("  			on tc.ano_letivo = @anoLetivo and a.tipo_calendario_id = tc.id");
            if (tipoPendenciaAula == TipoPendencia.Frequencia)
            {
                sqlQuery.AppendLine("  		inner join componente_curricular cc");
                sqlQuery.AppendLine("            on cc.permite_registro_frequencia and a.disciplina_id::int8 = cc.id");
            }
            sqlQuery.AppendLine("  		inner join turma t");
            sqlQuery.AppendLine("  			on tc.ano_letivo = t.ano_letivo and t.modalidade_codigo = any(@modalidades) and a.turma_id = t.turma_id");
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

            sqlQuery.AppendLine("	and p.id is null");
            sqlQuery.AppendLine("	and tf.id is null");

            return await database.Conexao.QueryAsync<Aula, Turma, Aula>(sqlQuery.ToString(), (aula, turma) =>
            {
                aula.Turma = turma;
                return aula;
            }, new
            {
                anoLetivo,
                hoje = DateTime.Today.Date,
                tipo = tipoPendenciaAula,
                modalidades,
                dreId
            }, splitOn: "Id", commandTimeout: 60);
        }

        public async Task<bool> PossuiPendenciasPorTipo(string disciplinaId, string turmaId, TipoPendencia tipoPendenciaAula, int bimestre, bool professorCj,bool professorTitular,string professorRf="")
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
                           and a.tipo_aula = @tipoAula
                           and a.disciplina_id = @disciplinaId ");

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sqlQuery.ToString(),
                new
                {
                    turmaId,
                    disciplinaId,
                    tipo = (int)tipoPendenciaAula,
                    bimestre,
                    tipoAula = (int)TipoAula.Normal,
                    professorRf 
                }, commandTimeout: 60);
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa(long dreId, int anoLetivo)
        {
            var sqlQuery = @"select distinct a.id, a.turma_id, a.disciplina_id, a.professor_rf
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

            return await database.Conexao
                .QueryAsync<Aula>(sqlQuery.ToString(), new
                {
                    anoLetivo,
                    hoje = DateTime.Today.Date,
                    tipo = TipoPendencia.Avaliacao,
                    dreId
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

        public async Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId)
        {
            var query = @"select a.data_aula as DataAula, pa.Motivo
                           from pendencia_aula pa
                          inner join aula a on a.id = pa.aula_id
                          where pa.pendencia_id = @pendenciaId
                          order by data_aula desc";

            return await database.Conexao.QueryAsync<PendenciaAulaDto>(query, new { pendenciaId });
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

        public async Task<bool> PossuiPendenciasPorAulasId(long[] aulasId, bool ehInfantil)
        {
            var sql = ehInfantil ? $@"select 1
                        from aula
                        inner join turma on aula.turma_id = turma.turma_id
	                    left join registro_frequencia rf on aula.id = rf.aula_id
                        where not aula.excluido
	                        and aula.id = ANY(@aulas)
                            and aula.data_aula::date < @hoje
                            and (rf.id is null)
	                        " :
                               $@"select 1
                        from aula
                        inner join turma on aula.turma_id = turma.turma_id
                        inner join componente_curricular cc on cc.id = aula.disciplina_id::bigint
	                    left join registro_frequencia rf on aula.id = rf.aula_id
                        where not aula.excluido
	                        and aula.id = ANY(@aulas)
                            and aula.data_aula::date < @hoje
                            and rf.id is null 
                            and cc.permite_registro_frequencia";

            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { aulas = aulasId, hoje = DateTime.Today.Date }));
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

        public async Task<PendenciaAulaDto> PossuiPendenciasPorAulaId(long aulaId, bool ehInfantil)
        {

            var sql = ehInfantil ? $@"select
	                          CASE WHEN rf.id is null and cc.permite_registro_frequencia THEN 1
                                    ELSE 0
                              END PossuiPendenciaFrequencia,
                              CASE WHEN tr.id is null THEN 1
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
                            left join diario_bordo tr on
                                aula.id =  tr.aula_id
                            where
	                            not aula.excluido
	                            and aula.id = @aula
                                and aula.data_aula::date < @hoje
                                and (rf.id is null or tr.id is null) " :

                                $@"select
	                          CASE WHEN rf.id is null and cc.permite_registro_frequencia THEN 1
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
                                and rf.id is null";

            return (await database.Conexao.QueryFirstOrDefaultAsync<PendenciaAulaDto>(sql, new { aula = aulaId, hoje = DateTime.Today.Date }));
        }
    }
}
