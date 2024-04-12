using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroColetivoDto
    {
        public long? Id { get; set; }
        public long DreId { get; set; }
        public IEnumerable<long> UeIds { get; set; }
        public long TipoReuniaoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public int QuantidadeParticipantes { get; set; }
        public int QuantidadeEducadores { get; set; }
        public int QuantidadeEducandos { get; set; }
        public int QuantidadeCuidadores { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public IEnumerable<AnexoDto> Anexos { get; set; }
    }
}
