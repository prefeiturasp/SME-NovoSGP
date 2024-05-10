using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoAnoEscolarCommand : IRequest<bool>
    {
        public ExcluirComunicadoAnoEscolarCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
