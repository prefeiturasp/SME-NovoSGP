using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CacheFechamentoAlunoDto
    {
        public CacheFechamentoAlunoDto()
        {
            FechamentoNotas = new List<CacheFechamentoNotaDto>();
        }

        public string AlunoCodigo { get; set; }
        public List<CacheFechamentoNotaDto> FechamentoNotas { get; }        
    }
}