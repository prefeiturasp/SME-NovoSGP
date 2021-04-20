using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CriarEventoItineranciaPAAICommandHandler : IRequestHandler<CriarEventoItineranciaPAAICommand, bool>
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioItineranciaEvento repositorioItineranciaEvento;
        private readonly IMediator mediator;

        public CriarEventoItineranciaPAAICommandHandler(IMediator mediator, IRepositorioEvento repositorioEvento, IRepositorioItineranciaEvento repositorioItineranciaEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioItineranciaEvento = repositorioItineranciaEvento ?? throw new ArgumentNullException(nameof(repositorioItineranciaEvento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarEventoItineranciaPAAICommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var tipoCalendarioId = await ObterTipoCalendarioId(request.UeCodigo);
            var tipoEventoId = await ObterTipoEventoItinerancia();

            var evento = new Evento()
            {
                TipoCalendarioId = tipoCalendarioId,
                TipoEventoId = tipoEventoId,
                DreId = request.DreCodigo,
                UeId = request.UeCodigo,
                DataInicio = request.DataEvento,
                DataFim = request.DataEvento,
                Letivo = EventoLetivo.Opcional,
                Nome = ObterNomeEvento(request, usuario),
                Descricao = ObterDescricaoEvento(request)
            };

            try
            {
                await repositorioEvento.SalvarAsync(evento);
                await repositorioItineranciaEvento.SalvarAsync(new ItineranciaEvento(request.ItineranciaId, evento.Id));

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao salvar evento da itinerância: {e.Message}");
            }        
        }

        private async Task<long> ObterTipoEventoItinerancia()
            => await mediator.Send(new ObterEventoTipoIdPorCodigoQuery(TipoEvento.ItineranciaPAAI));

        private async Task<long> ObterTipoCalendarioId(string ueCodigo)
        {
            var anoLetivo = DateTime.Now.Year;
            var semestre = DateTime.Now.Semestre();

            return await mediator.Send(new ObterTipoCalendarioIdPorCodigoUEQuery(ueCodigo, anoLetivo, semestre));
        }

        private string ObterNomeEvento(CriarEventoItineranciaPAAICommand request, Usuario usuario)
            => $"Itinerância PAAI {usuario.Nome}";

        private string ObterDescricaoEvento(CriarEventoItineranciaPAAICommand request)
        {
            var descricao = new StringBuilder();
            descricao.AppendLine($"Evento cadastrado automaticamente a partir do registro de itinerância do dia {request.DataItinerancia:dd/MM/yyyy}");
            
            descricao.AppendLine("Objetivos:");
            foreach(var objetivo in request.Objetivos)
                descricao.AppendLine($" - {objetivo.Nome}");

            return descricao.ToString();
        }
    }
}
