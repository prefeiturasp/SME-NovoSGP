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
        private const string NOME_USUARIO_SISTEMA = "Sistema";
        private readonly IUnitOfWork unitOfWork;

        public RepositorioAula(ISgpContext conexao, IUnitOfWork unitOfWork, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task ExcluirPeloSistemaAsync(long[] idsAulas)
        {
            var sql = "update aula set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = NOME_USUARIO_SISTEMA, alteradoEm = DateTimeExtension.HorarioBrasilia(), alteradoRf = NOME_USUARIO_SISTEMA });

            sql = "update diario_bordo set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where aula_id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = NOME_USUARIO_SISTEMA, alteradoEm = DateTimeExtension.HorarioBrasilia(), alteradoRf = NOME_USUARIO_SISTEMA });
        }

        public async Task SalvarVarias(IEnumerable<(Aula aula, long? planoAulaId)> aulas)
        {
            var sqlInsercao = ObterConsultaInsercaoVariasAulas(aulas.Where(a => a.aula.Id == 0).Select(a => a.aula));
            var sqlAtualizacao = string.Empty;

            var idsAulasAtualizacao = aulas
                .Where(a => a.aula.Id > 0)
                .Select(a => a.aula.Id)
                .ToArray();

            if (idsAulasAtualizacao.NaoEhNulo() && idsAulasAtualizacao.Any())
            {
                sqlAtualizacao = ObterConsultaAtualizacaoAulaEReferencias();
                sqlAtualizacao = ObterConsultaAtualizacaoPlanoAula(aulas, sqlAtualizacao);
            }

            if (!string.IsNullOrEmpty(sqlInsercao) || !string.IsNullOrEmpty(sqlAtualizacao))                
            {
                unitOfWork.IniciarTransacao();

                if (!string.IsNullOrEmpty(sqlInsercao))
                    await database.Conexao.ExecuteAsync(sqlInsercao);

                if (!string.IsNullOrEmpty(sqlAtualizacao))
                    await database.Conexao.ExecuteAsync(sqlAtualizacao, new { sistema = NOME_USUARIO_SISTEMA, idsAulas = idsAulasAtualizacao });

                unitOfWork.PersistirTransacao();
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

        private static string ObterConsultaInsercaoVariasAulas(IEnumerable<Aula> aulas)
            => aulas.Any() ? string.Concat(@"insert into aula (data_aula, 
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
                                             values ", string.Join(",", aulas.Select(a => $"('{a.DataAula:yyyy-MM-dd}', '{a.DisciplinaId}', {a.Quantidade}, {(int)a.RecorrenciaAula}, {(int)a.TipoAula}, {a.TipoCalendarioId}, '{a.TurmaId}', '{a.UeId}', '{a.ProfessorRf}', '{a.CriadoEm:yyyy-MM-dd HH:mm:ss}', '{a.CriadoPor}', '{a.CriadoRF}')")),";") : string.Empty;

        private static string ObterConsultaAtualizacaoAulaEReferencias()
            => @"update aula 
                 set excluido = false, 
                     alterado_por = @sistema, 
                     alterado_em = current_timestamp, 
                     alterado_rf = @sistema
                 where id = any(@idsAulas);

                 update registro_frequencia
                 set excluido = false,
                     alterado_por = @sistema, 
                     alterado_em = current_timestamp, 
                     alterado_rf = @sistema
                     where aula_id = any(@idsAulas) and excluido;

                 update registro_frequencia_aluno
                 set excluido = false,
                     alterado_por = @sistema, 
                     alterado_em = current_timestamp, 
                     alterado_rf = @sistema
                     where aula_id = any(@idsAulas) and excluido;";

        private static string ObterConsultaAtualizacaoPlanoAula(IEnumerable<(Aula aula, long? planoAulaId)> aulas, string sqlAtualizacao)
        {
            var sqlUpdatePlanoAula = @"update plano_aula 
                                       set aula_id = {0},
                                           alterado_por = @sistema, 
                                           alterado_em = current_timestamp, 
                                           alterado_rf = @sistema
                                       where id = {1};
                                       ";

            var aulasPlano = aulas.Where(a => a.planoAulaId.HasValue);

            foreach (var aula in aulasPlano)
                sqlAtualizacao += string.Format(sqlUpdatePlanoAula, aula.aula.Id, aula.planoAulaId.Value);

            return sqlAtualizacao;
        }
    }
}