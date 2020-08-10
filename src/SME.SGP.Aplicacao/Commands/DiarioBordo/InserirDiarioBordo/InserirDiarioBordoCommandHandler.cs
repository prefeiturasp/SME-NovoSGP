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
    public class InserirDiarioBordoCommandHandler : IRequestHandler<InserirDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;

        public InserirDiarioBordoCommandHandler(IMediator mediator,
                                                IRepositorioDiarioBordo repositorioDiarioBordo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<AuditoriaDto> Handle(InserirDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            if (!await mediator.Send(new AulaExisteQuery(request.AulaId)))
                throw new NegocioException("Aula informada não existe");

            var diarioBordo = MapearParaEntidade(request);

            await repositorioDiarioBordo.SalvarAsync(diarioBordo);

            return (AuditoriaDto)diarioBordo;
        }

        private DiarioBordo MapearParaEntidade(InserirDiarioBordoCommand request)
            => new DiarioBordo()
            { 
                AulaId = request.AulaId,
                Planejamento = request.Planejamento,
                ReflexoesReplanejamento = request.ReflexoesReplanejamento
            };
    }
}
