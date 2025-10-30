using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaDiarioBordoConsulta : IRepositorioPendenciaDiarioBordoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPendenciaDiarioBordoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }
        public async Task<long> ExisteIdPendenciaDiarioBordo(long aulaId, long componenteCurricularId)
        {
            var sql = "select pendencia_id from pendencia_diario_bordo where aula_id = @aulaId and componente_curricular_id = @componenteCurricularId ";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { aulaId, componenteCurricularId }, commandTimeout: 60);
        }

        public async Task<IEnumerable<PendenciaUsuarioDto>> ObterIdPendenciaDiarioBordoPorAulaId(long aulaId)
        {
            var sql = @"select pdb.pendencia_id as PendenciaId, u.id as UsuarioId 
                                from pendencia_diario_bordo pdb
                                inner join usuario u on u.rf_codigo = pdb.professor_rf
                                where pdb.aula_id = @aulaId";
            return await database.Conexao.QueryAsync<PendenciaUsuarioDto>(sql, new { aulaId}, commandTimeout: 60);
        }

        public async Task<IEnumerable<PendenciaDiarioBordoDescricaoDto>> ObterPendenciasDiarioPorPendencia(long pendenciaId, string codigoRf)
        {
            var query = @"select distinct a.data_aula as DataAula, coalesce(cc.descricao_infantil , cc.descricao_sgp, cc.descricao) as ComponenteCurricular, 
                                pe.bimestre, pdb.pendencia_id as PendenciaId, t.modalidade_codigo ModalidadeCodigo, t.nome NomeTurma, (a.tipo_aula = @tipoAulaReposicao) ehReposicao 
                        from pendencia_diario_bordo pdb
                        join aula a on a.id = pdb.aula_id
                        join componente_curricular cc on cc.id = pdb.componente_curricular_id
                        join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and pe.periodo_inicio <= a.data_aula and pe.periodo_fim >= a.data_aula
                        join turma t on t.turma_id = a.turma_id 
                        where pdb.pendencia_id = @pendenciaId and pdb.professor_rf = @codigoRf
                        order by a.data_aula desc";

            return await database.Conexao.QueryAsync<PendenciaDiarioBordoDescricaoDto>(query, new { pendenciaId, codigoRf, tipoAulaReposicao = (int)TipoAula.Reposicao });
        }

        public async Task<IEnumerable<long>> ObterIdsPendencias(int anoLetivo, string codigoUE)
        {
            var tipoPendencia = (int)TipoPendencia.DiarioBordo;
            var situacao = new List<int>() { (int)SituacaoPendencia.Pendente, (int)SituacaoPendencia.Resolvida };
            var query = @$"select distinct p.id 
                            from pendencia p
                            inner join pendencia_diario_bordo pdb  on pdb.pendencia_id = p.id 
                            inner join aula a on a.id = pdb.aula_id 
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

        public async Task<IEnumerable<PendenciaDiarioBordoParaExcluirDto>> ListarPendenciaDiarioBordoParaExcluirPorIdTurma(string turmaId)
        {
            const string sql = @" select 
                                      distinct db.aula_id as aulaId, 
                                      db.componente_curricular_id as componenteCurricularId
                                    from 
                                      pendencia_diario_bordo pdb 
                                      inner join pendencia p on p.id = pdb.pendencia_id 
                                      inner join diario_bordo db on db.componente_curricular_id = pdb.componente_curricular_id 
                                      and db.aula_id = pdb.aula_id 
                                      inner join turma t on t.id = db.turma_id
                                    where 
                                      not p.excluido 
                                      and not db.excluido 
                                      and t.turma_id = @turmaId";

            return await database.Conexao.QueryAsync<PendenciaDiarioBordoParaExcluirDto>(sql, new { turmaId });
        }
    }
}
