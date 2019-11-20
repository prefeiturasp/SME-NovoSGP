using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAula : RepositorioBase<PlanoAula>, IRepositorioPlanoAula
    {
        public RepositorioPlanoAula(ISgpContext conexao) : base(conexao) { }

        public async Task<PlanoAula> ObterPlanoAulaPorDataDisciplina(DateTime data, string disciplinaId)
        {
            var query = @"select pa.*
                 from PlanoAula pa
                inner join Aula a on a.Id = pa.aula_id
                where a.data_aula = @data
                  and a.disciplina_id = @disciplina";

            return database.Conexao.QueryFirstOrDefault<PlanoAula>(query, new { data, disciplinaId });
        }

        public IEnumerable<PlanoAula> Listar()
        {
            throw new NotImplementedException();
        }


        public PlanoAula ObterPorId(long id)
        {
            throw new NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new NotImplementedException();
        }

        public void Remover(PlanoAula entidade)
        {
            throw new NotImplementedException();
        }

        public long Salvar(PlanoAula entidade)
        {
            throw new NotImplementedException();
        }

        public Task<long> SalvarAsync(PlanoAula entidade)
        {
            throw new NotImplementedException();
        }
    }
}
