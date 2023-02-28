using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoExternaHierarquicoQuery : IRequest<IEnumerable<FuncionarioFuncaoExternaDTO>>
    {
        public ObterFuncionariosPorFuncaoExternaHierarquicoQuery(string codigoUe, FuncaoExterna funcaoExterna, bool primeiroNivel = true, bool notificacaoExigeAcao = false)
        {
            CodigoUe = codigoUe;
            FuncaoExterna = funcaoExterna;
            PrimeiroNivel = primeiroNivel;
            NotificacaoExigeAcao = notificacaoExigeAcao;
        }

        public string CodigoUe { get; }
        public FuncaoExterna FuncaoExterna { get; }
        public bool PrimeiroNivel { get; }
        public bool NotificacaoExigeAcao { get; }
    }

    public class ObterFuncionariosPorFuncaoExternaHierarquicoQueryValidator : AbstractValidator<ObterFuncionariosPorFuncaoExternaHierarquicoQuery>
    {
        public ObterFuncionariosPorFuncaoExternaHierarquicoQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para pesquisa de funcionários na UE");
        }
    }
}
