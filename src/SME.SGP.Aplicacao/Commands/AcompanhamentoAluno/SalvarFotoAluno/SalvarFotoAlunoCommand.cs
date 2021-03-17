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
        public SalvarFotoAlunoCommand(AcompanhamentoAlunoDto acompanhamento, IFormFile file)
        {
            Acompanhamento = acompanhamento;
            File = file;
        }

        public AcompanhamentoAlunoDto Acompanhamento { get; }
        public IFormFile File { get; }
    }

    public class SalvarFotoAlunoCommandValidator : AbstractValidator<SalvarFotoAlunoCommand>
    {
        public SalvarFotoAlunoCommandValidator()
        {
            RuleFor(a => a.Acompanhamento)
                .NotEmpty()
                .WithMessage("Os dados do acompanhamento do aluno deve ser informador para armazenamento da foto");

            RuleFor(a => a.File)
                .NotEmpty()
                .WithMessage("A foto deve ser informada para armazenamento");
        }
    }
}
