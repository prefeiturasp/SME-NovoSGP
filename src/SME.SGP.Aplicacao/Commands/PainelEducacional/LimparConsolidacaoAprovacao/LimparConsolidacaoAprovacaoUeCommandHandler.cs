using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoAprovacao
{
    class LimparConsolidacaoAprovacaoUeCommandHandler : IRequestHandler<LimparConsolidacaoAprovacaoUeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorio;

        public LimparConsolidacaoAprovacaoUeCommandHandler(IRepositorioPainelEducacionalAprovacaoUe repositorio)
        {
            this.repositorio = repositorio;
        }

        public async Task<bool> Handle(LimparConsolidacaoAprovacaoUeCommand request, CancellationToken cancellationToken)
        {
            await repositorio.LimparConsolidacao();
            return true;
        }
    }
}
