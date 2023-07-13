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
    public class ObterTurmasComMatriculaValidasParaValidarConselhoQuery : IRequest<IEnumerable<string>>
    {
        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

        public ObterTurmasComMatriculaValidasParaValidarConselhoQuery(string alunoCodigo, string[] turmasCodigos, DateTime periodoInicio, DateTime periodoFim)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }
    }

    public class ObterTurmasComMatriculaValidasParaValidarConselhoQueryValidator : AbstractValidator<ObterTurmasComMatriculaValidasParaValidarConselhoQuery>
    {
        public ObterTurmasComMatriculaValidasParaValidarConselhoQueryValidator()
        {
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para busca de matrículas/turma no EOL.");

            RuleFor(c => c.TurmasCodigos)
                .NotEmpty()
                .WithMessage("As turmas devem ser informados para busca de matrículas/turma no EOL.");

            RuleFor(c => c.PeriodoInicio)
                .NotEmpty()
                .WithMessage("O período início deve ser informado para tratamento da busca de matrículas/turma no EOL.");

            RuleFor(c => c.PeriodoFim)
                .NotEmpty()
                .WithMessage("O período final deve ser informado para tratamento da busca de matrículas/turma no EOL.");
        }
    }
}
