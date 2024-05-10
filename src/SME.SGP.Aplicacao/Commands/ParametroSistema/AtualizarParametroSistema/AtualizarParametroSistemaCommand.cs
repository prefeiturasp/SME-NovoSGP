using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtualizarParametroSistemaCommand : IRequest<long>
    {
        public AtualizarParametroSistemaCommand(ParametrosSistema parametro)
        {
            Parametro = parametro;
        }

        public ParametrosSistema Parametro { get; }
    }

    public class AtualizarParametroSistemaCommandValidator : AbstractValidator<AtualizarParametroSistemaCommand>
    {
        public AtualizarParametroSistemaCommandValidator()
        {
            RuleFor(a => a.Parametro)
                .NotEmpty()
                .WithMessage("O parâmetro do sistema deve ser informado para atualização");
        }
    }
}
