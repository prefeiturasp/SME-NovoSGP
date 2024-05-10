
using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class DreUeNomeSituacaoTipoEscolaDataABAEDto
    {
        public long Id { get; set; }
        public string Dre { get; set; }
        public string Ue { get; set; }
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public bool Situacao { get; set; }
        public DateTime Data { get; set; }
    }
}
