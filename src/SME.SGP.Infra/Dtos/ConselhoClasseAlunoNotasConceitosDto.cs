using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoNotasConceitosDto
    {
        public ConselhoClasseAlunoNotasConceitosDto()
        {
            ComponentesCurriculares = new List<ConselhoClasseComponenteFrequenciaDto>();
            DesabilitarCampos = true;
        }

        public ConselhoClasseComponenteRegenciaFrequenciaDto ComponenteRegencia { get; set; }
        public List<ConselhoClasseComponenteFrequenciaDto> ComponentesCurriculares { get; set; }
        public bool DesabilitarCampos { get; set; }
        public string GrupoMatriz { get; set; }
    }
}