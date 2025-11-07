using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoAprovacao
{
    public class LimparConsolidacaoAprovacaoCommandHandler : IRequestHandler<LimparConsolidacaoAprovacaoCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalAprovacao repositorio;

        public LimparConsolidacaoAprovacaoCommandHandler(IRepositorioPainelEducacionalAprovacao repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<bool> Handle(LimparConsolidacaoAprovacaoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.LimparConsolidacao();
            return true;
        }
    }
}
