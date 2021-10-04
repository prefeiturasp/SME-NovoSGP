using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterDescricaoComponenteCurricularPorIdQuery : IRequest<string>
    {
        public ObterDescricaoComponenteCurricularPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }


    public class ObterDescricaoComponenteCurricularPorIdQueryValidator : AbstractValidator<ObterDescricaoComponenteCurricularPorIdQuery>
    {
        public ObterDescricaoComponenteCurricularPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id deve ser informado para consulta da descrição do componente curricular");
        }
    }
}
