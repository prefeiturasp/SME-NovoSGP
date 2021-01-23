using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoAEECommandHandler : IRequestHandler<SalvarEncaminhamentoAEECommand, bool>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public SalvarEncaminhamentoAEECommandHandler(IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(SalvarEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoAEE.SalvarAsync(request.EncaminhamentoAEE);
            return true;
        }
    }
}
