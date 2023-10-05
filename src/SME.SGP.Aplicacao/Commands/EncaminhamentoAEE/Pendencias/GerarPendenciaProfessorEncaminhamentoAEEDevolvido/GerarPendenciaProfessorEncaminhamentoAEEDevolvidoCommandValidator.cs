using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.EncaminhamentoAEE.Pendencias.GerarPendenciaProfessorEncaminhamentoAEEDevolvido
{
    public class GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommandValidator : AbstractValidator<GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommand>
    {
        public GerarPendenciaProfessorEncaminhamentoAEEDevolvidoCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoAEE)
                   .NotEmpty()
                   .WithMessage("O encaminhamento precisa ser informado.");
        }
    }
}
