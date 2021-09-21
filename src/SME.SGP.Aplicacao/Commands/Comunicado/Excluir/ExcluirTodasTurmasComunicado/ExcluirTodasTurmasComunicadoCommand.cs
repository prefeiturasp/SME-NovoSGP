using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodasTurmasComunicadoCommand : IRequest<bool>
    {
        public ExcluirTodasTurmasComunicadoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
