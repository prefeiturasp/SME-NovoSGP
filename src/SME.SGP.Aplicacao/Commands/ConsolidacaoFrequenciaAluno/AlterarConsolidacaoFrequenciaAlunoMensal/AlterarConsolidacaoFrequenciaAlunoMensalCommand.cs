using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AlterarConsolidacaoFrequenciaAlunoMensalCommand : IRequest<bool>
    {
        public AlterarConsolidacaoFrequenciaAlunoMensalCommand(long idConsolidacao,
            double percentual, int quantidadeAulas, int quantidadeAusencias, int quantidadeCompensacoes)
        {
            ConsolidacaoId = idConsolidacao;
            Percentual = percentual;
            QuantidadeAulas = quantidadeAulas;
            QuantidadeAusencias = quantidadeAusencias;
            QuantidadeCompensacoes = quantidadeCompensacoes;
        }

        public long ConsolidacaoId { get; set; }
        public double Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
