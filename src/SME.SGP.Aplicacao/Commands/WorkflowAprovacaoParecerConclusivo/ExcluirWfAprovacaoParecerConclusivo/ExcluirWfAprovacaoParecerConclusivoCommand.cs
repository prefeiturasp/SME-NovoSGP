using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWfAprovacaoParecerConclusivoCommand : IRequest
    {
        public ExcluirWfAprovacaoParecerConclusivoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ExcluirWfAprovacaoParecerConclusivoCommandValidator : AbstractValidator<ExcluirWfAprovacaoParecerConclusivoCommand>
    {
        public ExcluirWfAprovacaoParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O identificador do workflow de aprovação do parecer conclusivo deve ser informado para exclusão");
        }
    }
}
