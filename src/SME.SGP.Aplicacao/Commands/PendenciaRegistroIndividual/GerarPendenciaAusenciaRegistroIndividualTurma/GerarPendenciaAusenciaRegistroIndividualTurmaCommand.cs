using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualTurmaCommand : IRequest<RetornoBaseDto>
    {
        public Turma Turma { get; set; }

        public GerarPendenciaAusenciaRegistroIndividualTurmaCommand(Turma turma)
        {
            Turma = turma;
        }
    }

    public class GerarPendenciaAusenciaRegistroIndividualTurmaCommandValidator : AbstractValidator<GerarPendenciaAusenciaRegistroIndividualTurmaCommand>
    {
        public GerarPendenciaAusenciaRegistroIndividualTurmaCommandValidator()
        {
            RuleFor(x => x.Turma)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para geração de pendência de registro individual.");
        }
    }
}