using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordoConsulta : RepositorioBase<DiarioBordo>, IRepositorioDiarioBordoConsulta
    {
        public RepositorioDiarioBordoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<DiarioBordo>> ObterIdDiarioBordoAulasExcluidas(string codigoTurma, string[] codigosDisciplinas, long tipoCalendarioId, DateTime[] datasConsideradas)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("select distinct db.id,");
            sqlQuery.AppendLine("    db.aula_id,");
            sqlQuery.AppendLine("    a.id,");
            sqlQuery.AppendLine("    a.data_aula");
            sqlQuery.AppendLine("  from diario_bordo db");
            sqlQuery.AppendLine("  inner join aula a on db.aula_id = a.id");
            sqlQuery.AppendLine("  where a.excluido and");
            sqlQuery.AppendLine("  db.excluido = false and");
            sqlQuery.AppendLine("  a.turma_id = @codigoTurma and");
            sqlQuery.AppendLine("  a.disciplina_id = any(@codigosDisciplinas) and");
            sqlQuery.AppendLine("  a.tipo_calendario_id = @tipoCalendarioId and");
            sqlQuery.AppendLine("  a.data_aula = any(@datasConsideradas);");

            return await database.Conexao
                .QueryAsync<DiarioBordo, Aula, DiarioBordo>(sqlQuery.ToString(), (diarioBordo, aula) =>
                {
                    diarioBordo.AdicionarAula(aula);
                    return diarioBordo;
                }, new
                {
                    codigoTurma,
                    codigosDisciplinas,
                    tipoCalendarioId,
                    datasConsideradas
                }, splitOn: "id");
        }
    }
}
