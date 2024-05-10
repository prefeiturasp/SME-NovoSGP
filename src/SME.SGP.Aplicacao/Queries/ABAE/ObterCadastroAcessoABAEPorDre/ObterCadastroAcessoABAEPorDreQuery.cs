using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEPorDreQuery : IRequest<IEnumerable<NomeCpfABAEDto>>
    {
        public ObterCadastroAcessoABAEPorDreQuery(string cpf, string codigoDre, string codigoUe, string nome)
        {
            Cpf = cpf;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Nome = nome;
        }

        public string Cpf { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Nome { get; set; }
    }

    public class ObterCadastroAcessoABAEPorDreQueryValidator : AbstractValidator<ObterCadastroAcessoABAEPorDreQuery>
    {
        public ObterCadastroAcessoABAEPorDreQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
                .NotEmpty()
                .WithMessage("O código da DRE é obrigatório.");
        }
    }
}
