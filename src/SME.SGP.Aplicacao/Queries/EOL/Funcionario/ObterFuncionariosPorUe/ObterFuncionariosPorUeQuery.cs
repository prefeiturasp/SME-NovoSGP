using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorUeQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }
        public string CodigoUe { get; set; }
    }

    public class ObterFuncionariosPorUeQueryValidator : AbstractValidator<ObterFuncionariosPorUeQuery>
    {
        public ObterFuncionariosPorUeQueryValidator()
        {
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Código da Ue deve ser informado para obter funcionários.");
        }
    }
}
