using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ValidarUeEducacaoInfantilQuery : IRequest<bool>
    {
        public ValidarUeEducacaoInfantilQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }

    public class ValidarUeEducacaoInfantilQueryValidator : AbstractValidator<ValidarUeEducacaoInfantilQuery>
    {
        public ValidarUeEducacaoInfantilQueryValidator()
        {
            RuleFor(c => c.UeId)
            .NotEmpty()
            .WithMessage("O id da UE deve ser informado para consulta de modalidade infantil.");
        }
    }
}
