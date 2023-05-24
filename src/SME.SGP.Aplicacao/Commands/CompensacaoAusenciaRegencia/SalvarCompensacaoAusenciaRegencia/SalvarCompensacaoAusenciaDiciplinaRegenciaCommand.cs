using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaDiciplinaRegenciaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaDiciplinaRegenciaCommand(CompensacaoAusenciaDisciplinaRegencia compensacaoAusenciaDisciplinaRegencia)
        {
            CompensacaoAusenciaDisciplinaRegencia = compensacaoAusenciaDisciplinaRegencia;
        }

        public Dominio.CompensacaoAusenciaDisciplinaRegencia CompensacaoAusenciaDisciplinaRegencia { get; }
    }

    public class SalvarCompensacaoAusenciaDiciplinaRegenciaCommandValidator : AbstractValidator<SalvarCompensacaoAusenciaDiciplinaRegenciaCommand>
    {
        public SalvarCompensacaoAusenciaDiciplinaRegenciaCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaDisciplinaRegencia)
                .NotNull()
                .WithMessage("A compensação ausência disciplina regencia deve ser preenchida para salvar.");

            RuleFor(x => x.CompensacaoAusenciaDisciplinaRegencia.CompensacaoAusenciaId)
                .GreaterThan(0)
                .WithMessage("A compensação ausência id deve ser preenchida para salvar a compensação ausência disciplina regencia.");

            RuleFor(x => x.CompensacaoAusenciaDisciplinaRegencia.DisciplinaId)
                .NotEmpty()
                .WithMessage("A disciplina deve ser preenchida para salvar a compensação ausência disciplina regencia.");
        }
    }
}
