﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarPendenciaCommand : IRequest<long>
    {
        public SalvarPendenciaCommand(TipoPendencia tipoPendencia, long? ueId = null, long? turmaId = null, string descricao = "", string instrucao = "", string titulo = "", string descricaoHtml = "")
        {
            TipoPendencia = tipoPendencia;
            Titulo = titulo;
            Descricao = descricao;
            Instrucao = instrucao;
            DescricaoHtml = descricaoHtml;
            UeId = ueId;
            TurmaId = turmaId;
        }

        public SalvarPendenciaCommand(){}

        public TipoPendencia TipoPendencia { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Instrucao { get; set; }
        public string DescricaoHtml { get; set; }
        public long? UeId { get; set; }
        public string DescricaoComponenteCurricular { get; set; }
        public string TurmaAnoComModalidade { get; set; }
        public string DescricaoUeDre { get; set; }
        public long? TurmaId { get; set; }
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
