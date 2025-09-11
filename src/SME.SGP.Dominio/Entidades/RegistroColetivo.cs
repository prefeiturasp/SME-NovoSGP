using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class RegistroColetivo : EntidadeBase
    {
        public long DreId { get; set; }
        public IEnumerable<Ue> Ues { get; set; }
        public long TipoReuniaoId { get; set; }
        public TipoReuniaoNAAPA TipoReuniao { get; set; }
        public DateTime DataRegistro { get; set; }
        public int QuantidadeParticipantes { get; set; }
        public int QuantidadeEducadores { get; set; }
        public int QuantidadeEducandos { get; set; }
        public int QuantidadeCuidadores { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public bool Excluido { get; set; }
        public IEnumerable<Arquivo> Anexos { get; set; }
    }
}
