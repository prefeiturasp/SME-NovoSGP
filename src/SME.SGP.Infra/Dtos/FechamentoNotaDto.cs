using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoNotaDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Necessário informar o codigo do aluno")]
        public string CodigoAluno { get; set; }
        [Required(ErrorMessage = "Necessário informar o componente curricular para atribuição da nota/conceito ao aluno")]
        [Range(1,double.MaxValue,ErrorMessage = "Necessário informar o componente curricular para atribuição da nota/conceito ao aluno")]
        public long DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public long? SinteseId { get; set; }
        public string Anotacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoRf { get; set; }
        public string CriadoPor { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoRf { get; set; }
        public string AlteradoPor { get; set; }
    }
}
