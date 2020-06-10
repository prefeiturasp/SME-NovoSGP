using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoRelatorioCorrelacaoJasper : IComandoRelatorioCorrelacaoJasper
    {
        private readonly IRepositorioRelatorioCorrelacaoJasper repositorio;

        public ComandoRelatorioCorrelacaoJasper(IRepositorioRelatorioCorrelacaoJasper repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<long> Salvar(RelatorioCorrelacaoJasper relatorioCorrelacaoJasper)
        {
            return repositorio.SalvarAsync(relatorioCorrelacaoJasper);
        }
    }
}