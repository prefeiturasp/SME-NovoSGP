using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverProcessoEmExecucaoCommandHandler : IRequestHandler<RemoverProcessoEmExecucaoCommand, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public RemoverProcessoEmExecucaoCommandHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(RemoverProcessoEmExecucaoCommand request, CancellationToken cancellationToken)
        {
            await repositorio.RemoverIdsAsync(request.ProcessosExecutandoIds);
            return true;
        }
    }
}
