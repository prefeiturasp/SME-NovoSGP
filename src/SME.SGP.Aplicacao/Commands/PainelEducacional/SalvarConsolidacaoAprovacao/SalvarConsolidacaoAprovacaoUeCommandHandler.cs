using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao
{
    public class SalvarConsolidacaoAprovacaoUeCommandHandler : IRequestHandler<SalvarConsolidacaoAprovacaoUeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalAprovacaoUe repositorio;

        public SalvarConsolidacaoAprovacaoUeCommandHandler(IRepositorioPainelEducacionalAprovacaoUe repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidacaoAprovacaoUeCommand request, CancellationToken cancellationToken)
        {
            await repositorio.BulkInsertAsync(request.Indicadores);

            return true;
        }
    }
}
