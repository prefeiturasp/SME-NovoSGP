using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterComponenteRegistraFrequenciaQuery : IRequest<bool>
    {
        public ObterComponenteRegistraFrequenciaQuery(long componenteCurricularId, long? codigoTerritorioSaber = null)
        {
            ComponenteCurricularId = componenteCurricularId;
            codigoTerritorioSaber = codigoTerritorioSaber;
        }

        public long ComponenteCurricularId { get; }
        public long? CodigoTerritorioSaber { get; set; }
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
