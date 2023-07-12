using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPQuery : IRequest<SecaoTurmaAlunoPAPDto>
    {
        public ObterSecoesPAPQuery(string codigoTurma, string codigoAluno, long pAPPeriodoId)
        {
            CodigoTurma = codigoTurma;
            CodigoAluno = codigoAluno;
            PAPPeriodoId = pAPPeriodoId;
        }

        public string CodigoTurma { get; set; }
        public string CodigoAluno { get; set; }
        public long PAPPeriodoId { get; set; }
    }

    public class OObterSecoesPAPQueryValidator : AbstractValidator<ObterSecoesPAPQuery>
    {
        public OObterSecoesPAPQueryValidator()
        {
            RuleFor(x => x.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado.");

            RuleFor(x => x.PAPPeriodoId)
                .NotNull()
                .WithMessage("O período id do relatório pap deve ser informado.");
        }
    }
}
