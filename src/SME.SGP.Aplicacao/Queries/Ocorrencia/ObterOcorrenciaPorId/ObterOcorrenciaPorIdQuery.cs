using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciaPorIdQuery : IRequest<OcorrenciaDto>
    {
        public ObterOcorrenciaPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ObterOcorrenciaPorIdQueryValidator : AbstractValidator<ObterOcorrenciaPorIdQuery>
    {
        public ObterOcorrenciaPorIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O identificador da ocorrência deve ser informado.");
        }
    }
}
