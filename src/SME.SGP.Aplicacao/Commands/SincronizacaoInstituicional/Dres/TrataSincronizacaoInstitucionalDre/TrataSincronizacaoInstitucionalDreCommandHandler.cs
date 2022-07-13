using MediatR;
using SME.SGP.Dominio.Enumerados;
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
            await LogarInfo("Buscando UEs");
            var uesCodigo = await mediator.Send(new ObterUesCodigoPorDreSincronizacaoInstitucionalQuery(request.DreCodigo));

            if (uesCodigo == null || !uesCodigo.Any()) return true;

            await LogarInfo("Iniciando Foreach");
            foreach (var ueCodigo in uesCodigo)
            {
                try
                {
                    await LogarInfo($"Publicando Fila UE [{ueCodigo}]");
                    var publicarSyncUe = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalUeTratar, ueCodigo, Guid.NewGuid(), null));
                    if (!publicarSyncUe)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir a UE de codígo : {ueCodigo} na fila de sync.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, "Sincronização Institucional de Dre"));
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível inserir a UE de codígo : {ueCodigo} na fila de sync.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, ex.Message));
                }
            }
            return true;
        }

        private Task LogarInfo(string mensagem)
        {
            return mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Informacao, LogContexto.SincronizacaoInstitucional));
        }
    }
}
