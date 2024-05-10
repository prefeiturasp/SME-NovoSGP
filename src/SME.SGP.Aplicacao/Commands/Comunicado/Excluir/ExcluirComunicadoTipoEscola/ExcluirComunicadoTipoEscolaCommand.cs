using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoTipoEscolaCommand : IRequest<bool>
    {
        public ExcluirComunicadoTipoEscolaCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
