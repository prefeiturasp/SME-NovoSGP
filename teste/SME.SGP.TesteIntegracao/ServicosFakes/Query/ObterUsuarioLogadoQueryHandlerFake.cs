﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes.Query
{
    public class ObterUsuarioLogadoQueryHandlerFake : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {
        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var usuario = new Usuario()
            {
                Id = 1,
                CodigoRf = "7111111",
                Login = "7111111",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01)
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>()
            {
                new PrioridadePerfil() { CodigoPerfil = Perfis.PERFIL_PROFESSOR }
            });

            return await Task.FromResult(usuario);
        }
    }
}
