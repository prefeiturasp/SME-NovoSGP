using MediatR;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoItineranciaCommand : IRequest<bool>
    {
        public AlterarSituacaoItineranciaCommand(long itineranciaId, SituacaoItinerancia situacao)
        {
            this.ItineranciaId = itineranciaId;
            this.Situacao = situacao;
        }

        public long ItineranciaId { get; set; }
        public SituacaoItinerancia Situacao { get; set; }

    }
}
