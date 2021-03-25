using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoCommand : IRequest<long>
    {
        public GerarAcompanhamentoAlunoCommand(long turmaId, string alunoCodigo)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; }
        public string AlunoCodigo { get; }
    }

    public class SalvarAcompanhamentoAlunoCommandValidator : AbstractValidator<GerarAcompanhamentoAlunoCommand>
    {
        public SalvarAcompanhamentoAlunoCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para geração do acompanhamento do aluno");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para geração do acompanhamento do aluno");
        }
    }
}
