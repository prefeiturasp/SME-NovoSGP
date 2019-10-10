namespace SME.SGP.Aplicacao
{
    public interface IServicoEmail
    {
        void Enviar(string destinatario, string assunto, string mensagemHtml);
    }
}