using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaMockComponentePortugues
    {
        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_sem_periodo_escolar_modalidade_fundamental()
        {
            //não cadastrar tipo calendario e ver se vai retornar isso

            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter os períodos deste tipo de calendário.";

            //await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarItensComuns(false);
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarTurma(Modalidade.Fundamental);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, new DateTime(2022, 02, 10));

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

    }
}