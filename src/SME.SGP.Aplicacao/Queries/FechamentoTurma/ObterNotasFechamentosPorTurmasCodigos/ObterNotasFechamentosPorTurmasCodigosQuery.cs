using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public ObterNotasFechamentosPorTurmasCodigosQuery(string[] turmasCodigos, string alunoCodigo)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
        }
        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }

        public class ObterNotasFechamentosPorTurmasCodigosQueryValidator : AbstractValidator<ObterNotasFechamentosPorTurmasCodigosQuery>
        {
            public ObterNotasFechamentosPorTurmasCodigosQueryValidator()
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
