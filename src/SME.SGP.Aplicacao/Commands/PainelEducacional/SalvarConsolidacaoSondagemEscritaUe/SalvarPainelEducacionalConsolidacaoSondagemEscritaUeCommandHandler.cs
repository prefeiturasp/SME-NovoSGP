using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoSondagemEscrita
{
    public class SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe repositorioPainelEducacionalConsolidacaoSondagemEscrita;

        public SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommandHandler(IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe repositorioPainelEducacionalConsolidacaoSondagemEscrita)
        {
            this.repositorioPainelEducacionalConsolidacaoSondagemEscrita = repositorioPainelEducacionalConsolidacaoSondagemEscrita;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand request, CancellationToken cancellationToken)
        {
            if (request.Indicadores?.Any() != true)
                return false;

            await repositorioPainelEducacionalConsolidacaoSondagemEscrita.LimparConsolidacao();

            await repositorioPainelEducacionalConsolidacaoSondagemEscrita.BulkInsertAsync(request.Indicadores);

            return true;
        }
    }
}
