using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoFinalConsultaRetornoAlunoDto
    {
        public FechamentoFinalConsultaRetornoAlunoDto()
        {
            NotasConceitoBimestre = new List<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto>();
            NotasConceitoFinal = new List<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto>();
            PodeEditar = true;
        }

        public string Codigo { get; set; }
        public double FrequenciaValor{ get; set; }
        public string Frequencia { get; set; }
        public string Informacao { get; set; }
        public string Nome { get; set; }
        public IList<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto> NotasConceitoBimestre { get; set; }
        public IList<FechamentoFinalConsultaRetornoAlunoNotaConceitoDto> NotasConceitoFinal { get; set; }
        public int NumeroChamada { get; set; }
        public bool PodeEditar { get; set; }
        public string Sintese { get; set; }
        public double TotalAusenciasCompensadas { get; set; }
        public int TotalFaltas { get; set; }
        public bool EhAtendidoAEE { get; set; }
    }
}