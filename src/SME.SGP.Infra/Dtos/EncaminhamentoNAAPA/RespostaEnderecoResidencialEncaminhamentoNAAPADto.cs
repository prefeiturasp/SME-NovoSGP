using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RespostaEnderecoResidencialEncaminhamentoNAAPADto
    {
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string tipoLogradouro { get; set; }
        public string logradouro { get; set; }

        public override bool Equals(object o)
        {
            if (!(o is RespostaEnderecoResidencialEncaminhamentoNAAPADto)) return false;

            RespostaEnderecoResidencialEncaminhamentoNAAPADto resposta = (RespostaEnderecoResidencialEncaminhamentoNAAPADto)o;
            return (this.numero == resposta.numero 
                && this.complemento == resposta.complemento
                && this.bairro == resposta.bairro
                && this.tipoLogradouro == resposta.tipoLogradouro
                && this.logradouro == resposta.logradouro
                );

        }
    }
}
