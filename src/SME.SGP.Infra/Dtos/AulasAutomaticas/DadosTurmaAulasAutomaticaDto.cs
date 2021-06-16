using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DadosTurmaAulasAutomaticaDto
    {
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public string ComponenteCurricularDescricao { get; set; }
        public DateTime DataInicioTurma { get; set; }
    }
}
