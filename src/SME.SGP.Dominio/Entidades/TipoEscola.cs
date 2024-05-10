using System;

namespace SME.SGP.Dominio
{
    public class TipoEscolaEol : EntidadeBase
    {
        public int CodEol { get; set; }
        public string Descricao { get; set; }
        public DateTime DtAtualizacao { get; set; }
    }
}