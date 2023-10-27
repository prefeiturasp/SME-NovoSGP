using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        private readonly IUnitOfWork unitOfWork;

        public RepositorioAula(ISgpContext conexao, IUnitOfWork unitOfWork, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task ExcluirPeloSistemaAsync(long[] idsAulas)
        {
            var sql = "update aula set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });

            sql = "update diario_bordo set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where aula_id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });
        }

        public async Task SalvarVarias(IEnumerable<(Aula aula, long? planoAulaId)> aulas)
        {
            var sql = @"copy aula ( 
                                        data_aula, 
                                        disciplina_id, 
                                        quantidade, 
                                        recorrencia_aula, 
                                        tipo_aula, 
                                        tipo_calendario_id, 
                                        turma_id, 
                                        ue_id, 
                                        professor_rf,
                                        criado_em,
                                        criado_por,
                                        criado_rf)
                            from
                            stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var itemAula in aulas.Where(a => a.aula.Id == 0))
                {
                   await writer.StartRowAsync();
                   await writer.WriteAsync(itemAula.aula.DataAula);
                   await writer.WriteAsync(itemAula.aula.DisciplinaId);
                   await writer.WriteAsync(itemAula.aula.Quantidade);
                   await writer.WriteAsync((int)itemAula.aula.RecorrenciaAula, NpgsqlDbType.Integer);
                   await writer.WriteAsync((int)itemAula.aula.TipoAula, NpgsqlDbType.Integer);
                   await writer.WriteAsync(itemAula.aula.TipoCalendarioId);
                   await writer.WriteAsync(itemAula.aula.TurmaId);
                   await writer.WriteAsync(itemAula.aula.UeId);
                   await writer.WriteAsync(itemAula.aula.ProfessorRf);
                   await writer.WriteAsync(itemAula.aula.CriadoEm);
                   await writer.WriteAsync(itemAula.aula.CriadoPor.NaoEhNulo() ? itemAula.aula.CriadoPor : "Sistema");
                   await writer.WriteAsync(itemAula.aula.CriadoRF.NaoEhNulo() ? itemAula.aula.CriadoRF : "Sistema");
                }
               await writer.CompleteAsync();
            }
            
            var idsAulasAtualizacao = aulas
                .Where(a => a.aula.Id > 0)
                .Select(a => a.aula.Id)
                .ToArray();

            if (idsAulasAtualizacao.NaoEhNulo() && idsAulasAtualizacao.Any())
            {
                try
                {
                    sql = @"update aula 
                        set excluido = false, 
                            alterado_por = 'Sistema', 
                            alterado_em = current_timestamp, 
                            alterado_rf = 'Sistema'
                        where id = any(@idsAulas);";

                    unitOfWork.IniciarTransacao();

                    await database.Conexao
                        .ExecuteAsync(sql, new { idsAulas = idsAulasAtualizacao });

                    sql = @"update registro_frequencia
                        set excluido = false,
                            alterado_por = 'Sistema', 
                            alterado_em = current_timestamp, 
                            alterado_rf = 'Sistema'
                        where aula_id = any(@idsAulas) and
                              excluido;";

                    await database.Conexao
                        .ExecuteAsync(sql, new { idsAulas = idsAulasAtualizacao });

                    sql = @"update registro_frequencia_aluno
                        set excluido = false,
                            alterado_por = 'Sistema', 
                            alterado_em = current_timestamp, 
                            alterado_rf = 'Sistema'
                        where aula_id = any(@idsAulas) and excluido;";

                    await database.Conexao
                        .ExecuteAsync(sql, new { idsAulas = idsAulasAtualizacao });

                    sql = @"update plano_aula set aula_id = @aulaId where id = @planoAulaId;";

                    var aulasPlano = aulas.Where(a => a.planoAulaId.HasValue).ToList();
                    foreach (var aula in aulasPlano)
                        await database.Conexao.ExecuteAsync(sql, new { aulaId = aula.aula.Id, planoAulaId = aula.planoAulaId.Value });

                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> ObterDatasAulaDiarioBordoPorPeriodo(string turmaCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select a.id as AulaId, 
                            db.id as DiarioBordoId, 
                            a.data_aula as DataAula, 
                            db.planejamento as Planejamento
                            from aula a 
                            left join diario_bordo db on db.aula_id = a.id 
                            where a.turma_id = @turmaCodigo 
                            and a.data_aula between @dataInicio and @dataFim
                            and db.componente_curricular_id = @componenteCurricularId
                            and not a.excluido and not db.excluido
                            order by a.data_aula desc";

            return await database.Conexao.QueryAsync<DiarioBordoPorPeriodoDto>(query, new { turmaCodigo, componenteCurricularId, dataInicio, dataFim });
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> ObterAulasDiariosPorPeriodo(string turmaCodigo, long componenteCurricularFilhoId, string componenteCurricularPaiCodigo, DateTime dataFim, DateTime dataInicio)
        {
            var query = @"
                         select db.id as DiarioBordoId, a.data_aula DataAula, a.id as AulaId, db.criado_rf CodigoRf,
                         db.criado_por Nome, db.planejamento as Planejamento, 
                         a.tipo_aula as Tipo, db.inserido_cj as InseridoCJ, false Pendente
                         from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         inner join diario_bordo db on a.id = db.aula_id
                         where t.turma_id = @turmaCodigo
                           and db.componente_curricular_id = @componenteCurricularFilhoId 
                           and not a.excluido and not db.excluido
                           and a.data_aula >= @dataInicio
                           and a.data_aula <= @dataFim
                         union all
                         select null DiarioBordoId, a.data_aula DataAula, a.id as AulaId, null CodigoRf, null Nome, 
                         null Planejamento, null Tipo, null InseridoCJ, true Pendente 
                         from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         where t.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularPaiCodigo
                           and a.data_aula >= @dataInicio
                           and a.data_aula <= @dataFim
                           and not a.excluido
                           and not exists (select 1 from diario_bordo db where db.componente_curricular_id = @componenteCurricularFilhoId and db.aula_id = a.id and not db.excluido)";

            return await database.Conexao.QueryAsync<DiarioBordoPorPeriodoDto>(query,
                                                    new
                                                    {
                                                        turmaCodigo,
                                                        componenteCurricularFilhoId,
                                                        componenteCurricularPaiCodigo,
                                                        dataFim,
                                                        dataInicio
                                                    });
        }
    }
}