using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorCargoUeQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorCargoUeQuery(string ueId, long cargoId)
        {
            UeId = ueId;
            CargoId = cargoId;
        }

        public long CargoId { get; set; }

        public string UeId { get; set; }
    }

    public class ObterFuncionariosPorCargoUeQueryValidator : AbstractValidator<ObterFuncionariosPorCargoUeQuery>
    {
        public ObterFuncionariosPorCargoUeQueryValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Informe o Cargo para Obter os Funcionarios Por Cargo e Ue");
            RuleFor(x => x.UeId).NotEmpty().WithMessage("Informe a UE para Obter os Funcionarios Por Cargo e Ue");
        }
    }
}