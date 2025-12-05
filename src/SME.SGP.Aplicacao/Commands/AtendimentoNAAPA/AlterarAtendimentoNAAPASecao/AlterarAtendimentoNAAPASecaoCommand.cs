using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarAtendimentoNAAPASecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarAtendimentoNAAPASecaoCommand(EncaminhamentoNAAPASecao secao)
        {
            Secao = secao;
        }

        public EncaminhamentoNAAPASecao Secao { get; }
    }

    public class AlterarAtendimentoNAAPASecaoCommandValidator : AbstractValidator<AlterarAtendimentoNAAPASecaoCommand>
    {
        public AlterarAtendimentoNAAPASecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
               .NotEmpty()
               .WithMessage("A seção deve ser informada para a alteração do atendimento NAAPA!");
        }
    }
}
