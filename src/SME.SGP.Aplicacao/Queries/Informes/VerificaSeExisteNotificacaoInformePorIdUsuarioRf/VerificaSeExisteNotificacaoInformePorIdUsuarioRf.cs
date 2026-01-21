using MediatR;

namespace SME.SGP.Aplicacao.Queries.Informes.VerificaSeExisteNotificacaoInformePorIdUsuarioRf
{
    public class VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery : IRequest<bool>
    {
        public VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery(long informativoId, string usuarioRf)
        {
            InformativoId = informativoId;
            UsuarioRf = usuarioRf;
        }

        public long InformativoId { get; set; }

        public string UsuarioRf { get; set; }
    }
}