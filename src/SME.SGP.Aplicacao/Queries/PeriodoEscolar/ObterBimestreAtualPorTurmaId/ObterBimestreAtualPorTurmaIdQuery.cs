using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestreAtualPorTurmaIdQuery : IRequest<int>
    {
        public ObterBimestreAtualPorTurmaIdQuery(Turma turma, DateTime dataReferencia)
        {
            this.Turma = turma;
            this.DataReferencia = dataReferencia;
        }

        public Turma Turma { get; set; }
        public DateTime DataReferencia { get; set; }
    }

    public class ObterBimestreAtualPorTurmaIdQueryValidator : AbstractValidator<ObterBimestreAtualPorTurmaIdQuery>
    {
        public ObterBimestreAtualPorTurmaIdQueryValidator()
        {
            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("Necessário informar a turma para consulta.");
            RuleFor(a => a.DataReferencia)
                .NotEmpty()
                .WithMessage("Necessário informar a data de referência para consulta.");
        }
    }
}
