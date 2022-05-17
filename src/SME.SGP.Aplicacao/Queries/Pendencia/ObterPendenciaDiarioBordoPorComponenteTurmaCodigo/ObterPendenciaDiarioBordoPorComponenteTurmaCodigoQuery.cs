using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery : IRequest<long>
    {
        public long ComponenteCurricularId { get; set; }
        public string TurmaCodigo { get; set; }

        public ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery(long componenteId, string turmaCodigo)
        {
            ComponenteCurricularId = componenteId;
            TurmaCodigo = turmaCodigo;
        }
    }

    public class ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQueryValidator : AbstractValidator<ObterPendenciaDiarioBordoPorComponenteTurmaCodigoQuery>
    {
        public ObterPendenciaDiarioBordoPorComponentePeriodoEscolarProfessorQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("É necessário informar o id do componente curricular para verificar se já existe pendência.");

            RuleFor(a => a.TurmaCodigo)
               .NotEmpty()
               .WithMessage("É necessário informar o código da turma para verificar se já existe pendência.");
        }
    }
}
