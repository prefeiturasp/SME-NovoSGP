using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class ConselhoClasseNotaDto
    {
        public long? Conceito { get; set; }
        public double? Nota { get; set; }
        [Required(ErrorMessage = "A justificativa é obrigatória")]
        public string Justificativa { get; set; }
        [Required(ErrorMessage = "O código do componente curricular é obrigatório")]
        public long CodigoComponenteCurricular { get; set; }
    }
}
