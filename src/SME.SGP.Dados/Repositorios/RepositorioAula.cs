using Dapper;
using Npgsql;
using NpgsqlTypes;
using Sentry;
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
                    writer.Write(aula.CriadoPor != null? aula.CriadoPor: "Sistema");
                    writer.Write(aula.CriadoRF != null? aula.CriadoRF: "Sistema");
                }
                writer.Complete();
            }
        }

        public async Task ExcluirPeloSistemaAsync(long[] idsAulas)
        {
            var sql = "update aula set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });

            sql = "update diario_bordo set excluido = true, alterado_por = @alteradoPor, alterado_em = @alteradoEm, alterado_rf = @alteradoRf where aula_id = any(@idsAulas)";
            await database.Conexao.ExecuteAsync(sql, new { idsAulas, alteradoPor = "Sistema", alteradoEm = DateTime.Now, alteradoRf = "Sistema" });
        } 

        public override long Salvar(Aula entidade)
        {
            ValideQuantidadeDeAulas(entidade);

            return base.Salvar(entidade);
        }
        public override Task<long> SalvarAsync(Aula entidade)
        {
            ValideQuantidadeDeAulas(entidade);

            return base.SalvarAsync(entidade);
        }

        private void ValideQuantidadeDeAulas(Aula entidade)
        {
            if (entidade.Quantidade < 0 && !entidade.Excluido)
            {
                SentrySdk.AddBreadcrumb($@"
                    Turma id: {entidade.TurmaId}, 
                    Quantidade: {entidade.Quantidade},
                    Data aula: {entidade.DataAula}, 
                    Professor: {entidade.ProfessorRf},
                    Disciplina: {entidade.DisciplinaId},
                    Recorrência aula: {entidade.RecorrenciaAula},
                    Tipo de aula: {entidade.TipoAula} -``
                    {DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt}", "Erro ao salvar aulas com quantidade negativa");

                throw new NegocioException("Não é possível salvar aula com quantidade negativa. Entre em contato com suporte.");
            }
        }
    }
}