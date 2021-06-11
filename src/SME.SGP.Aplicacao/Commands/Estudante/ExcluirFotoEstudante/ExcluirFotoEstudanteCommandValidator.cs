using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoEstudanteCommandValidator : AbstractValidator<ExcluirFotoEstudanteCommand>
    {
        public ExcluirFotoEstudanteCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do estudante deve ser enviado");
        }
    }
}
