using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ProcessoEstaEmExecucaoQueryHandler : IRequestHandler<ProcessoEstaEmExecucaoQuery, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public ProcessoEstaEmExecucaoQueryHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ProcessoEstaEmExecucaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ProcessoEstaEmExecucao(request.TipoProcesso);
        }
    }
}
