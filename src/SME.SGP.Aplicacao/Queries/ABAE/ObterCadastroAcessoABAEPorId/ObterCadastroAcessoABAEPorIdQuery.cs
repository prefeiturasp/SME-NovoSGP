using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorIdQuery : IRequest<CadastroAcessoABAE>
    {
        public ObterCadastroAcessoABAEPorIdQuery(long cadastroAcessoABAEId)
        {
            CadastroAcessoABAEId = cadastroAcessoABAEId;
        }
        public long CadastroAcessoABAEId { get; set; }
    }

    public class ObterCadastroAcessoABAEPorIdQueryValidator : AbstractValidator<ObterCadastroAcessoABAEPorIdQuery>
    {
        public ObterCadastroAcessoABAEPorIdQueryValidator()
        {
            RuleFor(a => a.CadastroAcessoABAEId)
                .GreaterThan(0)
                .WithMessage("É necessário informar o identificador para a busca de cadastro de acesso ABAE");
        }
    }
}
