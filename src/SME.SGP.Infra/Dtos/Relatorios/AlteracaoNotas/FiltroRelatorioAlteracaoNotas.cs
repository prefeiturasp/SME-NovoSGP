using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAlteracaoNotas
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }        
        public long AnoLetivo { get; set; }
        public Modalidade ModalidadeTurma { get; set; }
        public int Semestre { get; set; }
        public List<long> Turma { get; set; }
        public IEnumerable<long> ComponentesCurriculares { get; set; }
        public List<int> Bimestres { get; set; }
        public TipoAlteracaoNota TipoAlteracaoNota { get; set; }
        public string NomeUsuario { get; set; }

    }
}
