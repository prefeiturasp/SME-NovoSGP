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

            if (uesCodigo == null || !uesCodigo.Any()) return true;            

            foreach (var ueCodigo in uesCodigo)
            {
                try
                {
                    var publicarSyncUe = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalUeTratar, ueCodigo, Guid.NewGuid(), null));
                    if (!publicarSyncUe)
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
        }
    }
}
