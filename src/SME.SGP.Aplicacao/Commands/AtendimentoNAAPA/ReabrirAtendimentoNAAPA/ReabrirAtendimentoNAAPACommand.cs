using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReabrirAtendimentoNAAPACommand : IRequest<SituacaoDto>
    {
        public ReabrirAtendimentoNAAPACommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; set; }
    }
    public class ReabrirAtendimentoNAAPACommandValidator : AbstractValidator<ReabrirAtendimentoNAAPACommand>
    {
        public ReabrirAtendimentoNAAPACommandValidator()
        {
            RuleFor(x => x.EncaminhamentoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Atendimento deve ser informado!");
        }
    }
}
