using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaNotasTodosComponentesCurricularesAlunoQuery : IRequest<bool>
    {
        public VerificaNotasTodosComponentesCurricularesAlunoQuery(string alunoCodigo, Turma turma, long? periodoEscolarId = 0, bool? historico = false)
        {
            AlunoCodigo = alunoCodigo;
            TurmaAluno = turma;
            PeriodoEscolarId = periodoEscolarId?? 0;
            Historico = historico?? false;


        }

        public string AlunoCodigo { get; set; }
        public Turma TurmaAluno { get; set; }
        public long PeriodoEscolarId { get; set; }
        public bool Historico { get; set; }
    }

    public class VerificaNotasTodosComponentesCurricularesAlunoQueryValidator : AbstractValidator<VerificaNotasTodosComponentesCurricularesAlunoQuery>
    {
        public VerificaNotasTodosComponentesCurricularesAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("Necessário informar o Código do Aluno");
            RuleFor(a => a.TurmaAluno)
                .NotEmpty()
                .WithMessage("Necessário informar a Turma do Aluno");
        }
    }
}
