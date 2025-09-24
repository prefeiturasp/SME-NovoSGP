using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirIdeb
{
    public class PainelEducacionalExcluirIdebCommandHandler : IRequestHandler<PainelEducacionalExcluirIdebCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb;
        public PainelEducacionalExcluirIdebCommandHandler(IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb)
        {
            this.repositorioPainelEducacionalIdeb = repositorioPainelEducacionalIdeb;
        }
        public async Task<bool> Handle(PainelEducacionalExcluirIdebCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalIdeb.ExcluirIdeb();
            return true;
        }
    }
}
