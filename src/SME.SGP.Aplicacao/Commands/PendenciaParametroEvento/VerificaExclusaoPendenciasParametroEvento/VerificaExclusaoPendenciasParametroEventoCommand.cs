using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaExclusaoPendenciasParametroEventoCommand : IRequest<bool>
    {
        public VerificaExclusaoPendenciasParametroEventoCommand(long tipoCalendarioId, string ueCodigo, TipoEvento tipoEvento)
        {
            TipoCalendarioId = tipoCalendarioId;
            UeCodigo = ueCodigo;
            TipoEvento = tipoEvento;
        }

        public long TipoCalendarioId { get; set; }
        public string UeCodigo { get; set; }
        public TipoEvento TipoEvento { get; set; }
    }
}
