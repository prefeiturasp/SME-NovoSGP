using MediatR;
using Sentry;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalDreCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalDreCommand, bool>
    {
        private readonly IMediator mediator;

        public TrataSincronizacaoInstitucionalDreCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(TrataSincronizacaoInstitucionalDreCommand request, CancellationToken cancellationToken)
        {
            var uesCodigo = await mediator.Send(new ObterUesCodigoPorDreSincronizacaoInstitucionalQuery(request.DreCodigo));

            if (!uesCodigo?.Any() ?? true) return true;

            foreach (var ueCodigo in uesCodigo)
            {
                try
                {
                    var publicarSyncTurma = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, ueCodigo, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalUeTratar));
                    if (!publicarSyncTurma)
                    {
                        var mensagem = $"Não foi possível inserir a UE de codígo : {ueCodigo} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }                
            }
            return true;
            //TODO: Tratar caso não haja Ue para a Dre?
        }
    }
}
