using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FechamentoPorTurmaPeriodoCCDto
    {
        public FechamentoPorTurmaPeriodoCCDto()
        {
            FechamentoAlunos = new List<FechamentoAlunoPorTurmaPeriodoCCDto>();
        }

        public long Id { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public List<FechamentoAlunoPorTurmaPeriodoCCDto> FechamentoAlunos { get; }
        public DateTime CriadoEm { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
    }
}