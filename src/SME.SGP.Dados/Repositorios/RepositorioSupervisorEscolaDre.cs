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

        public IEnumerable<SupervisorEscolasDto> ObtemSupervisoresEscola(long dreId, long supervisorId)
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("select *");
            query.AppendLine("from supervisor_escola_dre");

            //query.AppendLine("where");
            //query.AppendLine("tca.ano = @ano");

            return database.Conexao.Query<SupervisorEscolasDto>(query.ToString()).AsList();
        }
    }
}