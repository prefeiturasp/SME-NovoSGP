using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCadastroAcessoABAECommand : IRequest<long>
    {
        public SalvarCadastroAcessoABAECommand(CadastroAcessoABAE cadastroAcessoAbae)
        {
            CadastroAcessoABAE = cadastroAcessoAbae;
        }

        public CadastroAcessoABAE CadastroAcessoABAE { get; }
    }

    public class SalvarCadastroAcessoABAECommandValidator : AbstractValidator<SalvarCadastroAcessoABAECommand>
    {
        public SalvarCadastroAcessoABAECommandValidator()
        {
            RuleFor(x => x.CadastroAcessoABAE)
                .NotNull()
                .WithMessage("O objeto de ABAE deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.UeId)
                .GreaterThan(0)
                .WithMessage("O identificador da Ue deve ser informada para o cadastro de acesso ABAE.");

            RuleFor(x => x.CadastroAcessoABAE.Nome)
                .NotEmpty()
                .WithMessage("O nome do usuário deve ser informado para o cadastro de acesso ABAE.");

            RuleFor(x => x.CadastroAcessoABAE.Cpf)
                .NotEmpty()
                .WithMessage("O cpf do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Email)
                .NotEmpty()
                .WithMessage("O e-mail do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Telefone)
                .NotEmpty()
                .WithMessage("O telefone do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Cep)
                .NotEmpty()
                .WithMessage("O cep do endereço do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Endereco)
                .NotEmpty()
                .WithMessage("O endereço do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Numero)
                .GreaterThan(0)
                .WithMessage("O número do endereço do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Cidade)
                .NotEmpty()
                .WithMessage("A cidade do endereço do usuário deve ser informado para o cadastro de acesso ABAE.");
            
            RuleFor(x => x.CadastroAcessoABAE.Estado)
                .NotEmpty()
                .WithMessage("O estado do endereço do usuário deve ser informado para o cadastro de acesso ABAE.");
        }
    }
}
