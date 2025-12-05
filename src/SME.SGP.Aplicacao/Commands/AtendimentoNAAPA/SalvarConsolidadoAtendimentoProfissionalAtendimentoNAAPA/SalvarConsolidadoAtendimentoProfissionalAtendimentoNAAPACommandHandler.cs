using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommandHandler : IRequestHandler<SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommand, bool>
    {
        private readonly IRepositorioConsolidadoAtendimentoNAAPA repositorio;

        public SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommandHandler(IRepositorioConsolidadoAtendimentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(SalvarConsolidadoAtendimentoProfissionalAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(request.Consolidado);
            return true;
        }
    }
}