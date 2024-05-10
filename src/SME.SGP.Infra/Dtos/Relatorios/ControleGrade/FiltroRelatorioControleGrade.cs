using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioControleGrade
    {
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }

        public IEnumerable<long> Turmas { get; set; }
        public IEnumerable<long> ComponentesCurriculares { get; set; }
        public IEnumerable<int> Bimestres { get; set; }
        public ModeloRelatorio Modelo { get; set; }

        public int AnoLetivo { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int Semestre { get; set; }
    }
}
