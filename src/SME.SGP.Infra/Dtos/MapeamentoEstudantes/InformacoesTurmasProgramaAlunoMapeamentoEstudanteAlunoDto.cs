using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto
    {
        public InformacoesTurmasProgramaAlunoMapeamentoEstudanteAlunoDto()
        {
            ComponentesSRMCEFAI = new();
            ComponentesPAP = new();
            ComponentesMaisEducacao = new();
            ComponentesFortalecimentoAprendizagens = new();
        }
        
        public List<ComponenteCurricularSimplificadoDto> ComponentesSRMCEFAI { get; set; }
        public List<ComponenteCurricularSimplificadoDto> ComponentesPAP { get; set; }
        public List<ComponenteCurricularSimplificadoDto> ComponentesMaisEducacao { get; set; }
        public List<ComponenteCurricularSimplificadoDto> ComponentesFortalecimentoAprendizagens { get; set; }
    }
}
