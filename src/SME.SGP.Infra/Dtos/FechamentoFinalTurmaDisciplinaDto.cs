﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class FechamentoFinalTurmaDisciplinaDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "A turma é obrigatória!")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "O bimestre é obrigatório")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "O componente curricular é obrigatória!")]
        public long DisciplinaId { get; set; }

        public string Justificativa { get; set; }

        public bool EhRegencia { get; set; }

        public bool EhFinal { get; set; }
        public bool ComponenteSemNota { get; set; }

        [ListaTemElementos(ErrorMessage = "Necessário informar a lista de alunos e notas/conceitos para o fechamento")]
        public IEnumerable<FechamentoNotaDto> NotaConceitoAlunos { get; set; }
    }
}
