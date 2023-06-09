using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class InconsistenciasAlunoFamiliaDto
    {
        public InconsistenciasAlunoFamiliaDto()
        {
            Inconsistencias = new List<string>();
        }
        public int? NumeroChamada { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }
        public List<string> Inconsistencias { get; set; }
    }
}