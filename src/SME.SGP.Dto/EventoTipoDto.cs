using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class EventoTipoDto
    {
        public long Codigo { get; set; }

        [Required(ErrorMessage = "A Descrição é obrigatoria")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Deve ser informando o local de ocorrencia")]
        public EventoLocalOcorrencia LocalOcorrencia { get; set; }

        [Required(ErrorMessage = "Deve ser informado o tipo de data")]
        public EventoTipoData TipoData { get; set; }

        [Required(ErrorMessage = "Deve ser informado se o feriado é letivo, opcional ou não letivo")]
        public EventoLetivo Letivo { get; set; }

        public bool Dependencia { get; set; }
        public bool Concomitancia { get; set; }
        public bool Ativo { get; set; }
    }
}
