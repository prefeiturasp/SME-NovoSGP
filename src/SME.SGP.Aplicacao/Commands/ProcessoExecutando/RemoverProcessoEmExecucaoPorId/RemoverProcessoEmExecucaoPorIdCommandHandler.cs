using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverProcessoEmExecucaoPorIdCommandHandler : IRequestHandler<RemoverProcessoEmExecucaoPorIdCommand, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public RemoverProcessoEmExecucaoPorIdCommandHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(RemoverProcessoEmExecucaoPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorio.RemoverIdsAsync(new[] { request.Id });

            return true;
        }
    }
}
