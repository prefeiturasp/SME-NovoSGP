using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFinaisPorAlunoTurmasQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFinaisPorAlunoTurmasQuery(string alunoCodigo, string[] turmasCodigos)
        {
            AlunoCodigo = alunoCodigo;
            TurmasCodigos = turmasCodigos;
        }

        public string AlunoCodigo { get; set; }
        public string[] TurmasCodigos { get; set; }


        public class ObterNotasFinaisPorAlunoTurmasBimestreQueryValidator : AbstractValidator<ObterNotasFinaisPorAlunoTurmasQuery>
        {
            public ObterNotasFinaisPorAlunoTurmasBimestreQueryValidator()
            {
                RuleFor(a => a.TurmasCodigos)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Necessário informar os códigos de turmas para obter as notas de fechmamento");
                RuleFor(a => a.AlunoCodigo)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Necessário informar o código do aluno para obter as notas de fechmamento");
            }
        }
    }
}
