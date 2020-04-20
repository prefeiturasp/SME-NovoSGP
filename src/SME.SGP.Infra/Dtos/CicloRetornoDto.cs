using System;

namespace SME.SGP.Infra
{
    public class CicloRetornoDto
    {
        public int Codigo { get; set; }
        public int CodigoEtapaEnsino { get; set; }
        public int CodigoModalidadeEnsino { get; set; }
        public string Descricao { get; set; }
        public DateTime DtAtualizacao { get; set; }
    }
}