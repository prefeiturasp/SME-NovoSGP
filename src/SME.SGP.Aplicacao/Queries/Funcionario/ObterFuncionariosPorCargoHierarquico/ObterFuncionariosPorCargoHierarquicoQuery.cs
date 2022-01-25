using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoHierarquicoQuery : IRequest<IEnumerable<FuncionarioCargoDTO>>
    {
        public ObterFuncionariosPorCargoHierarquicoQuery(string codigoUe, Cargo cargo, bool primeiroNivel = true, bool notificacaoExigeAcao = false)
        {
            CodigoUe = codigoUe;
            Cargo = cargo;
            PrimeiroNivel = primeiroNivel;
            NotificacaoExigeAcao = notificacaoExigeAcao;
        }

        public string CodigoUe { get; }
        public Cargo Cargo { get; }
        public bool PrimeiroNivel { get; }
        public bool NotificacaoExigeAcao { get; }
    }

    public class ObterFuncionariosPorCargoHierarquicoQueryValidator : AbstractValidator<ObterFuncionariosPorCargoHierarquicoQuery>
    {
        public ObterFuncionariosPorCargoHierarquicoQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para pesquisa de funcionários na UE");
        }
    }
}
