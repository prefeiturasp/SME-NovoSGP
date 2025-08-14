using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery : IRequest<IEnumerable<FuncionarioFuncaoAtividadeDTO>>
    {
        public ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery(string codigoUe, FuncaoAtividade funcaoAtividade, bool primeiroNivel = true, bool notificacaoExigeAcao = false)
        {
            CodigoUe = codigoUe;
            CodigoFuncaoAtividade = funcaoAtividade;
            PrimeiroNivel = primeiroNivel;
            NotificacaoExigeAcao = notificacaoExigeAcao;
        }

        public string CodigoUe { get; }
        public FuncaoAtividade CodigoFuncaoAtividade { get; }
        public bool PrimeiroNivel { get; }
        public bool NotificacaoExigeAcao { get; }
    }

    public class ObterFuncionariosPorFuncaoAtividadeHierarquicoQueryValidator : AbstractValidator<ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery>
    {
        public ObterFuncionariosPorFuncaoAtividadeHierarquicoQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para pesquisa de funcionários na UE");
        }
    }
}
