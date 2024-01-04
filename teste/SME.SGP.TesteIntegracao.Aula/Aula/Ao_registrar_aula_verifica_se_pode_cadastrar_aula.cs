using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Aula.Aula.ServicosFake;
using SME.SGP.TesteIntegracao.Aula.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace SME.SGP.TesteIntegracao.PodeCadastrarAula
{
    public class Ao_registrar_aula_verifica_se_pode_cadastrar_aula : AulaTeste
    {
        private const long TIPO_CALENDARIO_999999 = 999999;
        private const string NOME_TOKEN_TESTE = "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiUHJvZmVzc29yIiwiSXNzdWVyIjoiSXNzdWVyIiwiVXNlcm5hbWUiOiJUZXN0ZVByb2Zlc3NvciIsImlhdCI6MTY3MDUyNTQyMSwicGVyZmlsIjoiNDBFMUUwNzQtMzdENi1FOTExLUFCRDYtRjgxNjU0RkU4OTVEIn0.da2rLBX_esGtdudii1bZpSG6SQK264bQ_oKm74BkA6M";

        private readonly DateTime DATA_19_06 = new(DateTimeExtension.HorarioBrasilia().Year, 06, 19);

        public Ao_registrar_aula_verifica_se_pode_cadastrar_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>), typeof(ObterProfessorTitularPorTurmaComponenteCurricularQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAulasDaTurmaPorTipoCalendarioQuery, IEnumerable<Dominio.Aula>>), typeof(ObterAulasDaTurmaPorTipoCalendarioQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_sem_periodo_escolar()
        {
            var mensagemEsperada = "Ocorreu um erro ao solicitar a criação de aulas recorrentes, por favor tente novamente. Detalhes: Não foi possível obter os períodos deste tipo de calendário.";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05, false, TIPO_CALENDARIO_999999);

            excecao.ExistemErros.ShouldBeTrue();

            excecao.Mensagens.FirstOrDefault().ShouldNotBeNullOrEmpty();

            excecao.Mensagens.FirstOrDefault().ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_quando_existe_aula_normal()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            CriarClaimProfessorComToken();

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = 1,
                DataAula = DATA_02_05,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Status = EntidadeStatus.Aprovado
            });

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05, false, TIPO_CALENDARIO_999999));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        private void CriarClaimProfessorComToken()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("login", USUARIO_PROFESSOR_LOGIN_2222222);
            variaveis.Add("TokenAtual", NOME_TOKEN_TESTE);
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = USUARIO_PROFESSOR_CODIGO_RF_2222222, Type = "rf" },
                new InternalClaim { Value = PerfilUsuario.PROFESSOR.Name(), Type = "perfil" },
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_reposicao_quando_existe_aula_reposicao()
        {
            var mensagemEsperada = "Não é possível cadastrar aula de reposição com recorrência!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = 1,
                DataAula = DATA_02_05,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Reposicao,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Status = EntidadeStatus.Aprovado
            });

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao, RecorrenciaAula.RepetirBimestreAtual, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05, false, TIPO_CALENDARIO_999999));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_quando_nao_encontra_tipo_calendario()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_03_10,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_reposicao_quando_nao_encontra_tipo_calendario()
        {
            var mensagemEsperada = "Não é possível cadastrar aula de reposição com recorrência!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_03_10,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_normal_quando_nao_tem_evento_letivo_dia()
        {
            var mensagemEsperada = "Não é possível cadastrar aula do tipo 'Normal' para o dia selecionado!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES,
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_19_06,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Nao_pode_cadastrar_aula_reposicao_quando_nao_tem_evento_letivo_dia()
        {
            var mensagemEsperada = "Não é possível cadastrar aula de reposição com recorrência!";

            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES,
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarPeriodoEscolarEAbertura();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => InserirAulaUseCaseSemValidacaoBasica(TipoAula.Reposicao,
                                                                     RecorrenciaAula.RepetirBimestreAtual,
                                                                     COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                                                                     DATA_19_06,
                                                                     TIPO_CALENDARIO_1,
                                                                     TURMA_CODIGO_1,
                                                                     UE_CODIGO_1));

            excecao.Message.ShouldNotBeNullOrEmpty();

            excecao.Message.ShouldBeEquivalentTo(mensagemEsperada);
        }

        [Fact]
        public async Task Pode_cadastrar_aula_reposicao_quando_nao_tem_evento_letivo_dia_mas_tem_evento_reposicao_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES,
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarEventoTipoResumido(REPOSICAO_AULA,
                                          EventoLocalOcorrencia.UE,
                                          true,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_13);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_2,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await PodeCadastrarAulaUseCase(TipoAula.Reposicao, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_19_06);

            retorno.PodeCadastrarAula.ShouldBeTrue();
        }

        [Fact]
        public async Task Pode_cadastrar_aula_reposicao_quando_nao_tem_evento_letivo_dia_mas_tem_evento_reposicao_dia()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_1, false);

            await CriarEventoTipoResumido(SUSPENSAO_DE_ATIVIDADES,
                                          EventoLocalOcorrencia.SMEUE,
                                          false,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Nao,
                                          TIPO_EVENTO_21);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Nao,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_1,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarEventoTipoResumido(REPOSICAO_DIA,
                                          EventoLocalOcorrencia.UE,
                                          true,
                                          EventoTipoData.Unico,
                                          false,
                                          EventoLetivo.Sim,
                                          TIPO_EVENTO_14);

            await CriarEventoResumido(EVENTO_NAO_LETIVO,
                                      DATA_19_06,
                                      DATA_19_06,
                                      EventoLetivo.Sim,
                                      TIPO_CALENDARIO_1,
                                      TIPO_EVENTO_2,
                                      DRE_CODIGO_1,
                                      UE_CODIGO_1,
                                      EntidadeStatus.Aprovado);

            await CriarPeriodoEscolarEAbertura();

            var retorno = await PodeCadastrarAulaUseCase(TipoAula.Reposicao, TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_19_06);

            retorno.PodeCadastrarAula.ShouldBeTrue();
        }
        [Fact]
        public async Task Nao_Pode_Cadastrar_Aula_Infantil_E_Regencia_Automaticamente_Em_FimDeSemana()
        {
            var mensagemRabbit = await ObterMensagemRabbit();

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);

            var mediator = ServiceProvider.GetService<ICriarAulasInfantilERegenciaUseCase>();
            await mediator.Executar(mensagemRabbit);

            var aulaCriada = ObterTodos<Dominio.Aula>();

            aulaCriada.Count().ShouldBe(0);
        }

        [Fact]
        public async Task Verifica_aula_automatica_fim_de_semana_com_frequencia_lancada_e_evento_letivo_nao_ser_excluída_por_exceção_unica()
        {
            var mensagemRabbit = await ObterMensagemRabbit2();

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);

            var mediator = ServiceProvider.GetService<ICriarAulasInfantilERegenciaUseCase>();
            await mediator.Executar(mensagemRabbit);

            var aulaExistente = ObterTodos<Dominio.Aula>();
            aulaExistente.ShouldNotBeNull();
            aulaExistente.FirstOrDefault(a => a.DataAula.Day == 16).DataAula.FimDeSemana().ShouldBeTrue();

        }

        private async Task<MensagemRabbit> ObterMensagemRabbit()
        {
            var diasLetivos = new DiaLetivoDto
            {
                Data = DATA_16_09,
                Motivo = "REUNIAO",
                EhLetivo = true,
                PossuiEvento = true,
            };

            var diasLetivosList = new List<DiaLetivoDto> { diasLetivos };

            var dadosDisciplina = (codigo: COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), nome: COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105);

            var dadosAulaCriadaAutomaticamente = new DadosAulaCriadaAutomaticamenteDto(dadosDisciplina);
            var diasForaPeriodoEscolar = new List<DateTime> { DATA_20_07, DATA_21_07, DATA_22_07, DATA_23_07, DATA_01_10 };
            var codigoDisciplinaConsiderada = new List<string> { COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString() };
            var turma = new Dominio.Turma
            {
                Ano = ANO_9,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                CodigoTurma = TURMA_CODIGO_1,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                Ue = new Ue { CodigoUe = UE_CODIGO_1, Dre = new Dre { CodigoDre = DRE_CODIGO_1 } }
            };

            var mensagemRabbitObjeto = new ObjetoRabbit
            {
                DiasLetivos = diasLetivosList,
                Turma = turma,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DiasForaDoPeriodoEscolar = diasForaPeriodoEscolar,
                CodigosDisciplinasConsideradas = codigoDisciplinaConsiderada,
                DadosAulaCriadaAutomaticamente = dadosAulaCriadaAutomaticamente
            };

            var mensagemRabbitJson = JsonConvert.SerializeObject(mensagemRabbitObjeto);

            return await Task.FromResult(new MensagemRabbit
            {
                Mensagem = mensagemRabbitJson
            });
        }

        private async Task<MensagemRabbit> ObterMensagemRabbit2()
        {
            var diasLetivos = new List<DiaLetivoDto>()
            {
               new DiaLetivoDto(){
                    Data = DATA_16_09,
                    Motivo = "REUNIAO",
                    EhLetivo = true,
                    PossuiEvento = true
               },
               new DiaLetivoDto(){
                    Data = DATA_01_10,
                    Motivo = "",
                    EhLetivo = false,
                    PossuiEvento = false
               },
               new DiaLetivoDto(){
                    Data = DATA_03_10,
                    Motivo = "",
                    EhLetivo = true,
                    PossuiEvento = false
               },
            };

            var dadosDisciplina = (codigo: COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), nome: COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105);

            var dadosAulaCriadaAutomaticamente = new DadosAulaCriadaAutomaticamenteDto(dadosDisciplina);
            var diasForaPeriodoEscolar = new List<DateTime> { DATA_20_07, DATA_21_07, DATA_22_07, DATA_23_07, DATA_01_10 };
            var codigoDisciplinaConsiderada = new List<string> { COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString() };
            var turma = new Dominio.Turma
            {
                Ano = ANO_9,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                CodigoTurma = TURMA_CODIGO_1,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                Ue = new Ue { CodigoUe = UE_CODIGO_1, Dre = new Dre { CodigoDre = DRE_CODIGO_1 } },
                DataInicio = DATA_01_01
            };

            var mensagemRabbitObjeto = new ObjetoRabbit
            {
                DiasLetivos = diasLetivos,
                Turma = turma,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DiasForaDoPeriodoEscolar = diasForaPeriodoEscolar,

                CodigosDisciplinasConsideradas = codigoDisciplinaConsiderada,
                DadosAulaCriadaAutomaticamente = dadosAulaCriadaAutomaticamente
            };

            await InserirNaBase(new TipoCalendario()
            {
                Id = 1,
                AnoLetivo = DateTime.Now.Year,
                Nome = "tipo cal infantil",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Situacao = true,
                CriadoEm = DATA_01_01,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.Aula
            {
                Id = 1,
                DataAula = DATA_16_09,
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(),
                DadosComplementares = new AulaDadosComplementares() { PossuiFrequencia = true },
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                Quantidade = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal
            });
            var mensagemRabbitJson = JsonConvert.SerializeObject(mensagemRabbitObjeto);

            return await Task.FromResult(new MensagemRabbit
            {
                Mensagem = mensagemRabbitJson
            });
        }

        private class ObjetoRabbit
        {
            public long TipoCalendarioId { get; set; }
            public IEnumerable<DiaLetivoDto> DiasLetivos { get; set; }
            public Dominio.Turma Turma { get; set; }
            public IEnumerable<DateTime> DiasForaDoPeriodoEscolar { get; set; }
            public IEnumerable<string> CodigosDisciplinasConsideradas { get; set; }
            public DadosAulaCriadaAutomaticamenteDto DadosAulaCriadaAutomaticamente { get; set; }

        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}