using FluentValidation;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaParaSupervisorQuery : IRequest<AbrangenciaRetornoEolDto>
    {
        public ObterAbrangenciaParaSupervisorQuery(string[] uesIds)
        {
            UesIds = uesIds;
        }
        public string[] UesIds { get; set; }
    }

    public class ObterAbrangenciaParaSupervisorQueryValidator : AbstractValidator<ObterAbrangenciaParaSupervisorQuery>
    {
        public ObterAbrangenciaParaSupervisorQueryValidator()
        {
            RuleFor(c => c.UesIds)
              .NotNull()
              .WithMessage("As Ues devem ser informadas para obter a abrangência para o supervisor.");
        }
    }
}
