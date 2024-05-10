using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaCommand(CompensacaoAusencia compensacaoAusencia)
        {
            CompensacaoAusencia = compensacaoAusencia;
        }

        public CompensacaoAusencia CompensacaoAusencia { get; }
    }

    public class SalvarCompensacaoAusenciaCommandValidator : AbstractValidator<SalvarCompensacaoAusenciaCommand>
    {
        public SalvarCompensacaoAusenciaCommandValidator()
        {
            RuleFor(x => x.CompensacaoAusencia)
                .NotNull()
                .WithMessage("A compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo da compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.DisciplinaId)
                .NotEmpty()
                .WithMessage("A disciplina da compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.Bimestre)
                .GreaterThan(0)
                .WithMessage("O bimestre da compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.Nome)
                .NotEmpty()
                .WithMessage("A atividade da compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.Descricao)
                .NotEmpty()
                .WithMessage("A descrição da compensação ausência deve ser preenchido para salvar");

            RuleFor(x => x.CompensacaoAusencia.TurmaId)
                .GreaterThan(0)
                .WithMessage("A turma da compensação ausência deve ser preenchido para salvar");
        }
    }
}
