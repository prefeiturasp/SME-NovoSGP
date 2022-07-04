using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelGeralDoPlanoCommand : IRequest<bool>
    {
        public AtribuirResponsavelGeralDoPlanoCommand(long planoAEEId, string responsavelRF, string responsavelNome)
        {
            PlanoAEEId = planoAEEId;
            ResponsavelRF = responsavelRF;
            ResponsavelNome = responsavelNome;
        }

        public long PlanoAEEId { get; set; }
        public string ResponsavelRF { get; set; }
        public string ResponsavelNome { get; set; }
    }

    public class AtribuirResponsavelGeralDoPlanoCommandValidator : AbstractValidator<AtribuirResponsavelGeralDoPlanoCommand>
    {
        public AtribuirResponsavelGeralDoPlanoCommandValidator()
        {
            RuleFor(x => x.PlanoAEEId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Plano AEE deve ser informado para atribuição do responsável do plano!");
            RuleFor(x => x.ResponsavelRF)
                   .NotEmpty()
                   .WithMessage("O RF do responsável deve ser informado para atribuição do responsável do plano!");
        }
    }
}
