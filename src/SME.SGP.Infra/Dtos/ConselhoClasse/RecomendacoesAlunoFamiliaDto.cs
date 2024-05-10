using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class RecomendacoesAlunoFamiliaDto
    {
        public long Id { get; set; }
        public string Recomendacao { get; set; }
        public int Tipo { get; set; }
    }
}
