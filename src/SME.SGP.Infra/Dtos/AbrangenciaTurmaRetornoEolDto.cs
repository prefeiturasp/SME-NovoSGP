using System;

namespace SME.SGP.Dto
{
    public class AbrangenciaTurmaRetornoEolDto
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public string CodigoModalidade { get; set; }
        public string NomeTurma { get; set; }
        public int Semestre { get; set; }
        public int DuracaoTurno { get; set; }
        public int TipoTurno { get; set; }
        public DateTime? DataFIm { get; set; }
        public bool EHistorico { get; set; }
        public bool EnsinoEspecial { get; set; }
    }
}