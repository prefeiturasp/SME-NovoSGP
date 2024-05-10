using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteLancaNotaQuery : IRequest<bool>
    {
        public ObterComponenteLancaNotaQuery(long componenteCurricularId)
        {
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ComponenteCurricularId { get; }
    }

    public class ObterComponenteLancaNotaQueryValidator : AbstractValidator<ObterComponenteLancaNotaQuery>
    {
        public ObterComponenteLancaNotaQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado para conferência de lançamento de nota no componente");
        }
    }
}
