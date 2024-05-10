using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoFotoCommand : IRequest<long>
    {
        public GerarAcompanhamentoAlunoFotoCommand(long acompanhamentoAlunoSemestreId, long arquivoId, long? miniaturaId = null)
        {
            AcompanhamentoAlunoSemestreId = acompanhamentoAlunoSemestreId;
            ArquivoId = arquivoId;
            MiniaturaId = miniaturaId;
        }

        public long AcompanhamentoAlunoSemestreId { get; }
        public long ArquivoId { get; }
        public long? MiniaturaId { get; }
    }

    public class GerarAcompanhamentoAlunoFotoCommandValidator : AbstractValidator<GerarAcompanhamentoAlunoFotoCommand>
    {
        public GerarAcompanhamentoAlunoFotoCommandValidator()
        {
            RuleFor(a => a.AcompanhamentoAlunoSemestreId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento do aluno no semestre deve ser informado para relacionar a foto a ele");

            RuleFor(a => a.ArquivoId)
                .NotEmpty()
                .WithMessage("O id do arquivo da foto deve ser informado para relacionar a foto ao acompanhamento do aluno no semestre");
        }
    }
}
