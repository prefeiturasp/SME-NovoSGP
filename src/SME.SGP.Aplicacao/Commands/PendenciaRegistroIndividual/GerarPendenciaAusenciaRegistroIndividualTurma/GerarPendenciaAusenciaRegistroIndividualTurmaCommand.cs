using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualTurmaCommand : IRequest<RetornoBaseDto>
    {
        public long TurmaId { get; set; }

        public GerarPendenciaAusenciaRegistroIndividualTurmaCommand(long turmaId)
        {
            TurmaId = turmaId;
        }
    }

    public class GerarPendenciaAusenciaRegistroIndividualTurmaCommandValidator : AbstractValidator<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>
    {
        public GerarPendenciaAusenciaRegistroIndividualTurmaCommandValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para geração de pendência de registro individual.");
        }
    }
}