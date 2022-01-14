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
    }
}