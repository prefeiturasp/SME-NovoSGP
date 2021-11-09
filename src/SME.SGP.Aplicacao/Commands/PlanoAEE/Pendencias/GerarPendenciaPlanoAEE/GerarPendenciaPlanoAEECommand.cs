using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPlanoAEECommand : IRequest<bool>
    {
        public GerarPendenciaPlanoAEECommand(long planoAEEId, IEnumerable<long> usuariosIds, string titulo, string descricao, int perfil = 0)
        {
            PlanoAEEId = planoAEEId;
            UsuariosIds = usuariosIds;
            Titulo = titulo;
            Descricao = descricao;
            Perfil = perfil;
        }

        public GerarPendenciaPlanoAEECommand(long planoAEEId, long usuarioId, string titulo, string descricao, int perfil = 0)
        {
            PlanoAEEId = planoAEEId;
            UsuariosIds = new List<long>() { usuarioId };
            Titulo = titulo;
            Descricao = descricao;
            Perfil = perfil;
        }

        public long PlanoAEEId { get; }
        public IEnumerable<long> UsuariosIds { get; }
        public string Titulo { get; }
        public string Descricao { get; }
        public int Perfil { get; set; }
    }

    public class GerarPendenciaValidadePlanoAEECommandValidator : AbstractValidator<GerarPendenciaPlanoAEECommand>
    {
        public GerarPendenciaValidadePlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.UsuariosIds)
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
