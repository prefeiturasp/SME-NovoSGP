using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirFluenciaLeitora
{
    public class PainelEducacionalExcluirFluenciaLeitoraCommandHandler : IRequestHandler<PainelEducacionalExcluirFluenciaLeitoraCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFluenciaLeitora repositorioFluenciaLeitora;
        public PainelEducacionalExcluirFluenciaLeitoraCommandHandler(IRepositorioConsolidacaoFluenciaLeitora repositorioFluenciaLeitora)
        {
            this.repositorioFluenciaLeitora = repositorioFluenciaLeitora;
        }
        public async Task<bool> Handle(PainelEducacionalExcluirFluenciaLeitoraCommand request, CancellationToken cancellationToken)
        {
            await repositorioFluenciaLeitora.ExcluirConsolidacaoFluenciaLeitora();

            return true;
        }
    }
}
