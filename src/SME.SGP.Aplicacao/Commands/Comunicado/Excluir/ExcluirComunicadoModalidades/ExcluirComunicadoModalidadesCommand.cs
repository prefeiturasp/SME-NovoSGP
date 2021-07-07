using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoModalidadesCommand : IRequest<bool>
    {
        public ExcluirComunicadoModalidadesCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
