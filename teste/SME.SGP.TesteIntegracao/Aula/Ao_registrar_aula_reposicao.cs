using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaReposicao
{
    public class Ao_registrar_aula_reposicao : AulaTeste
    {
        private DateTime dataInicio = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime dataFim = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_registrar_aula_reposicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var aula = ObterAula(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
            aula.Quantidade = 4;

            await CriarPeriodoEscolarEAbertura();

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, dataInicio, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var aula = ObterAula(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, dataInicio);
            aula.Quantidade = 2;
            aula.EhRegencia = true;

            await CriarPeriodoEscolarEAbertura();

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        private async Task ValideAulaEnviadaParaAprovacao(PersistirAulaDto aula)
        {
            await CriarPeriodoEscolarEAbertura();
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var retorno = await useCase.Executar(aula);

            retorno.ShouldNotBeNull();

            retorno.Mensagens.Exists(mensagem => mensagem == "Aula cadastrada e enviada para aprovação com sucesso.").ShouldBe(true);

            var aulasCadastradas = ObterTodos<Aula>();

            aulasCadastradas.ShouldNotBeEmpty();
            aulasCadastradas.FirstOrDefault().Status.ShouldBe(EntidadeStatus.AguardandoAprovacao);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
