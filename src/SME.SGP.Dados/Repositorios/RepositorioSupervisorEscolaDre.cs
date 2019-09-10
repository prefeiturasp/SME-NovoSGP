using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSupervisorEscolaDre : RepositorioBase<SupervisorEscolaDre>, IRepositorioSupervisorEscolaDre
    {
        public RepositorioSupervisorEscolaDre(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisor(string dreId, string supervisorId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select id, dre_id, escola_id, supervisor_id, criado_em, criado_por, alterado_em, alterado_por ");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(supervisorId))
                query.AppendLine("and sed.supervisor_id = @supervisorId");

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and sed.dre_id = @dreId");

            return database.Conexao.Query<SupervisorEscolasDreDto>(query.ToString(), new { supervisorId, dreId }).AsList();
        }

        public IEnumerable<SupervisorEscolasDreDto> ObtemPorDreESupervisores(string dreId, string[] supervisoresId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select id, dre_id, escola_id, supervisor_id, criado_em, criado_por, alterado_em, alterado_por ");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where 1=1");

            if (supervisoresId.Length > 0)
            {
                var idsSupervisores = from a in supervisoresId
                                      select $"'{a}'";

                query.AppendLine($"and sed.supervisor_id in ({string.Join(",", idsSupervisores)})");
            }

            if (!string.IsNullOrEmpty(dreId))
                query.AppendLine("and sed.dre_id = @dreId");

            return database.Conexao.Query<SupervisorEscolasDreDto>(query.ToString(), new { dreId }).AsList();
        }

        public SupervisorEscolasDreDto ObtemPorUe(string ueId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select id, dre_id, escola_id, supervisor_id, criado_em, criado_por, alterado_em, alterado_por ");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where escola_id = @ueId");

            return database.Conexao.Query<SupervisorEscolasDreDto>(query.ToString(), new { ueId })
                .AsList()
                .FirstOrDefault();
        }
    }
}