using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirAulaEmManutencaoCommandHandler : IRequestHandler<InserirAulaEmManutencaoCommand, ProcessoExecutando>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public InserirAulaEmManutencaoCommandHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<ProcessoExecutando> Handle(InserirAulaEmManutencaoCommand request, CancellationToken cancellationToken)
        {
            var processoExecutando = new ProcessoExecutando()
            {
                TipoProcesso = TipoProcesso.ManutencaoAula,
                AulaId = request.AulaId
            };

            await repositorio.SalvarAsync(processoExecutando);

            return processoExecutando;
        }
    }
}
