using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoRelatorioCorrelacao : IComandoRelatorioCorrelacao
    {
        private readonly IRepositorioRelatorioCorrelacao repositorio;
        private readonly IUnitOfWork unitOfWork;

        public ComandoRelatorioCorrelacao(IRepositorioRelatorioCorrelacao repositorio,
            IUnitOfWork unitOfWork)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }

        //TODO: Criar DTO
        public Task<long> Salvar(RelatorioCorrelacao relatorioCorrelacao)
        {
            return repositorio.SalvarAsync(relatorioCorrelacao);
        }
    }
}