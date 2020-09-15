using MediatR;
using Org.BouncyCastle.Ocsp;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class ExcluirDiarioBordoDaAulaCommandHandler : IRequestHandler<ExcluirDiarioBordoDaAulaCommand, bool>
    {
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IMediator mediator;

        public ExcluirDiarioBordoDaAulaCommandHandler(IRepositorioDiarioBordo repositorioDiarioBordo, IMediator mediator)
        {
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
        }

        public async Task<bool> Handle(ExcluirDiarioBordoDaAulaCommand request, CancellationToken cancellationToken)
        {
            var diarioBordo = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId);
            var observacoesId = await repositorioDiarioBordo.ObterObservacaoPorId(diarioBordo.Id);

            await repositorioDiarioBordo.ExcluirDiarioBordoDaAula(request.AulaId);

            foreach (long observacaoId in observacoesId)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaExcluirNotificacaoDiarioBordo,
                          new ExcluirNotificacaoDiarioBordoDto(observacaoId), Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
