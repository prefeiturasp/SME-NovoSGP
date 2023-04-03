using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtualizarNotificacaoMensagemPorIdsCommand : IRequest
    {
        public AtualizarNotificacaoMensagemPorIdsCommand(long[] ids, string mensagem)
        {
            Ids = ids;
            Mensagem = mensagem;
        }

        public string Mensagem { get; set; }

        public long[] Ids { get; set; }
    }
}