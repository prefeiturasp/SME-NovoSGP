using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAcompanhamentoAlunoCommand : IRequest<AuditoriaDto>
    {
        public SalvarFotoAcompanhamentoAlunoCommand(AcompanhamentoAlunoDto acompanhamento)
        {
            Acompanhamento = acompanhamento;
        }

        public AcompanhamentoAlunoDto Acompanhamento { get; }
    }

    public class SalvarFotoAlunoAcompanhamentoCommandValidator : AbstractValidator<SalvarFotoAcompanhamentoAlunoCommand>
    {
        public SalvarFotoAlunoAcompanhamentoCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("Os dados do acompanhamento do aluno deve ser informador para armazenamento da foto");
        }
    }
}
