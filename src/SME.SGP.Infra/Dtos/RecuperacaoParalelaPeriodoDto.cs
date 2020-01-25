using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RecuperacaoParalelaPeriodoDto
    {
        public List<RecuperacaoParalelaAlunoDto> Alunos { get; set; }
        public string Descricao { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
    }
}