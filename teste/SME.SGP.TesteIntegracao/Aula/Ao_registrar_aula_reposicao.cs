using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaReposicao
{
    public class Ao_registrar_aula_reposicao : AulaMockComponentePortugues
    {
        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);

        public Ao_registrar_aula_reposicao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_especialista_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var aula = ObterAulaPortugues(TipoAula.Reposicao, RecorrenciaAula.AulaUnica);
            aula.Quantidade = 4;

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio, true);
        }

        [Fact]
        public async Task Ao_registrar_aula_reposicao_professor_regente_de_classe_com_aprovacao()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var aula = ObterAulaPortugues(TipoAula.Reposicao, RecorrenciaAula.AulaUnica);
            aula.Quantidade = 2;
            aula.EhRegencia = true;

            await ValideAulaEnviadaParaAprovacao(aula);
        }

        private async Task ValideAulaEnviadaParaAprovacao(PersistirAulaDto aula)
        {
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var retorno = await useCase.Executar(aula);

            retorno.ShouldNotBeNull();

            retorno.Mensagens.Exists(mensagem => mensagem == "Aula cadastrada e enviada para aprovação com sucesso.").ShouldBe(true);

            var aulasCadastradas = ObterTodos<Aula>();

            aulasCadastradas.ShouldNotBeEmpty();
            aulasCadastradas.FirstOrDefault().Status.ShouldBe(EntidadeStatus.AguardandoAprovacao);
        }
    }
}
