using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioCoreSSOQuery : IRequest<MeusDadosDto>
    {
        public ObterUsuarioCoreSSOQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
        }

        public string CodigoRf { get; set; }
    }
    public class ObterUsuarioCoreSSOQueryValidator : AbstractValidator<ObterUsuarioCoreSSOQuery>
    {
        public ObterUsuarioCoreSSOQueryValidator()
        {
            RuleFor(a => a.CodigoRf)
                .NotEmpty()
                .WithMessage("O código rf deve ser infomado para a consulta de usuário");            
        }
    }
}
