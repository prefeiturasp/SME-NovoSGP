using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(request.TipoPendencia, request.Ue.Id, request.Descricao, request.Instrucao));
            await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, ObterCodigoPerfilParaPendencia(request.TipoPendencia)));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, request.Ue.Id), Guid.NewGuid()));

            return await repositorioPendenciaCalendarioUe.SalvarAsync(new Dominio.PendenciaCalendarioUe()
            {
                PendenciaId = pendenciaId,
                UeId = request.Ue.Id,
                TipoCalendarioId = request.TipoCalendarioId
            });
        }

        private string[] ObterPerfisParaPendencia(TipoPendencia tipoPendencia)
        {
            switch (tipoPendencia)
            {
                case TipoPendencia.CalendarioLetivoInsuficiente:
                    return new string[] { "CP", "AD", "Diretor", "ADM UE" };
                case TipoPendencia.CadastroEventoPendente:
                    return new string[] { "ADM UE" };
                default:
                    return new string[] { };
            }
        }

        public List<PerfilUsuario> ObterCodigoPerfilParaPendencia(TipoPendencia tipoPendencia)
        {
            switch (tipoPendencia)
            {
                case TipoPendencia.CalendarioLetivoInsuficiente:
                    return new List<PerfilUsuario> { PerfilUsuario.CP, PerfilUsuario.AD, PerfilUsuario.DIRETOR, PerfilUsuario.ADMUE };
                case TipoPendencia.CadastroEventoPendente:
                    return new List<PerfilUsuario> {PerfilUsuario.ADMUE };
                default:
                    return null;
            }
        }

    }
}
