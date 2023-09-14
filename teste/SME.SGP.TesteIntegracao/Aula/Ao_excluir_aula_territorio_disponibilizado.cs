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

        }

        [Fact(DisplayName = "Aula - Deve permitir excluir aula futura com componente curricular território disponibilizado")]
        public async Task Exclui_aula_futura()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, 
                                        DateTimeExtension.HorarioBrasilia().Date,
                                        DateTimeExtension.HorarioBrasilia().Date.AddDays(30), BIMESTRE_2, false);
            await CriarAula(COMPONENTE_CURRICULAR_TERRITORIO_SABER_1_ID_1214.ToString(), DateTimeExtension.HorarioBrasilia().Date.AddDays(15), RecorrenciaAula.AulaUnica);

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
        }
    }
}
