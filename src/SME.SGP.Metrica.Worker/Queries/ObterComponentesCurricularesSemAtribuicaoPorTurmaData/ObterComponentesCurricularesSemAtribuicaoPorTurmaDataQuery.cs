using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery : IRequest<IEnumerable<string>>
    {
        public ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery(string codigoTurma, DateTime data)
        {
            CodigoTurma = codigoTurma;
            Data = data;
        }

        public string CodigoTurma { get; }
        public DateTime Data { get; }
    }

    public class ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQueryValidator : AbstractValidator<ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQuery>
    {
        public ObterComponentesCurricularesSemAtribuicaoPorTurmaDataQueryValidator()
        {
            RuleFor(a => a.CodigoTurma)
                .NotEmpty()
                .WithMessage("Necessário informar o código da turma para consulta de componentes curriculares sem atribuição.");
            RuleFor(a => a.Data)
                .NotEmpty()
                .WithMessage("Necessário informar a data base para consulta de componentes curriculares sem atribuição.");
        }
    }
}
