using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDiarioBordoCommandHandler : IRequestHandler<AlterarDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public AlterarDiarioBordoCommandHandler(IMediator mediator,
                                                IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<AuditoriaDto> Handle(AlterarDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            if (!await mediator.Send(new AulaExisteQuery(request.AulaId)))
                throw new NegocioException("Aula informada não existe");

            var diarioBordo = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId);
            if (diarioBordo == null)
                throw new NegocioException($"Diário de Bordo para a aula {request.AulaId} não encontrado!");

            MapearAlteracoes(diarioBordo, request);

            await repositorioDiarioBordo.SalvarAsync(diarioBordo);

            return (AuditoriaDto)diarioBordo;
        }

        private void MapearAlteracoes(DiarioBordo entidade, AlterarDiarioBordoCommand request)
        {
            entidade.Planejamento = request.Planejamento;
            entidade.ReflexoesReplanejamento = request.ReflexoesReplanejamento;
        }
    }
}
