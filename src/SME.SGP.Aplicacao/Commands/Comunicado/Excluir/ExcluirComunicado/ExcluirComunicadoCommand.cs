using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoCommand : IRequest<bool>
    {
        public long[] Ids { get; set; }

        public ExcluirComunicadoCommand(long[] ids)
        {
            Ids = ids;
        }
    }
}
