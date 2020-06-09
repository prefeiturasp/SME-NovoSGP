using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioCorrelacao : IRepositorioRelatorioCorrelacao
    {
        private readonly ISgpContext contexto;

        public RepositorioRelatorioCorrelacao(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<RelatorioCorrelacao> Listar()
        {
            throw new System.NotImplementedException();
        }

        public RelatorioCorrelacao ObterPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RelatorioCorrelacao> ObterPorIdAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(RelatorioCorrelacao entidade)
        {
            throw new System.NotImplementedException();
        }

        public long Salvar(RelatorioCorrelacao entidade)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> SalvarAsync(RelatorioCorrelacao entidade)
        {
            throw new System.NotImplementedException();
        }
    }
}