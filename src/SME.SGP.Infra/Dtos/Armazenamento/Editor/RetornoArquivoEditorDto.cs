namespace SME.SGP.Infra
{
    public class RetornoArquivoEditorDto
    {
        public RetornoArquivoEditorDto() { }

        public bool Success { get; set; }

        public ArquivoEditorDto Data {get;set;}
    }
}
