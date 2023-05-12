using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReabrirEncaminhamentoNAAPACommand : IRequest<SituacaoDto>
    {
        public ReabrirEncaminhamentoNAAPACommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; set; }
    }
    public class ReabrirEncaminhamentoNAAPACommandValidator : AbstractValidator<ReabrirEncaminhamentoNAAPACommand>
    {
        public ReabrirEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Encaminhamento deve ser informado!");
        }
    }
}
