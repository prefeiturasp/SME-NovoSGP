using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaPlanoAEECommand : IRequest<bool>
    {
        public GerarPendenciaPlanoAEECommand(long planoAEEId, IEnumerable<long> usuariosIds, string titulo, string descricao, long ueId, PerfilUsuario? perfil = null)
        {
            PlanoAEEId = planoAEEId;
            UsuariosIds = usuariosIds;
            Titulo = titulo;
            Descricao = descricao;
            UeId = ueId;
            Perfil = perfil;
        }

        public GerarPendenciaPlanoAEECommand(long planoAEEId, long usuarioId, string titulo, string descricao, long ueId, PerfilUsuario? perfil = null)
        {
            PlanoAEEId = planoAEEId;
            UsuariosIds = new List<long>() { usuarioId };
            Titulo = titulo;
            Descricao = descricao;
            UeId = ueId;
            Perfil = perfil;
        }

        public long PlanoAEEId { get; }
        public IEnumerable<long> UsuariosIds { get; }
        public string Titulo { get; }
        public string Descricao { get; }
        public long UeId { get; }
        public PerfilUsuario? Perfil { get; set; }
    }

    public class GerarPendenciaValidadePlanoAEECommandValidator : AbstractValidator<GerarPendenciaPlanoAEECommand>
    {
        public GerarPendenciaValidadePlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.Titulo)
                .NotEmpty()
                .WithMessage("O título deve ser informado para geração da pendência de validade do plano AEE");

            RuleFor(a => a.Descricao)
                .NotEmpty()
                .WithMessage("A descrição deve ser informada para geração da pendência de validade do plano AEE");
        }
    }
}
