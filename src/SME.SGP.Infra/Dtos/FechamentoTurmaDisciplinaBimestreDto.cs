using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoTurmaDisciplinaBimestreDto
    {
        public List<NotaConceitoAlunoBimestreDto> Alunos { get; set; }
        public long FechamentoId { get; set; }
        public DateTime DataFechamento { get; set; }
        public int TotalAulasDadas { get; set; }
        public int TotalAulasPrevistas { get; set; }
        public int Bimestre { get; set; }
        public bool EhSintese { get; set; }
        public Periodo Periodo { get; set; }        
        public SituacaoFechamento Situacao { get; set; }
        public bool PodeProcessarReprocessar { get; set; }
        public string SituacaoNome { get; set; }
    }
}