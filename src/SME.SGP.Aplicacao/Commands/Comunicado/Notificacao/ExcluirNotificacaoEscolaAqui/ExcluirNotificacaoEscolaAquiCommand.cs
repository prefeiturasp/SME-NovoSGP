using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoEscolaAquiCommand : IRequest<bool>
    {
        public ExcluirNotificacaoEscolaAquiCommand(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }
}
