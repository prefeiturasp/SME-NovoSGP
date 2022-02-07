using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioDeBordoSemObservacoesPorIdQueryHandler : IRequestHandler<ObterDiarioDeBordoSemObservacoesPorIdQuery, DiarioBordoListaoDto>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;

        public ObterDiarioDeBordoSemObservacoesPorIdQueryHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<DiarioBordoListaoDto> Handle(ObterDiarioDeBordoSemObservacoesPorIdQuery request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorIdAsync(request.Id);

            return MapearParaDto(diarioBordo);
        }

        private DiarioBordoListaoDto MapearParaDto(Dominio.DiarioBordo diarioBordo)
        {
            return new DiarioBordoListaoDto()
            {
                Auditoria = (AuditoriaDto)diarioBordo,
                Planejamento = diarioBordo.Planejamento,
            };
        }
    }
}
