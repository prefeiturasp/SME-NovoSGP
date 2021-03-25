using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoAlunoCommand : IRequest<AuditoriaDto>
    {
        public SalvarFotoAlunoCommand(AcompanhamentoAlunoDto acompanhamento)
        {
            Acompanhamento = acompanhamento;
        }

        public AcompanhamentoAlunoDto Acompanhamento { get; }
    }

    public class SalvarFotoAlunoCommandValidator : AbstractValidator<SalvarFotoAlunoCommand>
    {
        public SalvarFotoAlunoCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("Os dados do acompanhamento do aluno deve ser informador para armazenamento da foto");
        }
    }
}
