using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum FuncaoAtividade
    {
        [Display(Name = "Coord. Pedagógico CIEJA")]
        COORDERNADOR_PEDAGOGICO_CIEJA = 44,

        [Display(Name = "Assistente Coord. Geral CIEJA")]
        ASSISTENTE_COORDERNADOR_GERAL_CIEJA = 43,

        [Display(Name = "Coord. Geral CIEJA")]
        COORDERNADOR_GERAL_CIEJA = 42,

        [Display(Name = "Assistente Pedagógico Polo Formação")]
        ASSISTENTE_PEDAGOGICO_POLO_FORMACAO = 1056,

        [Display(Name = "Coordenador Polo Formação")]
        COORDENADOR_POLO_FORMACAO = 39,

        [Display(Name = "Secretário Polo Formação")]
        SECRETARIO_POLO_FORMACAO = 35
    }
}
