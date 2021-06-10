using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dto
{
    public class AbrangenciaTurmaRetornoEolDto
    {
        private string _ano { get; set; }
        public string Ano
        {
            get
            {
                switch (TipoTurma)
                {
                    case TipoTurma.EdFisica:
                        var anoDoNome = NomeTurma.Substring(0, 1);
                        if (int.TryParse(anoDoNome, out _))
                        {
                            return anoDoNome;
                        }
                        else return _ano;
                    case TipoTurma.Itinerarios2AAno:
                        return "2";
                    default:
                        return _ano;
                }
            }
            set
            { _ano = value; }
        }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public string CodigoModalidade { get; set; }
        public string NomeTurma { get; set; }
        public int Semestre { get; set; }
        public int DuracaoTurno { get; set; }
        public int TipoTurno { get; set; }
        public int EtapaEJA { get; set; }
        public DateTime? DataFim { get; set; }
        public bool EHistorico { get; set; }
        public bool EnsinoEspecial { get; set; }
        public string SerieEnsino { get; set; }
        public DateTime? DataInicioTurma { get; set; }
        public bool Extinta { get; set; }
        public TipoTurma TipoTurma { get; set; }
    }
}