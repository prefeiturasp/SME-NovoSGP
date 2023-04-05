using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroFaltasNaoCompensadaDto
    {
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
        public string Descricao { get; set; }
        public long RegistroFrequenciaAlunoId { get; set; }
        public bool Sugestao { get; set; }
    }
}