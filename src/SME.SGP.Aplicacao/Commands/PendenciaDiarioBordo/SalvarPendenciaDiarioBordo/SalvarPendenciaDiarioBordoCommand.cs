using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaDiarioBordoCommand : IRequest
    {
        public SalvarPendenciaDiarioBordoCommand()
        {}

        public IEnumerable<ProfessorEComponenteInfantilDto> ProfessoresComponentes { get; set; }
        public AulaComComponenteDto Aula { get; set; }
        public string DescricaoUeDre { get; set; }
        public string TurmaComModalidade { get; set; }
    }

    public class SalvarPendenciaDiarioBordoCommandValidator : AbstractValidator<SalvarPendenciaDiarioBordoCommand>
    {
        public SalvarPendenciaDiarioBordoCommandValidator()
        {
            RuleFor(c => c.Aula)
            .NotEmpty()
            .WithMessage("As aulas devem ser informados para geração de pendência diário de bordo.");

            RuleFor(c => c.DescricaoUeDre)
            .NotEmpty()
            .WithMessage("A descrição da Ue e Dre devem ser informados para geração de pendência diário de bordo.");

            RuleFor(c => c.TurmaComModalidade)
            .NotEmpty()
            .WithMessage("A turma com ano e modalidade devem ser informados para geração de pendência diário de bordo.");

            RuleFor(c => c.ProfessoresComponentes)
            .Must(a => a.Any())
            .WithMessage("A relação de professores e componentes devem ser informados para geração de pendência diário de bordo.");
        }
    }
}
