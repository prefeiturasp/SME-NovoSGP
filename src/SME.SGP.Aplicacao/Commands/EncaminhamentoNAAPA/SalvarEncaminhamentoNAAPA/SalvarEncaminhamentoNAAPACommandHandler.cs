using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoNAAPACommandHandler : IRequestHandler<SalvarEncaminhamentoNAAPACommand, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public SalvarEncaminhamentoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(SalvarEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoNAAPA.SalvarAsync(request.EncaminhamentoNAAPA);
            return true;
        }
    }
}
