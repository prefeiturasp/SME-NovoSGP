using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class DefinirComponenteCurricularParaAulaQuery : IRequest<(long codigoComponente, long? codigoTerritorio)>
    {
        public DefinirComponenteCurricularParaAulaQuery(string codigoTurma, long codigoComponenteCurricular, Usuario usuario)
        {
            CodigoTurma = codigoTurma;
            CodigoComponenteCurricular = codigoComponenteCurricular;
            Usuario = usuario;
        }
        public string CodigoTurma { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class DefinirComponenteCurricularParaAulaQueryValidator : AbstractValidator<DefinirComponenteCurricularParaAulaQuery>
    {
        public DefinirComponenteCurricularParaAulaQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotNull()
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(a => a.CodigoComponenteCurricular)
                .GreaterThan(0)
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(a => a.Usuario)
                .NotEmpty()
                .WithMessage("O usuário logado deve ser informado.");
        }
    }
}
