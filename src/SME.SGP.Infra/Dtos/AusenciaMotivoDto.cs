using System;

namespace SME.SGP.Infra
{
    public class AusenciaMotivoDto
    {
        public DateTime DataAusencia { get; set; }
        public string RegistradoPor { get; set; }
        public string MotivoAusencia { get; set; }
        public string JustificativaAusencia { get; set; }
    }
}