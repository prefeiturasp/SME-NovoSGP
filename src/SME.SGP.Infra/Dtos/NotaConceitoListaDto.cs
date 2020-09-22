using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoListaDto
    {
        [Required(ErrorMessage = "É obrigatório informar a disciplina da atividade avaliativa")]
        public long DisciplinaId { get; set; }

        [Required(ErrorMessage = "É obrigatório informar pelo menos uma nota/conceito")]
        [MinLength(1, ErrorMessage = "É obrigatório informar pelo menos uma nota/conceito")]
        public IEnumerable<NotaConceitoDto> NotasConceitos { get; set; }

        [Required(ErrorMessage = "É obrigatório informar o Id da turma")]
        public string TurmaId { get; set; }
    }
}