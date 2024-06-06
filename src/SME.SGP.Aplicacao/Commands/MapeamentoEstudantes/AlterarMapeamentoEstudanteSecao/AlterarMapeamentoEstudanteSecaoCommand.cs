using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarMapeamentoEstudanteSecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarMapeamentoEstudanteSecaoCommand(MapeamentoEstudanteSecao secao)
        {
            Secao = secao;
        }

        public MapeamentoEstudanteSecao Secao { get; }
    }

    public class AlterarMapeamentoEstudanteSecaoCommandValidator : AbstractValidator<AlterarMapeamentoEstudanteSecaoCommand>
    {
        public AlterarMapeamentoEstudanteSecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
               .NotEmpty()
               .WithMessage("A seção deve ser informada para a alteração do mapeamento de estudante!");
        }
    }
}
