using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
        public enum TipoArquivoImportacao
        {
                [Display(Name = "IDEB")]
                IDEB = 1,

                [Display(Name = "IDEP")]
                IDEP = 2,

                [Display(Name = "Fluência Leitora")]
                FLUENCIA_LEITORA = 3,

        [Display(Name = "Proficiencia IDEP")]
        PROFICIENCIA_IDEP = 4

        [Display(Name = "Boletim IDEP")]
        BOLETIM_IDEP = 4,

                [Display(Name = "Proficiencia IDEP")]
                PROFICIENCIA_IDEP = 4,

                [Display(Name = "Alfabetizacao")]
                ALFABETIZACAO = 5,
        }
}
