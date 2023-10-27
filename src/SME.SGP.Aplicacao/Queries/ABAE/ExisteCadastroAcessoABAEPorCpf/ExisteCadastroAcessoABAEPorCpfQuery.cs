using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExisteCadastroAcessoABAEPorCpfQuery : IRequest<bool>
    {
        public ExisteCadastroAcessoABAEPorCpfQuery(string cpf)
        {
            Cpf = cpf;
        }
        public string Cpf { get; set; }
    }

    public class ExisteCadastroAcessoABAEQueryValidator : AbstractValidator<ExisteCadastroAcessoABAEPorCpfQuery>
    {
        public ExisteCadastroAcessoABAEQueryValidator()
        {
            RuleFor(a => a.Cpf)
                .NotEmpty()
                .WithMessage("É necessário informar o cpf para a busca de cadastro de acesso ABAE");
        }
    }
}
