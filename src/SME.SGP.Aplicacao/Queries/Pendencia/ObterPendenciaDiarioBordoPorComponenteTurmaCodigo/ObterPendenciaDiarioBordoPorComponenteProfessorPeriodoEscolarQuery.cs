using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery : IRequest<long>
    {
        public long ComponenteCurricularId { get; set; }
        public string CodigoRf { get; set; }
        public long PeriodoEscolarId { get; set; }

        public string CodigoTurma { get; set; }

        public ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(long componenteId, string codigoRf, long periodoEscolarId, string turmaCodigo = "")
        {
            ComponenteCurricularId = componenteId;
            CodigoRf = codigoRf;
            PeriodoEscolarId = periodoEscolarId;
            CodigoTurma = turmaCodigo;
        }
    }

    public class ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryQueryValidator : AbstractValidator<ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery>
    {
        public ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQueryQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o código do professor para verificar se já existe pendência.");

            RuleFor(a => a.PeriodoEscolarId)
               .NotEmpty()
               .WithMessage("É necessário informar o período escolar para verificar se já existe pendência.");
        }
    }
}
