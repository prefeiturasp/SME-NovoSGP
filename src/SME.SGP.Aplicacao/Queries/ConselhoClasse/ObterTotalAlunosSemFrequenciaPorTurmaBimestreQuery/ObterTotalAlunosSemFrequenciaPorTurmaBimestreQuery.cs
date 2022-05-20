﻿using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery : IRequest<IEnumerable<int>>
    {
        public ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery(string disciplinaId, string codigoTurma, int bimestre)
        {
            CodigoTurma = codigoTurma;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
        }
        public string CodigoTurma { get; set; }
        public string DisciplinaId { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterTotalAlunosSemFrequenciaPorTurmaBimestreQueryValidator : AbstractValidator<ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery>
    {
        public ObterTotalAlunosSemFrequenciaPorTurmaBimestreQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("É necessário informar o código da turma para calcular o total de aulas que não registram frequência.");
            RuleFor(x => x.DisciplinaId).NotEmpty().WithMessage("É necessário informar o id da discplina para calcular o total de aulas que não registram frequência.");
            RuleFor(x => x.Bimestre).NotEmpty().WithMessage("É necessário informar o bimestre para calcular o total de aulas que não registram frequência.");
        }
    }
}
