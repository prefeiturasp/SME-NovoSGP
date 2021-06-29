using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasInfantilQuery : IRequest<IEnumerable<TurmaDTO>>
    {
        public ObterTurmasInfantilQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasInfantilQueryValidator : AbstractValidator<ObterTurmasInfantilQuery>
    {
        public ObterTurmasInfantilQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter os dados de consolidação acompanhamento de aprendizagem");
        }
    }
}
