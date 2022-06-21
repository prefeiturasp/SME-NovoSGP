using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaTeste
    {
        private const long TIPO_CALENDARIO_FAKE = 999999;

        private DateTime dataInicio = new DateTime(2022, 05, 02);
        private DateTime dataFim = new DateTime(2022, 07, 08);

        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_sem_periodo_escolar_modalidade_fundamental()
        {
            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter os períodos deste tipo de calendário.";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio, false, TIPO_CALENDARIO_FAKE);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_sem_dias_para_incluir_aula_recorrente_modalidade_fundamental()
        {
            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter dias para incluir aulas recorrentes.";
            
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);

            await CriarPeriodoEscolar(new DateTime(2022, 01, 01), new DateTime(2022, 01, 15), BIMESTRE_1, 2);
            
            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio, false, 2);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

        //throw new NegocioException("");

        //var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aulaRecorrente.CodigoTurma));

        //    if(turma == null)
        //        throw new NegocioException("Não foi possível obter a turma para inclusão de aulas recorrentes.");

    }
}