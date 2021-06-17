using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoFrequenciaTurmaCommand : IRequest<long>
    {
        public RegistraConsolidacaoFrequenciaTurmaCommand(long turmaId, int quantidadeAcimaMinimoFrequencia, int quantidadeAbaixoMinimoFrequencia)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
        }

        public long TurmaId { get; }
        public int QuantidadeAcimaMinimoFrequencia { get; }
        public int QuantidadeAbaixoMinimoFrequencia { get; }
    }
}
