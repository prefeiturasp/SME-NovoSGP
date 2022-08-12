using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroNotificarUsuarioAlteracaoExtemporaneaDto
    {
        public FiltroNotificarUsuarioAlteracaoExtemporaneaDto(AtividadeAvaliativa atividadeAvaliativa, string mensagemNotificacao, string turmaNome, string codigoUe)
        {
            AtividadeAvaliativa = atividadeAvaliativa;
            MensagemNotificacao = mensagemNotificacao;
            TurmaNome = turmaNome;
            CodigoUe = codigoUe;
        }

        public AtividadeAvaliativa AtividadeAvaliativa { get; set; }
        public string TurmaNome { get; set; }
        public string CodigoUe { get; set; }
        public string MensagemNotificacao { get; set; }
    }
}
