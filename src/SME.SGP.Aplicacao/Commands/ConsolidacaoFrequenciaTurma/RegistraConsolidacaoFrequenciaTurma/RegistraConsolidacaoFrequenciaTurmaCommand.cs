using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class RegistraConsolidacaoFrequenciaTurmaCommand : IRequest<long>
    {
        public RegistraConsolidacaoFrequenciaTurmaCommand(
                                                          long turmaId, 
                                                          int quantidadeAcimaMinimoFrequencia, 
                                                          int quantidadeAbaixoMinimoFrequencia, 
                                                          TipoConsolidadoFrequencia tipoConsolidacao,
                                                          DateTime? periodoInicio,
                                                          DateTime? periodoFim)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
            TipoConsolidacao = tipoConsolidacao;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public TipoConsolidadoFrequencia TipoConsolidacao { get; set; }
        public long TurmaId { get; }
        public int QuantidadeAcimaMinimoFrequencia { get; }
        public int QuantidadeAbaixoMinimoFrequencia { get; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
    }
}
