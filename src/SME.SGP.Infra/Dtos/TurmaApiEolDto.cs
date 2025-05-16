using System;

namespace SME.SGP.Infra.Dtos
{
    public class TurmaApiEolDto
    {
        public char Ano { get; set; }
        public int AnoLetivo { get; set; }
        public int Codigo { get; set; }
        public int TipoTurma { get; set; }
        public string Modalidade { get; set; }
        public int CodigoModalidade { get; set; }
        public string NomeTurma { get; set; }
        public int Semestre { get; set; }
        public int DuracaoTurno { get; set; }
        public int TipoTurno { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Ehistorico { get; set; }
        public bool EnsinoEspecial { get; set; }
        public int EtapaEJA { get; set; }
        public string SerieEnsino { get; set; }
        public DateTime? DataInicioTurma { get; set; }
        public bool Extinta { get; set; }
        public string Situacao { get; set; }
        public string UeCodigo { get; set; }
    }
}
