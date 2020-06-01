using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoNotasConceitosRetornoDto
    {
        public IEnumerable<ConselhoClasseAlunoNotasConceitosDto> NotasConceitos { get; set; }
        
        public bool PodeEditarNota { get; set; }


    }
}