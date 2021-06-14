using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoTurmaCommand : IRequest<AcompanhamentoTurma>
    {
        public GerarAcompanhamentoTurmaCommand(long turmaId, int semestre, string apanhadoGeral)
        {
            TurmaId = turmaId;
            Semestre = semestre;
            ApanhadoGeral = apanhadoGeral;
        }

        public long TurmaId { get; }
        public int Semestre { get; }
        public string ApanhadoGeral { get; }
    }

    public class GerarAcompanhamentoTurmaCommandValidator : AbstractValidator<GerarAcompanhamentoTurmaCommand>
    {
        public GerarAcompanhamentoTurmaCommandValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado para geração do registro");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre do acompanhamento da turma deve ser informado para geração do registro");
        }
    }
}
