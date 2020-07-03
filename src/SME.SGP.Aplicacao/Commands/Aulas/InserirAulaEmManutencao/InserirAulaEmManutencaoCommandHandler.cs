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
    public class InserirAulaEmManutencaoCommandHandler : IRequestHandler<InserirAulaEmManutencaoCommand, IEnumerable<ProcessoExecutando>>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public InserirAulaEmManutencaoCommandHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ProcessoExecutando>> Handle(InserirAulaEmManutencaoCommand request, CancellationToken cancellationToken)
        {
            var processosExecutando = new List<ProcessoExecutando>();

            foreach(var aulaId in request.AulasIds)
            {
                var processoExecutando = new ProcessoExecutando()
                {
                    TipoProcesso = TipoProcesso.ManutencaoAula,
                    AulaId = aulaId
                };

                await repositorio.SalvarAsync(processoExecutando);

                processosExecutando.Add(processoExecutando);
            }

            return processosExecutando;
        }
    }
}
