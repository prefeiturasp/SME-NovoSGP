using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaBimestreAtual
{
    public class Ao_registrar_aula_normal_repetir_bimestre_atual : AulaTeste
    {
        private const long TIPO_CALENDARIO_999999 = 999999;
        private const long TIPO_CALENDARIO_1 = 1;

        private readonly DateTime DATA_02_05_2022 = new(2022, 05, 02);
        private readonly DateTime DATA_08_07_2022 = new(2022, 07, 08);
        private readonly DateTime DATA_03_10_2022 = new(2022, 10, 03);
        private readonly DateTime DATA_19_06_2022 = new(2022, 06, 19);        

        public Ao_registrar_aula_normal_repetir_bimestre_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Ao_registrar_aula_normal_sem_periodo_escolar()
        {
            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter os períodos deste tipo de calendário.";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_2);

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05_2022, false, TIPO_CALENDARIO_999999);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_no_dia_quando_existe_aula_normal()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            await InserirNaBase(new Aula() 
            {
                AulaCJ = false,
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = 1,
                DataAula = DATA_02_05_2022,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Status = EntidadeStatus.Aprovado
            });

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05_2022, false, TIPO_CALENDARIO_999999));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_no_dia_quando_existe_aula_reposicao()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Reposição' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            await InserirNaBase(new Aula()
            {
                AulaCJ = false,
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = 1,
                DataAula = DATA_02_05_2022,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Reposicao,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Status = EntidadeStatus.Aprovado
            });

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05_2022, false, TIPO_CALENDARIO_999999));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_no_dia_quando_quando_nao_encontra_tipo_calendario()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_03_10_2022,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_reposicao_no_dia_quando_quando_nao_encontra_tipo_calendario()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Reposição' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_03_10_2022,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_quando_eh_final_de_semana_nao_tem_evento_letivo_dia()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES, 
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06_2022,
                                      DATA_19_06_2022,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_19_06_2022,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_reposicao_quando_eh_final_de_semana_nao_tem_evento_letivo_dia()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Reposição' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05_2022, DATA_08_07_2022, BIMESTRE_1);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES,
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06_2022,
                                      DATA_19_06_2022,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_19_06_2022,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }
        //[Fact]
        //public async Task Ao_registrar_aula_normal_repetir_no_bimestre_atual_professor_sem_dias_para_incluir_aula_recorrente_modalidade_fundamental()
        //{
        //    Não conseguido validar essa exceçõa em função da condição 'fimRecorrencia = periodos.Where(a => a.PeriodoFim >= request.DataInicio)' nesse ObterFimPeriodoRecorrenciaQueryHandler  
        //    var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter dias para incluir aulas recorrentes.";

        //    await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);

        //    await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);

        //    await CriarPeriodoEscolar(new DateTime(2022, 01, 01), new DateTime(2022, 01, 15), BIMESTRE_1, 2);

        //    var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataInicio, false, 2);

        //    excecao.ExistemErros.ShouldBeTrue();

        //    excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

        //    excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        //}
    }
}