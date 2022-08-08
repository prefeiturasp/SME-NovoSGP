using System;
using System.Collections.Generic;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class CacheFechamentoTurmaDisciplinaDto
    {
        public CacheFechamentoTurmaDisciplinaDto()
        {
            FechamentoAlunos = new List<CacheFechamentoAlunoDto>();
        }

        public long Id { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public List<CacheFechamentoAlunoDto> FechamentoAlunos { get; }
        public DateTime CriadoEm { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
    }
}