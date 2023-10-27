using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExisteCadastroAcessoABAEPorCpfQuery : IRequest<bool>
    {
        public ExisteCadastroAcessoABAEPorCpfQuery(string cpf, long ueId)
        {
            Cpf = cpf;
            UeId = ueId;
        }
        public string Cpf { get; set; }
        public long UeId { get; set; }
    }

    public class ExisteCadastroAcessoABAEQueryValidator : AbstractValidator<ExisteCadastroAcessoABAEPorCpfQuery>
    {
        public ExisteCadastroAcessoABAEQueryValidator()
        {
            RuleFor(a => a.Cpf)
                .NotEmpty()
                .WithMessage("É necessário informar o cpf para a busca de cadastro de acesso ABAE");
            
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("É necessário informar o identificador da Ue para a busca de cadastro de acesso ABAE");
        }
    }
}
