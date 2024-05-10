using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasInfantilPorUEQuery : IRequest<IEnumerable<TurmaDTO>>
    {
        public ObterTurmasInfantilPorUEQuery(int anoLetivo, string ueCodigo)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public int AnoLetivo { get; set; }
        public string UeCodigo { get; }
    }

    public class ObterTurmasInfantilQueryValidator : AbstractValidator<ObterTurmasInfantilPorUEQuery>
    {
        public ObterTurmasInfantilQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter as Turmas da Modalidade Infantil");

            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para obter as Turmas da Modalidade Infantil");
        }
    }
}
