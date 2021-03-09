using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaPeriodoListagemDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public List<RecuperacaoParalelaAlunoListagemDto> Alunos { get; set; }
        public DateTime? CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool EhAtendidoAEE { get; set; }
    }
}