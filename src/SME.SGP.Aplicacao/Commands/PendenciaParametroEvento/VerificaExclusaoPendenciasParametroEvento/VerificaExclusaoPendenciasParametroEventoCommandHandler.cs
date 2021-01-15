using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasParametroEventoCommandHandler : IRequestHandler<VerificaExclusaoPendenciasParametroEventoCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaExclusaoPendenciasParametroEventoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Handle(VerificaExclusaoPendenciasParametroEventoCommand request, CancellationToken cancellationToken)
        {
            var anoAtual = DateTime.Now.Year;
            var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(request.UeCodigo));

            var parametroQuantidadeEventos = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(ObterTipoParametro(request.TipoEvento), anoAtual));
            var pendenciaParametroEvento = await mediator.Send(new ObterPendenciaParametroEventoPorCalendarioUeParametroQuery(request.TipoCalendarioId, ue.Id, parametroQuantidadeEventos.Id));
            if (pendenciaParametroEvento != null)
            {
                var eventos = await mediator.Send(new ObterEventosPorTipoECalendarioUeQuery(request.TipoCalendarioId, request.UeCodigo, request.TipoEvento));
                if (EventosSuficientes(eventos, int.Parse(parametroQuantidadeEventos.Valor)))
                    await mediator.Send(new ExcluirPendenciaParametroEventoCommand(pendenciaParametroEvento));
                else
                    await AtualizarPendenciaParametroEvento(pendenciaParametroEvento, int.Parse(parametroQuantidadeEventos.Valor) - eventos.Count());
            }

            return true;
        }

        private async Task AtualizarPendenciaParametroEvento(PendenciaParametroEvento pendenciaParametroEvento, int quantidadeEventos)
        {
            pendenciaParametroEvento.QuantidadeEventos = quantidadeEventos;
            await mediator.Send(new AtualizarPendenciaParametroEventoCommand(pendenciaParametroEvento));
        }

        private bool EventosSuficientes(IEnumerable<Evento> eventos, int quantidadeEventosParametro)
        {
            return quantidadeEventosParametro == 0
                || ((eventos != null) && (eventos.Count() >= quantidadeEventosParametro));
        }


        private TipoParametroSistema ObterTipoParametro(TipoEvento tipoEvento)
        {
            switch (tipoEvento)
            {
                case TipoEvento.ConselhoDeClasse:
                    return TipoParametroSistema.QuantidadeEventosConselhoClasse;
                case TipoEvento.ReuniaoPedagogica:
                    return TipoParametroSistema.QuantidadeEventosPedagogicos;
                case TipoEvento.ReuniaoAPM:
                    return TipoParametroSistema.QuantidadeEventosAPM;
                case TipoEvento.ReuniaoConselhoEscola:
                    return TipoParametroSistema.QuantidadeEventosConselhoEscolar;
                default:
                    throw new NegocioException("Tipo de Evento não mapeado para parâmetro");
            }
        }
    }
}
