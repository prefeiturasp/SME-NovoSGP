using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaFuturaTerritorio
{
    public class Ao_excluir_aula_territorio_disponibilizado : AulaTeste
    {
        public Ao_excluir_aula_territorio_disponibilizado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeExcluirAulaUseCase), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>), typeof(PublicarFilaEmLoteSgpCommandFakeExcluirAulaUseCase), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Aula - Deve permitir excluir aula futura com componente curricular território disponibilizado e seu plano")]
        public async Task Exclui_aula_futura_com_plano()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, 
                                        DateTimeExtension.HorarioBrasilia().Date,
                                        DateTimeExtension.HorarioBrasilia().Date.AddDays(30), BIMESTRE_2, false);
            await CriarAula(COMPONENTE_CURRICULAR_TERRITORIO_SABER_1_ID_1214.ToString(), DateTimeExtension.HorarioBrasilia().Date.AddDays(15), RecorrenciaAula.AulaUnica);
            await CriarPlanoAula();

            var excluirAulaUseCase = ServiceProvider.GetService<IExcluirAulasRecorrentesTerritorioSaberUseCase>();
            var excluirAulaDto = ObterExcluirAularFuturaTerritorioSaberDto(DateTimeExtension.HorarioBrasilia().Date,
                                                                          TURMA_CODIGO_1, COMPONENTE_CURRICULAR_TERRITORIO_SABER_1_ID_1214.ToString());

            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(excluirAulaDto),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
            var retorno = await excluirAulaUseCase.Executar(mensagem);

            retorno.ShouldBeTrue();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Excluido.ShouldBe(true);

            var planosAula = ObterTodos<Dominio.PlanoAula>();
            planosAula.ShouldNotBeEmpty();
            planosAula.FirstOrDefault().Excluido.ShouldBe(true);
        }

        private async Task CriarPlanoAula()
        {
            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
