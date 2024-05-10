using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarFotoEstudanteCommandValidator : AbstractValidator<GerarFotoEstudanteCommand>
    {
        public GerarFotoEstudanteCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O id do aluno deve ser informado para relacionar a foto a ele");

            RuleFor(a => a.ArquivoId)
                .NotEmpty()
                .WithMessage("O id do arquivo da foto deve ser informado para relacionar a foto ao aluno");
        }
    }
}
