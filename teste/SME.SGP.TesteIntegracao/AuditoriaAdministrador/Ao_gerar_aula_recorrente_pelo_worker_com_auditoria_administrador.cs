using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.TesteIntegracao.Setup;
using SME.SGP.Worker.RabbitMQ;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_gerar_aula_recorrente_pelo_worker_com_auditoria_administrador : TesteBase
    {
        private ItensBasicosBuilder _buider;
        public Ao_gerar_aula_recorrente_pelo_worker_com_auditoria_administrador(CollectionFixture testFixture) : base(testFixture)
        {
            _buider = new ItensBasicosBuilder(this);
        }

        public async Task Deve_gravar_aula_recorrente_pelo_worker_com_auditoria_administrador()
        {
            await _buider.CriaItensComunsEja();
            await _buider.CriaComponenteCurricularSemFrequencia();

            var scope = ServiceProvider.GetService<IServiceScopeFactory>(); 
            var telemetria = ServiceProvider.GetService<IServicoTelemetria>();
            var connection = new ConnectionFactoryFake();

            var worker = new WorkerRabbitMQ(
                                scope,
                                telemetria,
                                new TelemetriaOptions(),
                                new ConsumoFilasOptions() { Padrao = true },
                                connection);

            var servicoUsuario = ServiceProvider.GetService<IServicoUsuario>();
            var usuario = await servicoUsuario.ObterUsuarioLogado();

            var comando = new InserirAulaRecorrenteCommand(usuario,
                                                            new DateTime(2022, 02, 10),
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

            Reconectar();

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
