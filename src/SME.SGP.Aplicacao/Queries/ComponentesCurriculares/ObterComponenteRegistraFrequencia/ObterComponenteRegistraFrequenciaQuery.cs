using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteRegistraFrequenciaQuery : IRequest<bool>
    {
        public ObterComponenteRegistraFrequenciaQuery(long componenteCurricularId)
        {
            ComponenteCurricularId = componenteCurricularId;
        }

        public long ComponenteCurricularId { get; }
    }

    public class ObterComponenteRegistraFrequenciaQueryValidator : AbstractValidator<ObterComponenteRegistraFrequenciaQuery>
    {
        public ObterComponenteRegistraFrequenciaQueryValidator()
        {
            RuleFor(a => a.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O identificador do componente curricular deve ser informado para verificação do registro de frequência");
        }
    }
}
