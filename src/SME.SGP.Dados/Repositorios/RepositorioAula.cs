using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<AulaDto> ObterAulas(long tipoCalendarioId, string turmaId, string ueId)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula");
            MontaWhere(query, turmaId, ueId);
            return (database.Conexao.Query<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId }));
        }

        public IEnumerable<AulaDto> ObterAulas(long tipoCalendarioId, string turmaId, string ueId, int mes)
        {
            StringBuilder query = new StringBuilder();
            MontaCabecalho(query);
            query.AppendLine("FROM public.aula");
            MontaWhere(query, turmaId, ueId, mes);
            return (database.Conexao.Query<AulaDto>(query.ToString(), new { tipoCalendarioId, turmaId, ueId, mes }));
        }

        private static void MontaCabecalho(StringBuilder query)
        {
            query.AppendLine("SELECT id,");
            query.AppendLine("ue_id,");
            query.AppendLine("disciplina_id,");
            query.AppendLine("turma_id,");
            query.AppendLine("tipo_calendario_id,");
            query.AppendLine("professor_id,");
            query.AppendLine("quantidade,");
            query.AppendLine("data_aula,");
            query.AppendLine("recorrencia_aula,");
            query.AppendLine("tipo_aula,");
            query.AppendLine("criado_em,");
            query.AppendLine("criado_por,");
            query.AppendLine("alterado_em,");
            query.AppendLine("alterado_por,");
            query.AppendLine("criado_rf,");
            query.AppendLine("alterado_rf,");
            query.AppendLine("excluido,");
            query.AppendLine("migrado");
        }

        private static void MontaWhere(StringBuilder query, string turmaId, string ueId, int? mes = null)
        {
            query.AppendLine("WHERE tipo_calendario_id = @tipoCalendarioId");
            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("AND turma_id = @turmaId");
            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("AND ue_id = @ueId");
            if (mes.HasValue)
                query.AppendLine("AND extract(month from data_aula) = @mes");
        }
    }
}