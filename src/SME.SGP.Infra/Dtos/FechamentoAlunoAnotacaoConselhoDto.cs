using System;

namespace SME.SGP.Infra
{
    public class FechamentoAlunoAnotacaoConselhoDto
    {
        public string Anotacao { get; set; }
        public DateTime Data { get; set; }
        public string Disciplina { get; set; }
        public string DisciplinaId { get; set; }
        public string Professor { get; set; }
        public string ProfessorRf { get; set; }
    }
}