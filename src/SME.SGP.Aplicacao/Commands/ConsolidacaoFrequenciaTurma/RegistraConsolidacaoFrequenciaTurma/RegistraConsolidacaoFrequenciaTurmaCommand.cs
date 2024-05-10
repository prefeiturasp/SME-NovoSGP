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
                                                          (DateTime? DataInicio, DateTime? DataFim) periodo,
                                                          int totalAulas, 
                                                          int totalFrequencias)
        {
            TurmaId = turmaId;
            QuantidadeAcimaMinimoFrequencia = quantidadeAcimaMinimoFrequencia;
            QuantidadeAbaixoMinimoFrequencia = quantidadeAbaixoMinimoFrequencia;
            TipoConsolidacao = tipoConsolidacao;
            PeriodoInicio = periodo.DataInicio;
            PeriodoFim = periodo.DataFim;
            TotalAulas = totalAulas;
            TotalFrequencias = totalFrequencias;
        }

        public TipoConsolidadoFrequencia TipoConsolidacao { get; set; }
        public long TurmaId { get; }
        public int QuantidadeAcimaMinimoFrequencia { get; }
        public int QuantidadeAbaixoMinimoFrequencia { get; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
        public int TotalAulas { get; set; }
        public int TotalFrequencias { get; set; }
    }
}
