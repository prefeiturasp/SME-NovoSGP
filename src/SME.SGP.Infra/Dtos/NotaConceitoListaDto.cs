using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoListaDto
    {
        [Required(ErrorMessage = "É obrigatorio informar pelo menos uma nota/conceito")]
        [MinLength(1, ErrorMessage = "É obrigatorio informar pelo menos uma nota/conceito")]
        public IEnumerable<NotaConceitoDto> NotasConceitos { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar o Id da turma")]
        public string TurmaId { get; set; }

        [Required(ErrorMessage = "È obrigatorio informar a disciplina da atividade avaliativa")]
        public string  DisciplinaId { get; set; }
    }
}