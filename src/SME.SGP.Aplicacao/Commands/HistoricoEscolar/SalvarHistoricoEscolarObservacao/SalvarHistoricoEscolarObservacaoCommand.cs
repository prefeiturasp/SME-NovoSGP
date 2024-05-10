using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Commands.HistoricoEscolar
{
    public class SalvarHistoricoEscolarObservacaoCommand : IRequest<long>
    {
        public SalvarHistoricoEscolarObservacaoCommand(HistoricoEscolarObservacao historicoEscolarObservacao)
        {
            HistoricoEscolarObservacao = historicoEscolarObservacao;
        }

        public HistoricoEscolarObservacao HistoricoEscolarObservacao { get; }
    }

    public class SalvarHistoricoEscolarObservacaoCommandValidator : AbstractValidator<SalvarHistoricoEscolarObservacaoCommand>
    {
        public SalvarHistoricoEscolarObservacaoCommandValidator()
        {
            RuleFor(f => f.HistoricoEscolarObservacao)
                .NotEmpty();

            RuleFor(f => f.HistoricoEscolarObservacao.AlunoCodigo)
                .NotEmpty()
                .WithMessage("Código do aluno deve ser informado.");

            RuleFor(f => f.HistoricoEscolarObservacao.Observacao)
                .NotEmpty()
                .WithMessage("Observação deve ser informado.");

            RuleFor(f => f.HistoricoEscolarObservacao.Observacao)
                .MaximumLength(500)
                .WithMessage("Observação não pode conter mais que 500 caracteres.");
        }
    }
}
