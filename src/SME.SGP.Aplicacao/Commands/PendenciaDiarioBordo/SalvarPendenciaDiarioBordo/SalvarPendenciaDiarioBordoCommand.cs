using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommand : IRequest
    {
        public SalvarPendenciaDiarioBordoCommand(IEnumerable<AulaComComponenteDto> aulas, List<ProfessorEComponenteInfantilDto> professores)
        {
            Aulas = aulas;
            ProfessoresComponentes = professores;
        }
        public IEnumerable<AulaComComponenteDto> Aulas { get; set; }
        public List<ProfessorEComponenteInfantilDto> ProfessoresComponentes { get; set; }
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
