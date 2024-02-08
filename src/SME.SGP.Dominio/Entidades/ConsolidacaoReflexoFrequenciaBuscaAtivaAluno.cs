﻿namespace SME.SGP.Dominio
{
    public class ConsolidacaoReflexoFrequenciaBuscaAtivaAluno : EntidadeBase
    {
        public string TurmaCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string DreCodigo { get; set; }
        public string AnoTurma { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public string AlunoCodigo  { get; set; }
        public string AlunoNome { get; set; }
        public string DataBuscaAtiva { get; set; }
        public int Mes { get; set; }
        public double PercFrequenciaAntesAcao { get; set; }
        public double PercFrequenciaAposAcao { get; set; }
    }
}
