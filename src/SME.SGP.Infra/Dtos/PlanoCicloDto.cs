using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PlanoCicloDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O ciclo deve ser informado")]
        public long CicloId { get; set; }

        [Required(ErrorMessage = "A descrição deve ser informada.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        public long Id { get; set; }

        public List<long> IdsMatrizesSaber { get; set; }

        public List<long> IdsObjetivosDesenvolvimento { get; set; }
    }
}