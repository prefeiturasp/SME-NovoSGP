using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarArquivoRepositorioCommand : IRequest<ArquivoArmazenadoDto>
    {
        public SalvarArquivoRepositorioCommand(string nomeArquivo, TipoArquivo tipo, string tipoConteudo)
        {
            NomeArquivo = nomeArquivo;
            Tipo = tipo;
            TipoConteudo = tipoConteudo;
        }

        public string NomeArquivo { get; set; }
        public TipoArquivo Tipo { get; set; }
        public string TipoConteudo { get; set; }
    }

    public class SalvarArquivoRepositorioCommandValidator : AbstractValidator<SalvarArquivoRepositorioCommand>
    {
        public SalvarArquivoRepositorioCommandValidator()
        {
            RuleFor(c => c.NomeArquivo)
            .NotEmpty()
            .WithMessage("O Nome do Arquivo deve ser informado para registrar no repositório.");

            RuleFor(c => c.Tipo)
            .NotEmpty()
            .WithMessage("O Tipo do Arquivo deve ser informado para registrar no repositório.");
        }
    }
}
