using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoBimestre : RepositorioBase<EventoBimestre>, IRepositorioEventoBimestre

    {
        public RepositorioEventoBimestre(ISgpContext conexao) : base(conexao)
        {

        }

        public async Task<int[]> ObterEventoBimestres(long eventoId)
        {
            string query = ("select bimestre from evento_bimestre where evento_id = @evento_id");
            var bimestres = await database.Conexao.QueryAsync<int>(query, new { eventoId });

            return bimestres.ToArray();

        }

        public Task ExcluiEventoBimestre(long eventoId)
        {
            string query = @"delete from evento_bimestre where evento_id =  @eventoId";

            database.Execute(query, new { eventoId });
            return Task.CompletedTask;
        }
    }
}
