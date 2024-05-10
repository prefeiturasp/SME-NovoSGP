using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDevolutiva : IRepositorioPendenciaDevolutiva
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPendenciaDevolutiva(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }


        public async Task Salvar(PendenciaDevolutiva pendenciaDevolutiva)
        {
            await database.Conexao.InsertAsync(pendenciaDevolutiva);
        }

        public async Task ExcluirPorTurmaComponente(long turmaId, long componenteId)
        {
            await database.Conexao.ExecuteScalarAsync(@"DELETE FROM pendencia_devolutiva WHERE turma_id = @turmaId AND componente_curricular_id =@componenteId", new { turmaId, componenteId });
        }

        public async Task ExcluirPorId(long id)
        {
            await database.Conexao.ExecuteScalarAsync(@"DELETE FROM pendencia_devolutiva WHERE id =@id", new { id });
        }


        public async Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorPendencia(long pendenciaId)
        {
            var query = $@"SELECT
                            pd.*, 
                            p.*,
                            cc.*,
                            t.*
                        FROM
                            pendencia_devolutiva pd
                        INNER JOIN pendencia p ON
                            pd.pendencia_Id = p.id
                        INNER JOIN componente_curricular cc ON
                            pd.componente_curricular_id = cc.id
                        INNER JOIN turma t ON
                            pd.turma_id = t.id
                        WHERE
                            pd.pendencia_Id = @pendenciaId ";

            return await database.Conexao.QueryAsync<PendenciaDevolutiva>(query, new { pendenciaId});
        }

        public async Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId)
        {
            var query = @"SELECT
                            pd.*, 
                            p.*,
                            cc.*,
                            t.*
                        FROM
                            pendencia_devolutiva pd
                        INNER JOIN pendencia p ON
                            pd.pendencia_Id = p.id
                        INNER JOIN componente_curricular cc ON
                            pd.componente_curricular_id = cc.id
                        INNER JOIN turma t ON
                            pd.turma_id = t.id
                        WHERE pd.componente_curricular_id  = @componenteId 
                        AND pd.turma_id  = @turmaId
                        AND NOT p.excluido";

            return await database.Conexao.QueryAsync<PendenciaDevolutiva>(query,new { turmaId, componenteId });
        }

        public async Task<IEnumerable<string>> ObterCodigoComponenteComDiarioBordoSemDevolutiva(long turmaId, string ueId)
        {
            var query = @"
                            SELECT 
                                DISTINCT  db.componente_curricular_id AS ComponenteCodigo
                            FROM
                                diario_bordo db
                            INNER JOIN aula a ON
                                db.aula_id = a.id
                            WHERE
                                db.devolutiva_id ISNULL
                                AND db.turma_id = @turmaId
                                AND a.ue_id  = @ueId ";

            return await database.Conexao.QueryAsync<string>(query, new { turmaId, ueId });
        }

        public async Task<Turma> ObterTurmaPorPendenciaId(long pendenciaId)
        {
            var query = @"select t.* 
                            from pendencia_devolutiva pd
                            inner join turma t on t.id = pd.turma_id
                            where pd.pendencia_id = @pendenciaId ";

            return await database.Conexao.QueryFirstOrDefaultAsync<Turma>(query, new { pendenciaId });
        }

        public async Task<bool> ExistePendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId)
        {
            var query = @"SELECT count(pd.id) as TotalPendenciasDevolutiva
                            FROM pendencia_devolutiva pd
                                INNER JOIN pendencia p ON pd.pendencia_Id = p.id
                                INNER JOIN componente_curricular cc ON pd.componente_curricular_id = cc.id
                                INNER JOIN turma t ON pd.turma_id = t.id
                            WHERE pd.componente_curricular_id = @componenteId 
                            AND pd.turma_id  = @turmaId
                            AND NOT p.excluido";

            var resultado = await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, componenteId });
            return (resultado > 0);
        }

        public async Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE)
        {
            var tipoPendencia = (int)TipoPendencia.Devolutiva;
            var situacao = new List<int>() { (int)SituacaoPendencia.Pendente, (int)SituacaoPendencia.Resolvida };
            var query = @$"select distinct p.id 
                            from pendencia p
                            inner join pendencia_devolutiva pri on pri.pendencia_id = p.id 
                            inner join turma t on t.id = pri.turma_id 
                            inner join aula a on a.turma_id = t.turma_id 
                            inner join tipo_calendario tc on tc.id = a.tipo_calendario_id 
                            where tipo = @tipoPendencia
                                    and not p.excluido 
                                    and p.situacao = any(@situacao)
                                    and tc.ano_letivo = @anoLetivo 
                                    and a.ue_id = @codigoUE";

            return await database.Conexao.QueryAsync<long>(query, new
            {
                tipoPendencia,
                situacao = situacao.ToArray(),
                anoLetivo,
                codigoUE
            });
        }

        public async Task ExlusaoLogicaPorTurmaComponente(long turmaId, long componenteId)
        {
            await database.Conexao.ExecuteAsync(@"update pendencia set excluido = true 
                                                    where id in (select pd.pendencia_id 
                                                                from pendencia_devolutiva pd 
                                                                where pd.turma_id = @turmaId 
                                                                and pd.componente_curricular_id = @componenteId)", new { @turmaId, componenteId });
        }
    }
}
