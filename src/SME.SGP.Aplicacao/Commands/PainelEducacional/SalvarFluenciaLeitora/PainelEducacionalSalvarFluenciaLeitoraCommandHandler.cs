using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarFluenciaLeitora
{
    public class PainelEducacionalSalvarFluenciaLeitoraCommandHandler : IRequestHandler<PainelEducacionalSalvarFluenciaLeitoraCommand, bool>
    {
        private readonly IRepositorioConsolidacaoFluenciaLeitora repositorioFluenciaLeitora;

        public PainelEducacionalSalvarFluenciaLeitoraCommandHandler(IRepositorioConsolidacaoFluenciaLeitora repositorioFluenciaLeitora)
        {
            this.repositorioFluenciaLeitora = repositorioFluenciaLeitora;
        }

        public async Task<bool> Handle(PainelEducacionalSalvarFluenciaLeitoraCommand request, CancellationToken cancellationToken)
        {
            await repositorioFluenciaLeitora.BulkInsertAsync(request.FluenciaLeitora);
            return true;
        }
    }
}
