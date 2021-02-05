using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEE : RepositorioBase<PlanoAEE>, IRepositorioPlanoAEE
    {
        public RepositorioPlanoAEE(ISgpContext database) : base(database)
        {
        }

        public void Remover(PlanoAEE entidade)
        {
            throw new NotImplementedException();
        }

        public long Salvar(PlanoAEE entidade)
        {
            throw new NotImplementedException();
        }

        public Task<long> SalvarAsync(PlanoAEE entidade)
        {
            throw new NotImplementedException();
        }

        IEnumerable<PlanoAEE> IRepositorioBase<PlanoAEE>.Listar()
        {
            throw new NotImplementedException();
        }

        PlanoAEE IRepositorioBase<PlanoAEE>.ObterPorId(long id)
        {
            throw new NotImplementedException();
        }

        Task<PlanoAEE> IRepositorioBase<PlanoAEE>.ObterPorIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
