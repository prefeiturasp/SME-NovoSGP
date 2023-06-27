using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class SolicitacaoGuidAutenticacaoFrequenciaDto
    {
        [Required(ErrorMessage = "É necessário informar o código Rf do usuário.")]
        public string Rf { get; set; }

        [Required(ErrorMessage = "É necessário informar o código da turma.")]
        public string TurmaCodigo { get; set; }

        [Required(ErrorMessage = "É necessário informar o código do componente curricular.")]
        public string ComponenteCurricularCodigo { get; set; }       
    }
}
