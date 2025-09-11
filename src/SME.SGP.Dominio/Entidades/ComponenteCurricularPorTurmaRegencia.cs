using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ComponenteCurricularPorTurmaRegencia
    {
        public string CodigoTurma { get; set; }
        public long CodDisciplina { get; set; }
        public long? CodDisciplinaPai { get; set; }
        public string Disciplina { get; set; }
        public bool Regencia { get; set; }
        public bool Compartilhada { get; set; }
        public bool Frequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool LancaNota { get; set; }
        public bool BaseNacional { get; set; }
        public ComponenteCurricularGrupoMatriz GrupoMatriz { get; set; }
    }
}
