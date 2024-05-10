using System.ComponentModel.DataAnnotations;

namespace SME.SGP.TesteIntegracao.Setup
{
    public enum ScriptCarga
    {
        [Display(Name = "V710__CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE.sql")]
        CARGA_QUESTIONARIO_MAPEAMENTO_ESTUDANTE = 1,
        [Display(Name = "V713__ALTERAR_QUESTAO_MIGRANTE_MAPEAMENTO_ESTUDANTE.sql")]
        ALTERAR_QUESTAO_MIGRANTE_MAPEAMENTO_ESTUDANTE = 2
    }
}
