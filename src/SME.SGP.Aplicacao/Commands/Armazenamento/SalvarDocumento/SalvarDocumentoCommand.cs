using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarDocumentoCommand : IRequest<bool>
    {
        public SalvarDocumentoCommand(SalvarDocumentoDto salvarDocumentoDto)
        {
            SalvarDocumentoDto = salvarDocumentoDto;

        }

        public SalvarDocumentoDto SalvarDocumentoDto { get; set; }
    }

    public class SalvarDocumentoCommandValidator : AbstractValidator<SalvarDocumentoCommand>
    {
        public SalvarDocumentoCommandValidator()
        {
            RuleFor(c => c.SalvarDocumentoDto.ArquivoCodigo)
            .NotEmpty()
            .WithMessage("O código do arquivo deve ser informado.");

            RuleFor(c => c.SalvarDocumentoDto.UsuarioId)
            .NotEmpty()
            .WithMessage("O id do usuario deve ser informado.");

            RuleFor(c => c.SalvarDocumentoDto.UeId)
            .NotEmpty()
            .WithMessage("O id da UE deve ser informada.");

            RuleFor(c => c.SalvarDocumentoDto.TipoDocumentoId)
            .NotEmpty()
            .WithMessage("O id do tipo de documento deve ser informado.");

            RuleFor(c => c.SalvarDocumentoDto.ClassificacaoId)
            .NotEmpty()
            .WithMessage("O id da classificação deve ser informada.");
        }
    }
}
