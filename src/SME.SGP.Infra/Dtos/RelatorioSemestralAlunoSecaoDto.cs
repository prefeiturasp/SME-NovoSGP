using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class RelatorioSemestralAlunoSecaoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Obrigatorio { get; set; }
        public string Valor { get; set; }
    }
}
