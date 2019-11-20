using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAula : IRepositorioPlanoAula
    {
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
