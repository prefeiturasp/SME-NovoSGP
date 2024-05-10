using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodosAlunosComunicadoCommand : IRequest<bool>
    {
        public ExcluirTodosAlunosComunicadoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
