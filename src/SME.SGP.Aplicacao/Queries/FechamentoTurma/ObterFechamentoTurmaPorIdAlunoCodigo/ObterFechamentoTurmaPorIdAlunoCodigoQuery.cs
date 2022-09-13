﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdAlunoCodigoQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorIdAlunoCodigoQuery(long fechamentoTurmaId, string alunoCodigo,
            bool ehAnoAnterior = false)
        {
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
            EhAnoAnterior = ehAnoAnterior;
        }

        public long FechamentoTurmaId { get; }
        public string AlunoCodigo { get; }
        public bool EhAnoAnterior { get; }
    }
    
    public class ObterFechamentoTurmaPorIdAlunoCodigoQueryValidator : AbstractValidator<ObterFechamentoTurmaPorIdAlunoCodigoQuery>
    {
        public ObterFechamentoTurmaPorIdAlunoCodigoQueryValidator()
        {
            RuleFor(a => a.FechamentoTurmaId)
                .NotNull()
                .WithMessage("Necessário informar o id para obter o fechamento da turma");

            RuleFor(a => a.AlunoCodigo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Necessário informar o código do aluno para obter o fechamento da turma");
        }
    }    
}
