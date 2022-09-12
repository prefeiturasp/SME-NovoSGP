using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaNotasTodosComponentesCurricularesAlunoQuery : IRequest<bool>
    {
        public VerificaNotasTodosComponentesCurricularesAlunoQuery(string alunoCodigo, Turma turma, int? bimestre = null, bool? historico = false)
        {
            AlunoCodigo = alunoCodigo;
            TurmaAluno = turma;
            Bimestre = bimestre;
            Historico = historico;


        }

        public string AlunoCodigo { get; set; }
        public Turma TurmaAluno { get; set; }
        public int? Bimestre { get; set; }
        public bool? Historico { get; set; }
    }

    public class VerificaNotasTodosComponentesCurricularesAlunoQueryValidator : AbstractValidator<VerificaNotasTodosComponentesCurricularesAlunoQuery>
    {
        public VerificaNotasTodosComponentesCurricularesAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno para verificação das notas de todos os compopnentes curriculares da Turma no Bimestre");
            RuleFor(a => a.TurmaAluno)
                .NotEmpty()
                .WithMessage("Necessário informar a Turma do Aluno para verificação das notas de todos os compopnentes curriculares do Aluno no Bimestre");
        }
    }
}
