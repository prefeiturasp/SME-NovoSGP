using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoSemestreCommand : IRequest<AcompanhamentoAlunoSemestre>
    {
        public GerarAcompanhamentoAlunoSemestreCommand(long acompanhamentoAlunoId, int semestre, string observacoes)
        {
            AcompanhamentoAlunoId = acompanhamentoAlunoId;
            Semestre = semestre;
            Observacoes = observacoes;
        }

        public long AcompanhamentoAlunoId { get; }
        public int Semestre { get; }
        public string Observacoes { get; }
    }

    public class GerarAcompanhamentoAlunoSemestreCommandValidator : AbstractValidator<GerarAcompanhamentoAlunoSemestreCommand>
    {
        public GerarAcompanhamentoAlunoSemestreCommandValidator()
        {
            RuleFor(a => a.AcompanhamentoAlunoId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento aluno deve ser informado para geração do registro do semestre");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre do acompanhamento aluno deve ser informado para geração do registro");
        }
    }
}
