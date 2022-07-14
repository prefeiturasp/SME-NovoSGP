using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual_sem_permissao : AulaTeste
    {
        private DateTime DATA_02_05 = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime DATA_08_07 = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_registrar_aula_normal_repetir_bimestre_atual_sem_permissao(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_nao_pode_fazer_alteracoes_modalidade_fundamental()
        {
            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

    }
}