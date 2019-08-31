using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSupervisorEscolaDre : RepositorioBase<SupervisorEscolaDre>, IRepositorioSupervisorEscolaDre
    {
        public RepositorioSupervisorEscolaDre(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<SupervisorEscolasDreDto> ObtemSupervisoresEscola(string dreId, string supervisorId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from supervisor_escola_dre sed");
            query.AppendLine("where");
            query.AppendLine("sed.id_supervisor = @supervisorId");

            //var parametros = new { IdSupervisor = supervisorId };

            if (!string.IsNullOrEmpty(dreId))
            {
                query.AppendLine(" and sed.id_dre = @dreId");
                //parametros =  parametros.IdDre = dreId;
            }

            return database.Conexao.Query<SupervisorEscolasDreDto>(query.ToString(), new { supervisorId, dreId }).AsList();
        }
    }
}