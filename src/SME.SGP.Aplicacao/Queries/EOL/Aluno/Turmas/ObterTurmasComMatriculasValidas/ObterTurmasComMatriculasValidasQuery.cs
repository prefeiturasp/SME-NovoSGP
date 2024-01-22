using System;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasQuery : IRequest<IEnumerable<string>>
    {    
        public string[] TurmasCodigos { get; set; }
        public string AlunoCodigo { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public DateTime FinalDoFechamento { get; set; }

        public ObterTurmasComMatriculasValidasQuery(string alunoCodigo, string[] turmasCodigos, DateTime periodoInicio, DateTime periodoFim, DateTime inicioDoFechamento, DateTime finalDoFechamento)
        {
            TurmasCodigos = turmasCodigos;
            AlunoCodigo = alunoCodigo;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            InicioDoFechamento = inicioDoFechamento;
            FinalDoFechamento = finalDoFechamento;
        }
    }
    
    public class ObterTurmasComMatriculasValidasQueryValidator : AbstractValidator<ObterTurmasComMatriculasValidasQuery>
    {
        public ObterTurmasComMatriculasValidasQueryValidator()
        {
            RuleFor(c => c.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para busca de matrículas/turma no EOL.");
            
            RuleFor(c => c.TurmasCodigos)
                .NotEmpty()
                .WithMessage("Os códigos das turmas devem ser informados para busca de matrículas/turma no EOL.");
            
            RuleFor(c => c.PeriodoInicio)
                .NotEmpty()
                .WithMessage("O período início deve ser informado para tratamento da busca de matrículas/turma no EOL.");
            
            RuleFor(c => c.PeriodoFim)
                .NotEmpty()
                .WithMessage("O período final deve ser informado para tratamento da busca de matrículas/turma no EOL.");
        }        
    }
}
