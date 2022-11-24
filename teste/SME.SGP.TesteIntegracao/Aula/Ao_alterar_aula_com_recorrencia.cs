using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interface;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaRecorrencia
{
    public class Ao_alterar_aula_com_recorrencia : AulaTeste
    {
        public Ao_alterar_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture) { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IServicoAuditoria),typeof(ServicoAuditoriaFake), ServiceLifetime.Scoped));
            
        }

        [Fact]
        public async Task Altera_quantidade_de_aulas_com_recorrencia_no_bimestre_atual()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, 138, DATA_02_05);

            aula.DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 27);

            aula.Id = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);

            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();

            listaNotificao.FirstOrDefault().Mensagem.ShouldContain("Foram alteradas 2 aulas do componente curricular Língua Portuguesa para a turma Turma Nome 1 da Nome da UE (DRE 1).");
        }

        [Fact]
        public async Task Altera_quantidade_de_aulas_com_recorrencia_para_todos_bimestres()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirTodosBimestres);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            
            var aula = ObterAula(TipoAula.Normal, RecorrenciaAula.RepetirTodosBimestres, 138, DATA_02_05);
            
            aula.DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 26);
            
            aula.Id = 1;

            await CriarPeriodoEscolarEAbertura();

            var retorno = await usecase.Executar(aula);
            
            var listaNotificao = ObterTodos<Notificacao>();

            retorno.ShouldNotBeNull();

            listaNotificao.ShouldNotBeEmpty();

            listaNotificao.FirstOrDefault().Mensagem.ShouldContain("Foram alteradas 17 aulas do componente curricular Língua Portuguesa para a turma Turma Nome 1 da Nome da UE (DRE 1).");

        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_28_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
