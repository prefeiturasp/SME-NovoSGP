using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<AulaConsultaDto> ObterAulasPorFiltro(FiltroAulaDto dto)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("select id");
            sbSql.AppendLine("ue_id,");
            sbSql.AppendLine("disciplina_id,");
            sbSql.AppendLine("turma_id, ");
            sbSql.AppendLine("tipo_calendario_id,");
            sbSql.AppendLine("tipo_aula,");
            sbSql.AppendLine("quantidade,");
            sbSql.AppendLine("data_aula,");
            sbSql.AppendLine("alterado_em,");
            sbSql.AppendLine("alterado_por,");
            sbSql.AppendLine("alterado_rf,");
            sbSql.AppendLine("criado_em,");
            sbSql.AppendLine("criado_por,");
            sbSql.AppendLine("criado_rf");
            sbSql.AppendLine("from aula");
            sbSql.AppendLine("where excluido = false");
            if (dto.UeId > 0L)
            {
                sbSql.AppendLine($"and ue_id = {dto.UeId}");
            }
            if (dto.DisciplinaId > 0L)
            {
                sbSql.AppendLine($"and disciplina_id = {dto.DisciplinaId}");
            }
            if (dto.TurmaId > 0L)
            {
                sbSql.AppendLine($"and turma_id = {dto.TurmaId}");
            }
            if (dto.TipoCalendarioId > 0L)
            {
                sbSql.AppendLine($"and tipo_calendario_id = {dto.TipoCalendarioId}");
            }
            return database.Conexao.Query<AulaConsultaDto>(sbSql.ToString()).ToList();
        }
    }
}
