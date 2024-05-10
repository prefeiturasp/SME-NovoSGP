using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWFAprovacaoNotaFechamentoCommand : IRequest
    {
        public ExcluirWFAprovacaoNotaFechamentoCommand(WfAprovacaoNotaFechamento wfAprovacaoNotaFechamento)
        {
            WfAprovacaoNotaFechamento = wfAprovacaoNotaFechamento;
        }

        public WfAprovacaoNotaFechamento WfAprovacaoNotaFechamento { get; }
    }

    public class ExcluirWFAprovacaoNotaFechamentoCommandValidator : AbstractValidator<ExcluirWFAprovacaoNotaFechamentoCommand>
    {
        public ExcluirWFAprovacaoNotaFechamentoCommandValidator()
        {
            RuleFor(a => a.WfAprovacaoNotaFechamento)
                .NotEmpty()
                .WithMessage("O Workflow de aprovação de nota de fechamento deve ser informado para exclusão");
        }
    }
}
