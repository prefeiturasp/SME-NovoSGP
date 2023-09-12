using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarConsolidacaoFrequenciaAlunoMensalCommand : IRequest<long>
    {
        public RegistrarConsolidacaoFrequenciaAlunoMensalCommand(long turmaId, string alunoCodigo, int mes,
            double percentual, int quantidadeAulas, int quantidadeAusencias, int quantidadeCompensacoes)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            Mes = mes;
            Percentual = percentual;
            QuantidadeAulas = quantidadeAulas;
            QuantidadeAusencias = quantidadeAusencias;
            QuantidadeCompensacoes = quantidadeCompensacoes;
        }

        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Mes { get; set; }
        public double Percentual { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
