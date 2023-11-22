using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SME.SGP.TesteIntegracao.Setup;
using SME.SGP.Dominio.Entidades;
using Shouldly;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.TesteIntegracao.Aula.Evento.ServicosFakes;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra;
using System.Collections.Generic;
using Elastic.Apm.Api;

namespace SME.SGP.TesteIntegracao.Evento
{
    public class Ao_excluir_um_evento : AulaTeste
    {
        public const string USUARIO_ADMSME_LOGIN_1111111 = "1111111";
        public const string USUARIO_ADMSME_NOME_1111111 = "USUARIO ADMSME 1111111";
        public Ao_excluir_um_evento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoEventoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Evento - Deve permitir excluir um evento do tipo SME com um usuário ADM SME")]
        public async Task Deve_permitir_excluir_um_evento_SME_com_perfil_ADMSME()
        {
            CriarClaimUsuario(ObterPerfilAdmSme());

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_ADMSME_LOGIN_1111111,
                Login = USUARIO_ADMSME_LOGIN_1111111,
                Nome = USUARIO_ADMSME_NOME_1111111,
                PerfilAtual = Guid.Parse(PerfilUsuario.ADMSME.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new EventoTipo()
            {
                Id = 1,
                Codigo = 1,
                Descricao = "Evento Tipo Teste",
                Letivo = EventoLetivo.Nao,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Calendário Teste Ano Atual",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dominio.Evento()
            {
                Id = 1,
                Nome = "Evento Teste SME 1",
                DreId = null,
                UeId = null,
                TipoEventoId = 1,
                DataFim = new DateTime(DateTime.Now.Year, 1, 2),
                DataInicio = new DateTime(DateTime.Now.Year, 1, 1),
                TipoCalendarioId = 1,
                Status = EntidadeStatus.Aprovado,
                Descricao = "Descrição Evento Teste SME 1",
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            var comandoEvento = ServiceProvider.GetService<IComandosEvento>();

            await comandoEvento.Excluir(new long[] { 1 });

            var eventosCadastrados = ObterTodos<Dominio.Evento>();

            eventosCadastrados.ShouldNotBeNull();
            eventosCadastrados.Count().ShouldBe(1);
            eventosCadastrados.FirstOrDefault().Excluido.ShouldBeTrue();
        }
    }
}
