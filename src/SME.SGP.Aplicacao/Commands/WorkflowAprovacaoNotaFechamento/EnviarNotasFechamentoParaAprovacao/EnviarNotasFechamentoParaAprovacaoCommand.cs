using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotasFechamentoParaAprovacaoCommand : IRequest
    {
        public EnviarNotasFechamentoParaAprovacaoCommand(List<FechamentoNotaDto> notasAprovacao, Usuario usuarioLogado)
        {
            NotasAprovacao = notasAprovacao;
            Usuario = usuarioLogado;
        }

        public List<FechamentoNotaDto> NotasAprovacao { get; }
        public Usuario Usuario { get; set; }
    }

    public class EnviarNotasFechamentoParaAprovacaoCommandValidator : AbstractValidator<EnviarNotasFechamentoParaAprovacaoCommand>
    {
        public EnviarNotasFechamentoParaAprovacaoCommandValidator()
        {
            RuleFor(a => a.NotasAprovacao)
                .NotEmpty()
                .WithMessage("Necessário informar as notas para envio para aprovação");

            RuleFor(a => a.Usuario)
                .NotEmpty()
                .WithMessage("Necessário informar o usuário para envio para aprovação");
        }
    }
}
