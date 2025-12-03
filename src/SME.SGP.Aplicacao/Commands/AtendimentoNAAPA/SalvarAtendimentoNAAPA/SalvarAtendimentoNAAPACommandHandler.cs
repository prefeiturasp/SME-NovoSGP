using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtendimentoNAAPACommandHandler : IRequestHandler<SalvarAtendimentoNAAPACommand, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;

        public SalvarAtendimentoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(SalvarAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoNAAPA.SalvarAsync(request.EncaminhamentoNAAPA);
            return true;
        }
    }
}
