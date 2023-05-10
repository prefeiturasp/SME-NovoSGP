using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class OtimizarArquivosCommand : IRequest<bool>
    {
        public OtimizarArquivosCommand(string nomeArquivo)
        {
            NomeArquivo = nomeArquivo;
        }

        public string NomeArquivo { get; }
    }

    public class OtimizarArquivosCommandValidator : AbstractValidator<OtimizarArquivosCommand>
    {
        public OtimizarArquivosCommandValidator()
        {
            RuleFor(a => a.NomeArquivo)
                .NotEmpty()
                .WithMessage("O nome do arquivo deve ser informado para relizar a otimização");
        }
    }
}
