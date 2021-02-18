using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCommand : IRequest<long>
    {
        public SalvarPendenciaCommand(TipoPendencia tipoPendencia, string descricao = "", string instrucao = "", string titulo = "")
        {
            TipoPendencia = tipoPendencia;
            Titulo = titulo;
            Descricao = descricao;
            Instrucao = instrucao;
        }

        public TipoPendencia TipoPendencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Instrucao { get; set; }
    }

    public class SalvarPendenciaCommandValidator : AbstractValidator<SalvarPendenciaCommand>
    {
        public SalvarPendenciaCommandValidator()
        {
            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado para geração de pendência.");

        }
    }
}
