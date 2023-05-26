using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoEncaminhamentoNAAPACommandHandler : IRequestHandler<SalvarConsolidadoEncaminhamentoNAAPACommand,bool>
    {
        private readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorio;

        public SalvarConsolidadoEncaminhamentoNAAPACommandHandler(IRepositorioConsolidadoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidadoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(request.Consolidado);
            return true;
        }
    }
}