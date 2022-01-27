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
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task ExcluirPeloSistemaAsync(long[] idsAulas)
        {
            var sql = "update aula set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });

            sql = "update diario_bordo set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where aula_id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });
        }

        public void SalvarVarias(IEnumerable<Aula> aulas)
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
                foreach (var aula in aulas)
                {
                    writer.StartRow();
                    writer.Write(aula.DataAula);
                    writer.Write(aula.DisciplinaId);
                    writer.Write(aula.Quantidade);
                    writer.Write((int)aula.RecorrenciaAula, NpgsqlDbType.Integer);
                    writer.Write((int)aula.TipoAula, NpgsqlDbType.Integer);
                    writer.Write(aula.TipoCalendarioId);
                    writer.Write(aula.TurmaId);
                    writer.Write(aula.UeId);
                    writer.Write(aula.ProfessorRf);
                    writer.Write(aula.CriadoEm);
                    writer.Write(aula.CriadoPor != null ? aula.CriadoPor : "Sistema");
                    writer.Write(aula.CriadoRF != null ? aula.CriadoRF : "Sistema");
                }
                writer.Complete();
            }
        }

        public async Task<IEnumerable<DiarioBordoPorPeriodoDto>> ObterDatasAulaDiarioBordoPorPeriodo(string turmaCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select a.id as AulaId, 
                            db.id as DiarioBordoId, 
                            a.data_aula as DataAula, 
                            db.planejamento as Planejamento, 
                            db.reflexoes_replanejamento as ReflexoesReplanejamento 
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
                         db.criado_por Nome, db.planejamento as Planejamento, db.reflexoes_replanejamento as ReflexoesReplanejamento, 
                         a.tipo_aula as Tipo, db.inserido_cj as InseridoCJ, false Pendente
                         from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         inner join diario_bordo db on a.id = db.aula_id
                         where t.turma_id = @turmaCodigo
                           and db.componente_curricular_id = @componenteCurricularFilhoId 
                           and not a.excluido
                           and a.data_aula >= @dataInicio
                           and a.data_aula <= @dataFim
                         union all
                         select null DiarioBordoId, a.data_aula DataAula, a.id as AulaId, null CodigoRf, null Nome, 
                         null Planejamento, null ReflexoesReplanejamento, null Tipo, null InseridoCJ, true Pendente 
                         from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         where t.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularPaiCodigo
                           and a.data_aula >= @dataInicio
                           and a.data_aula <= @dataFim
                           and not a.excluido
                           and not exists (select 1 from diario_bordo db where db.componente_curricular_id = @componenteCurricularFilhoId and db.aula_id = a.id)";

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