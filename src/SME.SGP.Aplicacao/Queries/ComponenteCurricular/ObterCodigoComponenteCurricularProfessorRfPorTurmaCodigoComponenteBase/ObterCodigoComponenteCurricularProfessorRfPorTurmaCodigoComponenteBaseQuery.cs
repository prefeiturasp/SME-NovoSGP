using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQuery : IRequest<(long codigo, string rf)>
    {
        public ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQuery(Turma turma, long componenteCurricularIdBase)
        {
            Turma = turma;
            ComponenteCurricularIdBase = componenteCurricularIdBase;
        }

        public Turma Turma { get; set; }

        public long ComponenteCurricularIdBase { get; set; }
    }

    public class ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQueryValidador : AbstractValidator<ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQuery>
    {
        public ObterCodigoComponenteCurricularProfessorRfPorTurmaCodigoComponenteBaseQueryValidador()
        {
            RuleFor(t => t.Turma)
                .NotNull()
                .WithMessage("A turma deve ser informada.");

            RuleFor(t => t.ComponenteCurricularIdBase)
                .GreaterThan(0)
                .WithMessage("O código do componente base deve ser informado.");
        }
    }
}
