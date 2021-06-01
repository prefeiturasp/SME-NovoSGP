using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaAula : IRepositorioPendenciaAula
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaAula(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades)
        {
            var query = $@"select
	                        aula.id as Id
                        from
	                        aula
                        inner join turma on 
	                        aula.turma_id = turma.turma_id
                        left join pendencia_aula pa on
	                        aula.id = pa.aula_id
	                    left join pendencia p on p.id = pa.pendencia_id and p.tipo = @tipo and not p.excluido
                        left join {tabelaReferencia} on
	                        aula.id = {tabelaReferencia}.aula_id
                        where
	                        not aula.excluido
	                        and aula.data_aula < @hoje
	                        and turma.modalidade_codigo = ANY(@modalidades)
	                        and pendencia_aula.id is null
	                        and {tabelaReferencia}.id is null
                        group by
	                        aula.id";

            return (await database.Conexao.QueryAsync<Aula>(query, new { hoje = DateTime.Today, tipo = tipoPendenciaAula, modalidades }));
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa()
        {
            var sql = @"select
	                        a.id as Id
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
                        left join pendencia_aula pa on
	                        a.id = pa.aula_id
                        left join pendencia p on p.id = pa.pendencia_id
	                        and p.tipo = @tipo
	                        and not p.excluido
                        where
	                        not a.excluido
	                        and a.data_aula::date < @hoje
	                        and n.id is null
	                        and pa.id is null
	                        and p.id is null
                        group by
	                        a.id";

            return (await database.Conexao.QueryAsync<Aula>(sql.ToString(), new { hoje = DateTime.Today, tipo = TipoPendencia.Avaliacao }));
        }

        public async Task Excluir(long pendenciaId, long aulaId)
        {
            await database.Conexao.ExecuteScalarAsync(@"delete from pendencia_aula 
                                                    where aula_id = @aulaId and pendencia_id = @pendenciaId", new { aulaid = aulaId, pendenciaId });
        }

        public async Task Salvar(long aulaId, string motivo, long pendenciaId)
        {
            await database.Conexao.InsertAsync(new PendenciaAula()
            {
                AulaId = aulaId,
                Motivo = motivo,
                PendenciaId = pendenciaId
            });
        }

        public void SalvarVarias(long pendenciaId, IEnumerable<long> aulas)
        {
            var sql = @"copy pendencia_aula (pendencia_id, aula_id)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var aulaId in aulas)
                {
                    writer.StartRow();
                    writer.Write(pendenciaId, NpgsqlDbType.Bigint);
                    writer.Write(aulaId, NpgsqlDbType.Bigint);
                }
                writer.Complete();
            }
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
                        left join diario_bordo tr on aula.id = tr.aula_id
                        where not aula.excluido
	                        and aula.id = ANY(@aulas)
                            and aula.data_aula::date < @hoje
                            and (rf.id is null or tr.id is null)
	                        " :
                            $@"select 1
                        from aula
                        inner join turma on aula.turma_id = turma.turma_id
	                    left join registro_frequencia rf on aula.id = rf.aula_id
                        where not aula.excluido
	                        and aula.id = ANY(@aulas)
                            and aula.data_aula::date < @hoje
                            and rf.id is null";

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
                                and aad.disciplina_id = a.disciplina_id
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