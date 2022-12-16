﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aula.Worker;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_gerar_aula_recorrente_pelo_worker_com_auditoria_administrador : TesteBase
    {
        private ItensBasicosBuilder _buider;
        public Ao_gerar_aula_recorrente_pelo_worker_com_auditoria_administrador(CollectionFixture testFixture) : base(testFixture)
        {
            _buider = new ItensBasicosBuilder(this);
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Deve_gravar_aula_recorrente_pelo_worker_com_auditoria_administrador()
        {
            await _buider.CriaItensComunsEja();
            await _buider.CriaComponenteCurricularSemFrequencia();

            var scope = new WorkerServiceScopeFactoryFake(ServiceProvider);
            var telemetria = ServiceProvider.GetService<IServicoTelemetria>();
            var mensageria = ServiceProvider.GetService<IServicoMensageriaSGP>();
            var connection = new ConnectionFactoryFake();

            var worker = new WorkerRabbitAula(
                scope,
                telemetria,
                mensageria,
                Options.Create(new TelemetriaOptions()),
                Options.Create(new ConsumoFilasOptions()),
                connection);

            var servicoUsuario = ServiceProvider.GetService<IServicoUsuario>();
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var comando = new InserirAulaRecorrenteCommand(usuario,
                                                            new(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                                                            5,
                                                            "1",
                                                            1106,
                                                            "Regência",
                                                            1,
                                                            TipoAula.Normal,
                                                            "1",
                                                            true,
                                                            RecorrenciaAula.AulaUnica);

            var request = new MensagemRabbit(
                                 comando,
                                 Guid.NewGuid(),
                                 usuario.Nome,
                                 usuario.CodigoRf,
                                 usuario.PerfilAtual,
                                 false,
                                 "7924488");

            var basic = new BasicDeliverEventArgs()
            {
                Body = ObtenhaCorpoMensagem(request),
                RoutingKey = RotasRabbitSgpAula.RotaInserirAulaRecorrencia,
                Exchange = ExchangeSgpRabbit.Sgp
            };

            await worker.TratarMensagem(basic);

            var listaDeAuditoria = ObterTodos<Auditoria>();

            listaDeAuditoria.ShouldNotBeEmpty();
            listaDeAuditoria.Exists(auditorio => auditorio.Administrador == "7924488").ShouldBeTrue();
        }

        private ReadOnlyMemory<byte> ObtenhaCorpoMensagem(MensagemRabbit mensagemRabbit)
        {
            var mensagem = JsonConvert.SerializeObject(mensagemRabbit, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return Encoding.UTF8.GetBytes(mensagem);
        }
    }
}
