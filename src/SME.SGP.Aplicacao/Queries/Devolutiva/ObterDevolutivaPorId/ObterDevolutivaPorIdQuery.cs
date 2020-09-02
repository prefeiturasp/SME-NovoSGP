using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorIdQuery : IRequest<Devolutiva>
    {
        public ObterDevolutivaPorIdQuery(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }

    public class ObterDevolutivaPorIdQueryValidator : AbstractValidator<ObterDevolutivaPorIdQuery>
    {
        public ObterDevolutivaPorIdQueryValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da devolutiva para consulta.");
        }
    }
}
