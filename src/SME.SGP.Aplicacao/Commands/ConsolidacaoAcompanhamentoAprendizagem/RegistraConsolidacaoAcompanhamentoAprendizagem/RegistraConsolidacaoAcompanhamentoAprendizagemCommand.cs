using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoAcompanhamentoAprendizagemCommand : IRequest<long>
    {
        public RegistraConsolidacaoAcompanhamentoAprendizagemCommand(long turmaId, int quantidadeComAcompanhamento, int quantidadeSemAcompanhamento, int semestre)
        {
            TurmaId = turmaId;
            QuantidadeComAcompanhamento = quantidadeComAcompanhamento;
            QuantidadeSemAcompanhamento = quantidadeSemAcompanhamento;
            Semestre = semestre;
        }

        public long TurmaId { get; }
        public int QuantidadeComAcompanhamento { get; }
        public int QuantidadeSemAcompanhamento { get; }
        public int Semestre { get; }
    }
}
