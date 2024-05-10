using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEECommand : IRequest<bool>
    {
        public AtribuirResponsavelEncaminhamentoAEECommand(long encaminhamentoId, string rfResponsavel)
        {
            EncaminhamentoId = encaminhamentoId;
            RfResponsavel = rfResponsavel;
        }

        public long EncaminhamentoId { get; set; }
        public string RfResponsavel { get; set; }
    }

    public class AtribuirResponsavelEncaminhamentoAEECommandValidator : AbstractValidator<AtribuirResponsavelEncaminhamentoAEECommand>
    {
        public AtribuirResponsavelEncaminhamentoAEECommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Encaminhamento deve ser informado!");
            RuleFor(x => x.RfResponsavel)
                   .GreaterThan(string.Empty)
                   .WithMessage("O Rf do responsável deve ser informado!");
        }
    }
}
