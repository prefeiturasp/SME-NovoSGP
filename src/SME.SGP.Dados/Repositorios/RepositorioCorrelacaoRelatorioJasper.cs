using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioCorrelacaoJasper : IRepositorioRelatorioCorrelacaoJasper
    {
        private readonly ISgpContext contexto;

        public RepositorioRelatorioCorrelacaoJasper(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public IEnumerable<RelatorioCorrelacaoJasper> Listar()
        {
            throw new System.NotImplementedException();
        }

        public RelatorioCorrelacaoJasper ObterPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RelatorioCorrelacaoJasper> ObterPorIdAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(RelatorioCorrelacaoJasper entidade)
        {
            throw new System.NotImplementedException();
        }

        public long Salvar(RelatorioCorrelacaoJasper entidade)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> SalvarAsync(RelatorioCorrelacaoJasper entidade)
        {
            throw new System.NotImplementedException();
        }
    }
}