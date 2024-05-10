using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirProcessoEmExecucaoCommandHandler : IRequestHandler<IncluirProcessoEmExecucaoCommand, long>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public IncluirProcessoEmExecucaoCommandHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(IncluirProcessoEmExecucaoCommand request, CancellationToken cancellationToken)
        {
            var processoExecutando = new ProcessoExecutando()
            {
                TipoProcesso = request.TipoProcesso
            };

            return await repositorio.SalvarAsync(processoExecutando);
        }
    }
}
