using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorIdQuery : IRequest<CadastroAcessoABAE>
    {
        public ObterCadastroAcessoABAEPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }

    public class ObterCadastroAcessoABAEPorIdQueryValidator : AbstractValidator<ObterCadastroAcessoABAEPorIdQuery>
    {
        public ObterCadastroAcessoABAEPorIdQueryValidator()
        {
            RuleFor(a => a.Id)
                .GreaterThan(0)
                .WithMessage("É necessário informar o identificador para a busca de cadastro de acesso ABAE");
        }
    }
}
