namespace SME.SGP.Dto
{
    public class UsuarioEolAutenticacaoRetornoDto
    {
        public string CodigoRf { get; set; }
        public string[] Permissoes { get; set; }
        public AutenticacaoStatusEol Status { get; set; }
    }
}