using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQuery : IRequest<long>
    {
        public long ComponenteCurricularId { get; set; }
        public string CodigoRf { get; set; }
        public long PeriodoEscolarId { get; set; }

        public ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQuery(long componenteId, string codigoRf, long periodoEscolarId)
        {
            ComponenteCurricularId = componenteId;
            CodigoRf = codigoRf;
            PeriodoEscolarId = periodoEscolarId;
        }
    }

    public class ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQueryValidator : AbstractValidator<ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQuery>
    {
        public ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o RF para verificar se já existe pendência.");

            RuleFor(a => a.CodigoRf)
               .NotEmpty()
               .WithMessage("É necessário informar o id do período escolar para verificar se já existe pendência.");
        }
    }
}
