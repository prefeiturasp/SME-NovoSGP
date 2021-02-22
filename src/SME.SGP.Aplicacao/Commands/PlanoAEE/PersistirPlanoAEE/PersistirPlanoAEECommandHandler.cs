using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PersistirPlanoAEECommandHandler : IRequestHandler<PersistirPlanoAEECommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public PersistirPlanoAEECommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(PersistirPlanoAEECommand request, CancellationToken cancellationToken)
            => (await repositorioPlanoAEE.SalvarAsync(request.PlanoAEE)) > 0;
    }
}
