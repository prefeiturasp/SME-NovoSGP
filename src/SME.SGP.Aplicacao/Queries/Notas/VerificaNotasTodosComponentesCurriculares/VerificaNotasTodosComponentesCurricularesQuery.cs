using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Queries
{
    public class VerificaNotasTodosComponentesCurricularesQuery : IRequest<bool>
    {
        public VerificaNotasTodosComponentesCurricularesQuery(string alunoCodigo, Turma turma, long? periodoEscolarId, bool? historico = false)
        {
            AlunoCodigo = alunoCodigo;
            Turma = turma;
            PeriodoEscolarId = periodoEscolarId;
            Historico = historico;
        }
        public Turma Turma { get; set; }
        public string AlunoCodigo { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public bool? Historico { get; set; }
    }

    public class VerificaNotasTodosComponentesCurricularesQueryValidator : AbstractValidator<VerificaNotasTodosComponentesCurricularesQuery>
    {
        public VerificaNotasTodosComponentesCurricularesQueryValidator()
        {
            RuleFor(x => x.Turma)
                .NotNull().WithMessage("Informe a turma para verificar notas de todos os componentes curriculares");
            
            RuleFor(x => x.AlunoCodigo)
                .NotEmpty().WithMessage("Informe o código do aluno para verificar notas de todos os componentes curriculares");
        }
    }
}