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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaReposicao
{
    public class Ao_registrar_aula_reposicao : AulaTeste
    {
        private const string TITULO_NOTIFICACAO = "Notificação Teste de Integração.";
        private const string MENSAGEM_NOTIFICACAO = "Mensagem notificação Teste de Integração.";
        private const string SISTEMA = "Sistema";

        public Ao_registrar_aula_reposicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorCargoUeQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorCargoUeQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            var aula = ObterAula(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            aula.Quantidade = 4;

            await CriarPeriodoEscolarEAbertura();
            await CriarNotificacao();

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_02_05, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);

            var aula = ObterAula(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213, DATA_02_05);

            aula.Quantidade = 2;

            aula.EhRegencia = true;

            await CriarPeriodoEscolarEAbertura();
            await CriarNotificacao();

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        private async Task ValideAulaEnviadaParaAprovacao(PersistirAulaDto aula)
        {
            await CriarPeriodoEscolarEAbertura();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var retorno = await useCase.Executar(aula);

            retorno.ShouldNotBeNull();

            retorno.Mensagens.Exists(mensagem => mensagem == "Aula cadastrada e enviada para aprovação com sucesso.").ShouldBe(true);

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.ShouldNotBeEmpty();

            aulasCadastradas.FirstOrDefault().Status.ShouldBe(EntidadeStatus.AguardandoAprovacao);
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        private async Task CriarNotificacao()
        {
            await InserirNaBase(new Notificacao()
            {
                Id = 1,
                Titulo = TITULO_NOTIFICACAO,
                Mensagem = MENSAGEM_NOTIFICACAO,
                Status = NotificacaoStatus.Pendente,
                Categoria = NotificacaoCategoria.Aviso,
                Tipo = NotificacaoTipo.Calendario,
                Codigo = 1,
                Excluida = false,
                UsuarioId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA
            });
        }
    }
}
