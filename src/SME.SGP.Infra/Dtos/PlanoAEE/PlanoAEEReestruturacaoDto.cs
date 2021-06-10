using System;

namespace SME.SGP.Infra
{
    public class PlanoAEEReestruturacaoDto
    {
        public long Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataVersao { get; set; }
        public int Semestre { get; set; }
        public long VersaoId { get; set; }
        public string Versao { get; set; }
        public string Descricao { get; set; }
        public string DescricaoSimples { get; set; }
    }
}
