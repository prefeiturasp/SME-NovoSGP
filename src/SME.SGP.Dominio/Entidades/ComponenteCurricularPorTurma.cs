using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComponenteCurricularPorTurma
    {
        public string CodigoAluno { get; set; }
        public string CodigoTurma { get; set; }
        public string CodigoTurmaAssociada { get; set; }
        public long CodDisciplina { get; set; }
        public long? CodDisciplinaPai { get; set; }
        public string Disciplina { get; set; }
        public bool Regencia { get; set; }
        public string TipoEscola { get; set; }
        public bool Compartilhada { get; set; }
        public bool Frequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool LancaNota { get; set; }
        public bool BaseNacional { get; set; }
        public ComponenteCurricularGrupoMatriz GrupoMatriz { get; set; }
        public AreaDoConhecimento AreaDoConhecimento { get; set; }
        public IEnumerable<ComponenteCurricularPorTurmaRegencia> ComponentesCurricularesRegencia { get; set; }
    }
}
