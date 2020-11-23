using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCalendarioUeCommandHandler : IRequestHandler<SalvarPendenciaCalendarioUeCommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public SalvarPendenciaCalendarioUeCommandHandler(IMediator mediator, IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }

        public async Task<long> Handle(SalvarPendenciaCalendarioUeCommand request, CancellationToken cancellationToken)
        {
            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(request.TipoPendencia, request.Descricao, request.Instrucao));         
                        
             await SalvarPendenciaUsuario(await ObterAdministradoresPorUE(request.Ue.CodigoUe), pendenciaId);

            return await repositorioPendenciaCalendarioUe.SalvarAsync(new Dominio.PendenciaCalendarioUe()
            {
                PendenciaId = pendenciaId,
                UeId = request.Ue.Id,
                TipoCalendarioId = request.TipoCalendarioId
            });
        }      

        private async Task<IList<long>> ObterAdministradoresPorUE(string CodigoUe)
        {
            var administradoresId = await mediator.Send(new ObterAdministradoresPorUEQuery(CodigoUe));
            IList<long> AdministradoresUeId = new List<long>();

            foreach (var adm in administradoresId)
            {
                AdministradoresUeId.Add(await ObterUsuarioId(adm));
            }
            return AdministradoresUeId;
        }

        private async Task<bool> SalvarPendenciaUsuario(IList<long> administradoresUeId, long pendenciaId)
        {
            if (administradoresUeId.Count > 0)
                foreach (var id in administradoresUeId)
                {
                    try
                    {
                        await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, id));
                    }
                    catch (Exception e)
                    {
                        SentrySdk.CaptureException(e);
                    }
                }
            return true;
        }

        private async Task<long> ObterUsuarioId(string rf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rf));

            return usuarioId;
        }
    }
}
