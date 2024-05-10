using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoEsporadicaDto
    {
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Deve ser informado o ano letivo vigente")]
        public int AnoLetivo { get; set; }

        [DataRequerida(ErrorMessage = "É necessario informar a data fim da atribuição")]
        public DateTime DataFim { get; set; }

        [DataRequerida(ErrorMessage = "É necessario informar a data inicio da atribuição")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "É necessario informar a DRE")]
        public string DreId { get; set; }

        public bool EhInfantil { get; set; }
        public bool Excluido { get; set; }

        public long Id { get; set; }

        public bool Migrado { get; set; }

        public string ProfessorNome { get; set; }

        [Required(ErrorMessage = "É necessario informar o RF do professor Atribuido")]
        public string ProfessorRf { get; set; }

        [Required(ErrorMessage = "É necessario informar a UE")]
        public string UeId { get; set; }
    }
}