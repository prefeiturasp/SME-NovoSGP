using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoInformacoesEducacionais
    {
        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Ue { get; set; }
        public int IdepAnosIniciais { get; set; }
        public int IdepAnosFinais { get; set; }
        public int IdebAnosIniciais { get; set; }
        public int IdebAnosFinais { get; set; }
        public int IdebEnsinoMedio { get; set; }
        public decimal PercentualFrequenciaGlobal { get; set; }
        public int QuantidadeAlunosPap { get; set; }
        public int QuantidadeTurmasPap { get; set; }
        public decimal PercentualFrequenciaAlunosPap { get; set; }
        public int QuantidadeAlunosDesistentesAbandono { get; set; }
        public int QuantidadePromocoes { get; set; }
        public int QuantidadeRetencoesFrequencia { get; set; }
        public int QuantidadeRetencoesNota { get; set; }
        public int QuantidadeNotasAbaixoMedia { get; set; }
        public int QuantidadeNotasAcimaMedia { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTimeExtension.HorarioBrasilia();
    }
}