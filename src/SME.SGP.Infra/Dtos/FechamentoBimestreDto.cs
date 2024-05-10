using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoBimestreDto
    {
        public int Bimestre { get; set; }

        [DataRequerida(ErrorMessage = "A data final é obrigatória.")]
        public DateTime? FinalDoFechamento { get; set; }

        public DateTime FinalMaximo { get; set; }
        public long Id { get; set; }

        [DataRequerida(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime? InicioDoFechamento { get; set; }

        public DateTime InicioMinimo { get; set; }

        [Required(ErrorMessage = "O período escolar é obrigatório")]
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long PeriodoEscolarId { get; set; }
    }
}