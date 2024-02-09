using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EtapaEnsino
    {
        [Display(Name = "TECNICO MEDIO")]
        TenicoMedio = 14,
        [Display(Name = "QUALIFICACAO PROFISSIONAL")]
        QualificacaoProfissional = 18
    }
}
