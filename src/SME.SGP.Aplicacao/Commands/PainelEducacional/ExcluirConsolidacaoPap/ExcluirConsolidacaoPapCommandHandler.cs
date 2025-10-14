using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirConsolidacaoPap
{
    public class ExcluirConsolidacaoPapCommandHandler : IRequestHandler<ExcluirConsolidacaoPapCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap;
        public ExcluirConsolidacaoPapCommandHandler(IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap ?? throw new ArgumentNullException(nameof(repositorioPainelEducacionalPap));
        }
        public async Task<bool> Handle(ExcluirConsolidacaoPapCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalPap.ExcluirConsolidacaoPorAno(request.AnoLetivo);
            return true;
        }
    }
}