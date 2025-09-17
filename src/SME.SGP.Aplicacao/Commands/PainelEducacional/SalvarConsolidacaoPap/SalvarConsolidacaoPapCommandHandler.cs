using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapCommandHandler : IRequestHandler<SalvarConsolidacaoPapCommand, bool>
    {
        private readonly IRepositorioPapPainelEducacionalConsolidacao repositorioPapConsolidacao;

        public SalvarConsolidacaoPapCommandHandler(IRepositorioPapPainelEducacionalConsolidacao repositorioPapConsolidacao)
        {
            this.repositorioPapConsolidacao = repositorioPapConsolidacao ?? throw new ArgumentNullException(nameof(repositorioPapConsolidacao));
        }

        public async Task<bool> Handle(SalvarConsolidacaoPapCommand request, CancellationToken cancellationToken)
        {
            if (request.IndicadoresPap?.Any() != true)
                return false;

            await repositorioPapConsolidacao.LimparConsolidacao();
            
            await repositorioPapConsolidacao.Inserir(request.IndicadoresPap);

            return true;
        }
    }
}