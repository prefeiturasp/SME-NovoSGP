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
        public ObterFuncionariosPorUeQuery(string codigoUe, string[] codigosRfs)
        {
            CodigoUe = codigoUe;
            CodigosRfs = codigosRfs;
        }

        public ObterFuncionariosPorUeQuery(string codigoUe, string filtro)
        {
            CodigoUe = codigoUe;
            Filtro = filtro;
        }

        public string CodigoUe { get; set; }
        public string[] CodigosRfs { get; set; }
        public string Filtro { get; set; }
    }

    public class ObterFuncionariosPorUeQueryValidator : AbstractValidator<ObterFuncionariosPorUeQuery>
    {
        public ObterFuncionariosPorUeQueryValidator()
        {
            RuleFor(x => x.CodigoUe).NotEmpty().WithMessage("O Código da Ue deve ser informado para obter funcionários.");
        }
    }
}
