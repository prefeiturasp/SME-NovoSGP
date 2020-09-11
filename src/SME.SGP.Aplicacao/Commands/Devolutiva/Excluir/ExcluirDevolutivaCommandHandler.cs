using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDevolutivaCommandHandler : IRequestHandler<ExcluirDevolutivaCommand, bool>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ExcluirDevolutivaCommandHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<bool> Handle(ExcluirDevolutivaCommand request, CancellationToken cancellationToken)
        {
            repositorioDevolutiva.Remover(request.DevolutivaId);
            return await Task.FromResult(true);
        }
    }
}
