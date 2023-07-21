using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoFrequenciaTurmaCommand : IRequest<long>
    {
        public RegistraConsolidacaoFrequenciaTurmaCommand(long turmaId, int quantidadeAcimaMinimoFrequencia, int quantidadeAbaixoMinimoFrequencia, TipoConsolidadoFrequencia tipoConsolidado)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
            TipoConsolidado = tipoConsolidado;
        }

        public TipoConsolidadoFrequencia TipoConsolidado { get; set; }
        public long TurmaId { get; }
        public int QuantidadeAcimaMinimoFrequencia { get; }
        public int QuantidadeAbaixoMinimoFrequencia { get; }
    }
}
