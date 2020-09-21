using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ValidaSeExisteTipoCalendarioPorIdQuery : IRequest<bool>
    {
        public ValidaSeExisteTipoCalendarioPorIdQuery(long tipoCalendarioId)
        {
            TipoCalendarioId = tipoCalendarioId;
        }
        public long TipoCalendarioId { get; set; }
    }
}
