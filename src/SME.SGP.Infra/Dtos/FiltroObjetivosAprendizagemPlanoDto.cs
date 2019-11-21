using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroObjetivosAprendizagemPlanoDto
    {
        public int Ano { get; set; }
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
        public long ComponenteId { get; set; }
        public long DisciplinaId { get; set; }
    }
}
