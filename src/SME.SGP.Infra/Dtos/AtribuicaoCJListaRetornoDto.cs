using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJListaRetornoDto
    {
        public AtribuicaoCJListaRetornoDto()
        {
            Disciplinas = new List<AtribuicaoCJDisciplinaRetornoDto>();
        }

        public List<AtribuicaoCJDisciplinaRetornoDto> Disciplinas { get; set; }
        public string Modalidade { get; set; }
        public string Turma { get; set; }
    }
}