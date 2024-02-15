using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroColetivoCompletoDto : AuditoriaDto
    {
        public long DreId { get; set; }
        public string CodigoDre { get; set; }
        public string NomeDre { get; set; }
        public long TipoReuniaoId { get; set; }
        public string TipoReuniaoDescricao { get; set; }
        public DateTime DataRegistro { get; set; }
        public int QuantidadeParticipantes { get; set; }
        public int QuantidadeEducadores { get; set; }
        public int QuantidadeEducandos { get; set; }
        public int QuantidadeCuidadores { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public IEnumerable<UeRegistroColetivoDto> Ues { get; set; }      
        public IEnumerable<ArquivoAnexoRegistroColetivoDto> Anexos { get; set; }
    }
}
