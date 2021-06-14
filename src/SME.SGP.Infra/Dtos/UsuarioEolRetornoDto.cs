namespace SME.SGP.Infra
{
    public class UsuarioEolRetornoDto
    {
        public string CodigoRf { get; set; }
        public string NomeServidor { get; set; }
        public int CodigoFuncaoAtividade { get; set; }
        public long UsuarioId { get; set; }
        public bool EstaAfastado { get; set; }
    }
}