using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunosFechamentoNotaConceitoTurmaDto
    {
        public AlunosFechamentoNotaConceitoTurmaDto()
        {
            NotasConceitoBimestre = new List<FechamentoConsultaNotaConceitoTurmaListaoDto>();
            NotasConceitoFinal = new List<FechamentoConsultaNotaConceitoTurmaListaoDto>();
            PodeEditar = true;
            PossuiAnotacao = false;
        }

        public string Nome { get; set; }
        public string CodigoAluno { get; set; }
        public int NumeroChamada { get; set; }
        public double PercentualFrequencia { get { return string.IsNullOrWhiteSpace(Frequencia) || !double.TryParse(Frequencia, out double valor) ? 0 : Convert.ToDouble(Frequencia ?? "0"); } }
        public string Frequencia { get; set; }
        public bool EhAtendidoAEE { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
        public bool PodeEditar { get; set; }
        public bool PossuiAnotacao { get; set; }
        public MarcadorFrequenciaDto Marcador { get; set; }
        public IList<FechamentoConsultaNotaConceitoTurmaListaoDto> NotasConceitoBimestre { get; set; }
        public IList<FechamentoConsultaNotaConceitoTurmaListaoDto> NotasConceitoFinal { get; set; }
    }
}
