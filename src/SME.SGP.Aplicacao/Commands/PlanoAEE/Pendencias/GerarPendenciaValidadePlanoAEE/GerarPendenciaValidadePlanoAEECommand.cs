using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaValidadePlanoAEECommand : IRequest<bool>
    {
        public GerarPendenciaValidadePlanoAEECommand(long planoAEEId, long usuarioId, string titulo, string descricao)
        {
            PlanoAEEId = planoAEEId;
            UsuarioId = usuarioId;
            Titulo = titulo;
            Descricao = descricao;
        }

        public long PlanoAEEId { get; }
        public long UsuarioId { get; }
        public string Titulo { get; }
        public string Descricao { get; }
    }

    public class GerarPendenciaValidadePlanoAEECommandValidator : AbstractValidator<GerarPendenciaValidadePlanoAEECommand>
    {
        public GerarPendenciaValidadePlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.UsuarioId)
                .NotEmpty()
                .WithMessage("O id do usuário deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.Titulo)
                .NotEmpty()
                .WithMessage("O título deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("A descrição deve ser informada para geração da pendência de validade do plano AEE");
        }
    }
}
