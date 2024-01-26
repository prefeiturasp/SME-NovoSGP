using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroAcaoSecaoCommand : IRequest<AuditoriaDto>
    {
        public AlterarRegistroAcaoSecaoCommand(RegistroAcaoBuscaAtivaSecao secao)
        {
            Secao = secao;
        }

        public RegistroAcaoBuscaAtivaSecao Secao { get; }
    }

    public class AlterarRegistroAcaoSecaoCommandValidator : AbstractValidator<AlterarRegistroAcaoSecaoCommand>
    {
        public AlterarRegistroAcaoSecaoCommandValidator()
        {
            RuleFor(a => a.Secao)
               .NotEmpty()
               .WithMessage("A seção deve ser informada para a alteração do registro de ação!");
        }
    }
}
