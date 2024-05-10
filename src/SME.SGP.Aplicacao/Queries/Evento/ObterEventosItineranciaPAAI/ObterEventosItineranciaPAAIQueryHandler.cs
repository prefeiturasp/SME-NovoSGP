using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosItineranciaPAAIQueryHandler : IRequestHandler<ObterEventosItineranciaPAAIQuery, IEnumerable<EventoNomeDto>>
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ObterEventosItineranciaPAAIQueryHandler(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
        }

        public async Task<IEnumerable<EventoNomeDto>> Handle(ObterEventosItineranciaPAAIQuery request, CancellationToken cancellationToken) 
            => MapearParaDto(await repositorioEvento
                .ListarEventosItinerancia(request.TipoCalendarioId, request.ItineranciaId, request.CodigoUE ,request.Login, request.Perfil));

        private IEnumerable<EventoNomeDto> MapearParaDto(IEnumerable<EventoDataDto> eventos)
        {
            foreach (var evento in eventos)
                yield return new EventoNomeDto()
                {
                    Id = evento.Id,
                    Nome = $"{evento.Data:dd/MM/yyyy} {evento.Nome} ({evento.TipoEscola.ShortName()} {evento.UeNome})"
                };

        }
    }
}
