using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria
{
    public class RegistroFrequenciaDiariaTurmaDto
    {
        public DateTime Data { get; set; }
        public string Turma { get; set; }
        public long QuantidadeEstudantes { get; set; }
        public long EstudantesPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public string NivelFrequencia { get; set; }
    }
}
