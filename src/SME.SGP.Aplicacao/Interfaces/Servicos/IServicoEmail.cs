namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoEmail
    {
        void Enviar(string destinatario, string assunto, string mensagemHtml);
    }
}