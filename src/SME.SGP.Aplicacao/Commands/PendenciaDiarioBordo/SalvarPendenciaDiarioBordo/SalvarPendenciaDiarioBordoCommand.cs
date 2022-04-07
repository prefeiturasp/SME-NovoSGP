using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommand : IRequest<bool>
    {
        public SalvarPendenciaDiarioBordoCommand(IEnumerable<Aula> aulas)
        {
            Aulas = aulas;
        }
        public IEnumerable<Aula> Aulas { get; set; }
    }

    public class SalvarPendenciaDiarioBordoCommandValidator : AbstractValidator<SalvarPendenciaDiarioBordoCommand>
    {
        public SalvarPendenciaDiarioBordoCommandValidator()
        {
            RuleFor(c => c.Aulas)
            .Must(a => a.Any())
            .WithMessage("As aulas devem ser informados para geração de pendência diário de bordo.");
        }
    }
}
