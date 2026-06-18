using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsideradasNoConselhoQuery : IRequest<List<string>>
    {
        public IEnumerable<string> TurmasCodigos { get; set; }
        public Turma TurmaSelecionada { get; set; }

        public ObterTurmasConsideradasNoConselhoQuery(IEnumerable<string> turmasCodigos, Turma turmaSelecionada)
        {
            TurmasCodigos = turmasCodigos;
            TurmaSelecionada = turmaSelecionada;
        }

        public class ObterTurmasConsideradasNoConselhoQueryValidator : AbstractValidator<ObterTurmasConsideradasNoConselhoQuery>
        {
            public ObterTurmasConsideradasNoConselhoQueryValidator()
            {
                RuleFor(a => a.TurmasCodigos)
                    .NotEmpty()
                    .WithMessage("É necessário informar o(s) código(s) da(s) turma(s) para verificar se deve considerar no conselho de classe.");
            }
        }
    }
}
