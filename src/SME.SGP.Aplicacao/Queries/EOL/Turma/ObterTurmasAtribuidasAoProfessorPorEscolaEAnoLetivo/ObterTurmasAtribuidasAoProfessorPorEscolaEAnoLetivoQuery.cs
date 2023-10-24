using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery : IRequest<IEnumerable<TurmaDto>>
    {
        public ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            RfProfessor = rfProfessor;
            CodigoEscola = codigoEscola;
            AnoLetivo = anoLetivo;
        }

        public string RfProfessor { get; set; }
        public string CodigoEscola { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQueryValidator : AbstractValidator<ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery>
    {
        public ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.RfProfessor)
            .NotEmpty()
            .WithMessage("O Rf do professor deve ser informado para a busca de turmas atribuídas ao professor.");
            
            RuleFor(c => c.CodigoEscola)
                .NotEmpty()
                .WithMessage("O código da escola deve ser informado para a busca de turmas atribuídas ao professor.");
            
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado para a busca de turmas atribuídas ao professor.");
        }
    }

}
