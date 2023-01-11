using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dto
{
    public class EnderecoRespostaDto
    {
        public string Nro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int CEP { get; set; }
        public string NomeMunicipio { get; set; }
        public string SiglaUF { get; set; }
        public string Tipologradouro { get; set; }
        public string Logradouro { get; set; }
    }
}
