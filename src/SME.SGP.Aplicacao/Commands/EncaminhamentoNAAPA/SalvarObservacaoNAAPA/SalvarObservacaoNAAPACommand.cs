using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacaoNAAPACommand : IRequest<bool>
    {
        public SalvarObservacaoNAAPACommand(AtendimentoNAAPAObservacaoSalvarDto filtro)
        {
            encaminhamentoNAAPAObservacaoSalvarDto = filtro;
        }

        public AtendimentoNAAPAObservacaoSalvarDto encaminhamentoNAAPAObservacaoSalvarDto { get; set; }
    }
    public class SalvarObservacaoNAAPACommandValidator : AbstractValidator<SalvarObservacaoNAAPACommand>
    {
        public SalvarObservacaoNAAPACommandValidator()
        {
            RuleFor(a => a.encaminhamentoNAAPAObservacaoSalvarDto.Observacao)
                .NotEmpty()
                .WithMessage("A Observação deve ser informada");
            RuleFor(a => a.encaminhamentoNAAPAObservacaoSalvarDto.EncaminhamentoNAAPAId)
                .GreaterThan(0)
                .WithMessage("O Encaminhamento NAAPA deve ser informado");
        }
    }
}