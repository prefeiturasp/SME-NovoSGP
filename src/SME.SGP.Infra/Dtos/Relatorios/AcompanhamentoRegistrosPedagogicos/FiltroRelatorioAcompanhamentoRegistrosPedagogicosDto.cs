using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioAcompanhamentoRegistrosPedagogicosDto
    {
        public FiltroRelatorioAcompanhamentoRegistrosPedagogicosDto()
        {
            Turmas = new List<long>();
            Bimestres = new List<int>();
        }
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
        public List<long> Turmas { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public List<int> Bimestres { get; set; }
        public string ProfessorCodigo { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
    }
}
