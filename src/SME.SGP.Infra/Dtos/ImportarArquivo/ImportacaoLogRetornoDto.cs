namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ImportacaoLogRetornoDto
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public long Id { get; set; }

        public static ImportacaoLogRetornoDto RetornarSucesso(string mensagem, long id)
        {
            return new ImportacaoLogRetornoDto
            {
                Sucesso = true,
                Mensagem = mensagem,
                Id = id
            };
        }

        public static ImportacaoLogRetornoDto RetornarSucesso(string mensagem)
        {
            return new ImportacaoLogRetornoDto
            {
                Sucesso = true,
                Mensagem = mensagem
            };
        }
    }
}
