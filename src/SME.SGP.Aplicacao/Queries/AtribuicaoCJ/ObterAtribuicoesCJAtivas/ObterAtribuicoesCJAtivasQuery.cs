using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesCJAtivasQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public string CodigoRf { get; set; }
        public bool Historico { get; set; }

        public ObterAtribuicoesCJAtivasQuery(string codigoRf, bool historico)
        {
            CodigoRf = codigoRf;
            Historico = historico;
        }
    }

    public class ObterAtribuicoesCJAtivasQueryValidator : AbstractValidator<ObterAtribuicoesCJAtivasQuery>
    {
        public ObterAtribuicoesCJAtivasQueryValidator()
        {
            RuleFor(a => a.CodigoRf)
                .NotEmpty()
                .WithMessage("É necessário informar o código RF para obter se o mesmo, possui atribuição CJ ativa");
        }
    }
}
