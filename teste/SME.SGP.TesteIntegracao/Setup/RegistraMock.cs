﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using System.Collections.Generic;

namespace SME.SGP.TesteIntegracao
{
    public static class RegistraMock
    {
        public static void AddMockPlanoAEE(this IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));
        }

        public static void AddMockAluno(this IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        public static void AddMockFila(this IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>),
                typeof(PublicarFilaSgpCommandHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>),
                typeof(PublicarFilaEmLoteSgpCommandHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>),
                typeof(IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>),
                typeof(IncluirFilaConciliacaoFrequenciaTurmaCommandHandlerFake), ServiceLifetime.Scoped));
        }
    }
}
