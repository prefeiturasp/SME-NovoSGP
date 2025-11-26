using System;

namespace SME.SGP.Infra
{
    public class FechamentoCicloSondagemDto
    {
        public long Id { get; set; }
        public int Cico { get; set; }

        [DataRequerida(ErrorMessage = "A data final é obrigatória.")]
        public DateTime? FinalDoFechamento { get; set; }

        [DataRequerida(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime? InicioDoFechamento { get; set; }

        public DateTime InicioMinimo { get; set; }
    }
}