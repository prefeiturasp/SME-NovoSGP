using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentosAEEResumoDto
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }
        public string Situacao { get; set; }
    }
}
