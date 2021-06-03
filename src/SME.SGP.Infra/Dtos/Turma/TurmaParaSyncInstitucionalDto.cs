using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class TurmaParaSyncInstitucionalDto
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public int Codigo { get; set; }
        public int TipoTurma { get; set; }        
        public Modalidade CodigoModalidade { get; set; }
        public string NomeTurma { get; set; }
        public int Semestre { get; set; }
        public int DuracaoTurno { get; set; }
        public int TipoTurno { get; set; }
        public DateTime? DataFim { get; set; }
        public bool EnsinoEspecial { get; set; }        
        public string SerieEnsino { get; set; }
        public DateTime? DataInicioTurma { get; set; }
        public bool Extinta { get; set; }
        public string Situacao { get; set; }
        public string UeCodigo { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public DateTime DataStatusTurmaEscola { get; set; }
        public string NomeFiltro { get; set; }
        public int EtapaEJA
        {
            get
            {
                if (CodigoModalidade == Dominio.Modalidade.EJA && !string.IsNullOrEmpty(SerieEnsino) && SerieEnsino.Length > 2)
                {
                    var etapa = SerieEnsino.Substring(SerieEnsino.Length - 2).Trim();

                    var indexPrimeiroCiclo = SerieEnsino.IndexOf(" I ");
                    var indexSegundoCiclo = SerieEnsino.IndexOf(" II");

                    if ((etapa == "I" && indexSegundoCiclo < 0) || (indexPrimeiroCiclo >= 0 && indexSegundoCiclo < 0))
                        return 1;
                    else if ((etapa == "II" && indexPrimeiroCiclo < 0) || (indexPrimeiroCiclo < 0 && indexSegundoCiclo >= 0))
                        return 2;
                }
                return 0;

            }
        }
    }
}
