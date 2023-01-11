﻿using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConselhoClasseAlunoNotasConceitosRetornoDto
    {
        public IEnumerable<ConselhoClasseAlunoNotasConceitosDto> NotasConceitos { get; set; }
        
        public bool PodeEditarNota { get; set; }

        public bool TemConselhoClasseAluno { get; set; }
        public NotaParametroDto DadosArredondamento { get; set; }
    }
}