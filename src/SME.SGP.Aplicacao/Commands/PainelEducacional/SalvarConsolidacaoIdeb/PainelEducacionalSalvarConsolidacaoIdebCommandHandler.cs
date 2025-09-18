using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoIdeb;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PainelEducacionalSalvarConsolidacaoIdebCommandHandler : IRequestHandler<PainelEducacionalSalvarConsolidacaoIdebCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb;
        public PainelEducacionalSalvarConsolidacaoIdebCommandHandler(IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb)
        {
            this.repositorioPainelEducacionalIdeb = repositorioPainelEducacionalIdeb ?? throw new ArgumentNullException(nameof(repositorioPainelEducacionalIdeb));
        }
        public async Task<bool> Handle(PainelEducacionalSalvarConsolidacaoIdebCommand request, CancellationToken cancellationToken)
        {
            if (request.Ideb == null || !request.Ideb.Any())
                return false;

            await repositorioPainelEducacionalIdeb.SalvarIdeb(request.Ideb);

            return true;
        }
    }
}
