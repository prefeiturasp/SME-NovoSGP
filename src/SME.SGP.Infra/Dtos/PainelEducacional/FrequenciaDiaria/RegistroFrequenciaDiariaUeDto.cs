using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria
{
    public class RegistroFrequenciaDiariaUeDto
    {
        public DateTime Data { get; set; }
        public string Ue { get; set; }
        public long QuantidadeAlunos { get; set; }
        public long QuantidadeAlunosPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public string NivelFrequencia { get; set; }
    }
}
