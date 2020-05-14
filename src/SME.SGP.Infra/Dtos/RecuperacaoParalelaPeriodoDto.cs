using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaPeriodoDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public List<RecuperacaoParalelaAlunoDto> Alunos { get; set; }
        public DateTime? CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public long Id { get; set; }
    }
}