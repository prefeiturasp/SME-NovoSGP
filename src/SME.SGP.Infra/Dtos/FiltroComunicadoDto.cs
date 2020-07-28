using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroComunicadoDto
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public int[] GruposId { get; set; }
        public string Titulo { get; set; }
        [Range(1,int.MaxValue, ErrorMessage = "O Ano letivo é Obrigatório")]
        public int AnoLetivo { get; set; }
        [Required(ErrorMessage = "O Codigo da Dre é Obrigatório")]
        public string CodigoDre { get; set; }
        [Required(ErrorMessage = "O Codigo da Ue é Obrigatório")]
        public string CodigoUe { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public string[] Turmas { get; set; }
    }
}