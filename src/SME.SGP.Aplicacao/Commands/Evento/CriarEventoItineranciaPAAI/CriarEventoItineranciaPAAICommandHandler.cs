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
        private readonly IMediator mediator;

        public CriarEventoItineranciaPAAICommandHandler(IRepositorioEvento repositorioEvento, IMediator mediator)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarEventoItineranciaPAAICommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            var evento = new Evento()
            {
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

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao salvar evento da itinerância: {e.Message}");
            }        
        }

        private string ObterNomeEvento(CriarEventoItineranciaPAAICommand request, Usuario usuario)
            => $"tinerância PAAI {usuario.Nome}";

        private string ObterDescricaoEvento(CriarEventoItineranciaPAAICommand request)
        {
            var descricao = new StringBuilder();
            descricao.AppendLine($"Evento cadastrado automaticamente a partir do registro de itinerância do dia {request.DataItinerancia:dd/MM/yyyy}");
            
            descricao.AppendLine("Objetivos:");
            foreach(var objetivo in request.Objetivos)
            {
                var complemento = !string.IsNullOrEmpty(objetivo.Descricao) ? $": {objetivo.Descricao}" : "";
                descricao.AppendLine($" - {objetivo.Nome}{complemento}");
            }

            return descricao.ToString();
        }
    }
}
