using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeAvaliativaDisciplinaCommand : IRequest
    {
        public SalvarAtividadeAvaliativaDisciplinaCommand(long atividadeAvaliativaId, string componenteCurricularId)
        {
            AtividadeAvaliativaId = atividadeAvaliativaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long AtividadeAvaliativaId { get; }
        public string ComponenteCurricularId { get; }
    }

    public class SalvarAtividadeAvaliativaDisciplinaCommandValidator : AbstractValidator<SalvarAtividadeAvaliativaDisciplinaCommand>
    {
        public SalvarAtividadeAvaliativaDisciplinaCommandValidator()
        {
            RuleFor(a => a.AtividadeAvaliativaId)
                .NotEmpty()
                .WithMessage("O id da atividade avaliativa deve ser infomada para vincular o componente curricular");

            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser infomado para vincular a atividade avaliativa");
        }
    }
}
