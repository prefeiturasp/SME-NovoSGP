namespace SME.SGP.Infra.Dtos
{
    public class NotificacaoInformativoUsuarioFiltro
    {
        public long InformativoId { get; set; }
        public string UsuarioRf { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }
}
