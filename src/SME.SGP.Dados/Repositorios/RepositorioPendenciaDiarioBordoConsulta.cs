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

        public async Task<bool> VerificarSeExistePendenciaDiarioComPendenciaId(long pendenciaId)
        {
            var sql = @"select count(id) from pendencia_diario_bordo where pendencia_id = @pendenciaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { pendenciaId }) > 0;
        }

        public async Task<IEnumerable<AulaComComponenteDto>> ListarPendenciasDiario(string turmaId, long[] componentesCurricularesId)
        {
            var sqlQuery = @" select distinct a.id as Id,
                                              a.turma_id as TurmaId,
                                              db.componente_curricular_id as ComponenteId,
                                              pe.id as PeriodoEscolarId
                              from aula a
                                   join turma t on a.turma_id = t.turma_id and t.modalidade_codigo = @modalidadeCodigo
                                   join ue on t.ue_id = ue.id
                                   join periodo_escolar pe on a.data_aula between pe.periodo_inicio and pe.periodo_fim 	
                                   left join diario_bordo db on db.aula_id = a.id and db.componente_curricular_id = ANY(@componentesCurricularesId)
                                   left join pendencia_diario_bordo pdb on pdb.aula_id = a.id and pdb.componente_curricular_id = ANY(@componentesCurricularesId)
                                   left join pendencia p on p.id = pdb.pendencia_id and not p.excluido and p.tipo = @tipoPendencia
                              where not a.excluido
                                    and a.data_aula < @hoje
                                    and t.turma_id = @turmaId
                                    and pdb.id is null
                                    and p.id is null
                                    and pe.tipo_calendario_id = a.tipo_calendario_id";
            return await database.Conexao.QueryAsync<AulaComComponenteDto>(sqlQuery.ToString(), new
            {
                hoje = DateTime.Today.Date,
                turmaId,
                componentesCurricularesId,
                modalidadeCodigo = Modalidade.EducacaoInfantil,
                tipoPendencia = TipoPendencia.DiarioBordo
            }, commandTimeout: 200);
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
                        and pe.id = @periodoEscolarId and p.tipo = @tipoPendencia and not p.excluido 
                        order by p.criado_em desc";

                var retorno = (await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { componenteCurricularId, codigoRf, periodoEscolarId, tipoPendencia = (int)TipoPendencia.DiarioBordo }, commandTimeout: 60));
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public async Task<IEnumerable<long>> TrazerAulasComPendenciasDiarioBordo(string componenteCurricularId, string professorRf, bool ehGestor, string codigoTurma)
        {
            var disciplinaId = Convert.ToInt32(componenteCurricularId);
            var sqlQuery = string.Empty;
            if (ehGestor)
            {
                sqlQuery = @"select aula_id from pendencia_diario_bordo pdb join aula a on a.id = pdb.aula_id where a.turma_id = @codigoTurma";
                return await database.Conexao.QueryAsync<long>(sqlQuery, new { codigoTurma }, commandTimeout: 60);
            }
            else
            {
                sqlQuery = @"select distinct pdb.aula_id from pendencia_diario_bordo pdb where pdb.componente_curricular_id = @disciplinaId and pdb.professor_rf = @professorRf";
                return await database.Conexao.QueryAsync<long>(sqlQuery, new { professorRf, disciplinaId }, commandTimeout: 60);
            }

        }
    }
}
