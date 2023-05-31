using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommandHandler : IRequestHandler<SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommand, bool>
    {
        private readonly IRepositorioConsolidadoAtendimentoNAAPA repositorio;

        public SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommandHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidadoAtendimentoProfissionalEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(request.Consolidado);
            return true;
        }
    }
}