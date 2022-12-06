using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtribuicoesCJAtivasQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public string CodigoRf { get; set; }

        public ObterAtribuicoesCJAtivasQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
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
