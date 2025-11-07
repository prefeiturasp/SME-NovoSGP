using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao
{
    public class SalvarConsolidacaoAprovacaoCommandHandler : IRequestHandler<SalvarConsolidacaoAprovacaoCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalAprovacao repositorio;

        public SalvarConsolidacaoAprovacaoCommandHandler(IRepositorioPainelEducacionalAprovacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidacaoAprovacaoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.BulkInsertAsync(request.Indicadores);

            return true;
        }
    }
}
