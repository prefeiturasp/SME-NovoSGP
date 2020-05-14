using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class EventoTipoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public bool Ativo { get; set; }

        public bool Concomitancia { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public bool Dependencia { get; set; }
        public bool PossuiEventos { get; set; }
        [Required(ErrorMessage = "A Descrição é obrigatória")]
        public string Descricao { get; set; }

        public long Id { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informado se o feriado é letivo, opcional ou não letivo")]
        public EventoLetivo Letivo { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informando o local de ocorrencia")]
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }

        [EnumeradoRequirido(ErrorMessage = "Deve ser informado o tipo de data")]
        public EventoTipoData TipoData { get; set; }
        public bool SomenteLeitura { get; set; }
    }
}