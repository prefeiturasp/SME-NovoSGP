namespace SME.SGP.Infra
{
    public class RespostaEnderecoResidencialAtendimentoNAAPADto
    {
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string tipoLogradouro { get; set; }
        public string logradouro { get; set; }

        public bool EhIgual(object o)
        {
            if (!(o is RespostaEnderecoResidencialAtendimentoNAAPADto)) return false;

            RespostaEnderecoResidencialAtendimentoNAAPADto resposta = (RespostaEnderecoResidencialAtendimentoNAAPADto)o;
            return (this.numero == resposta.numero 
                && this.complemento == resposta.complemento
                && this.bairro == resposta.bairro
                && this.tipoLogradouro == resposta.tipoLogradouro
                && this.logradouro == resposta.logradouro
                );

        }
    }
}
