using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class PlanoAnualDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int? Ano { get; set; }

        [Required(ErrorMessage = "O bimestre deve ser informado")]
        public int? Bimestre { get; set; }

        [Required(ErrorMessage = "A descrição deve ser informada.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public long? EscolaId { get; set; }

        public long Id { get; set; }

        public List<long> IdsDisciplinas { get; set; }
        public List<long> IdsObjetivosAprendizagem { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public long? TurmaId { get; set; }
    }
}