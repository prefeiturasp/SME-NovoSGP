using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal
{
    public class PainelEducacionalSalvarVisaoGeralCommand : IRequest<bool>
    {
        public PainelEducacionalSalvarVisaoGeralCommand(IEnumerable<PainelEducacionalVisaoGeral> registros)
        {
            VisaoGeral = registros;
        }

        public IEnumerable<PainelEducacionalVisaoGeral> VisaoGeral { get; set; }
    }

    public class PainelEducacionalSalvarVisaoGeralCommandValidator : AbstractValidator<PainelEducacionalSalvarVisaoGeralCommand>
    {
        public PainelEducacionalSalvarVisaoGeralCommandValidator()
        {
            RuleFor(c => c.VisaoGeral)
                .Must(a => a.Any())
                .WithMessage("Informe um registro de visão geral");
        }
    }
}
