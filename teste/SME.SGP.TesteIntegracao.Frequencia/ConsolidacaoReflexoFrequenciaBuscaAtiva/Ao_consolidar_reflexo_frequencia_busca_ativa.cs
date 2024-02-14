using Bogus.DataSets;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.ConsolidacaoFrequenciaMensal.ServicosFakes;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.ConsolidacaoReflexoFrequenciaBuscaAtiva
{
    public class Ao_consolidar_reflexo_frequencia_busca_ativa : FrequenciaTesteBase
    {
        private readonly IExecutor ExecutorComandos;
        private const long ID_REGISTRO_BUSCA_ATIVA_ALUNO_1 = 1;
        private const long ID_REGISTRO_BUSCA_ATIVA_ALUNO_2 = 2;

        public Ao_consolidar_reflexo_frequencia_busca_ativa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            ExecutorComandos = ServiceProvider.GetRequiredService<IExecutor>();
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        }

        [Fact(DisplayName = "Ao consolidar frequência de turma mensal do mês")]
        public async Task Ao_consolidar_frequencia_turma_mensal_do_mes()
        {
            await CriarDreUePerfilComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Medio, ANO_5, TURMA_CODIGO_1);
            ExecutorComandos.SetarComando(new PublicarQuestionarioBuscaAtivaComando(this));
            await ExecutorComandos.ExecutarComando();
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriaAulaFrequencia(AULA_ID_1, new(dataReferencia.Year, 5, 01), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_2, new(dataReferencia.Year, 5, 05), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_3, new(dataReferencia.Year, 5, 10), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_4, new(dataReferencia.Year, 5, 15), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_5, new(dataReferencia.Year, 5, 16), TipoFrequencia.C, TipoFrequencia.F);
            await CriaAulaFrequencia(AULA_ID_6, new(dataReferencia.Year, 5, 20), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_7, new(dataReferencia.Year, 5, 25), TipoFrequencia.F, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_8, new(dataReferencia.Year, 5, 28), TipoFrequencia.R, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_9, new(dataReferencia.Year, 6, 01), TipoFrequencia.C, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_10, new(dataReferencia.Year, 6, 05), TipoFrequencia.C, TipoFrequencia.C);
            await CriaAulaFrequencia(AULA_ID_11, new(dataReferencia.Year, 6, 10), TipoFrequencia.C, TipoFrequencia.C);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_1);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_2);

            var useCase = ServiceProvider.GetService<IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase>();
            //Consolidação Mês 5 mensal e anual
            await ConsolidarReflexoFrequencia(useCase, ID_REGISTRO_BUSCA_ATIVA_ALUNO_1, new(dataReferencia.Year, 5, 28));
            await ConsolidarReflexoFrequencia(useCase, ID_REGISTRO_BUSCA_ATIVA_ALUNO_2, new(dataReferencia.Year, 5, 28));

            //Consolidação Mês 6 somente anual pois a ação busca ativa é do mês 5
            await ConsolidarReflexoFrequencia(useCase, ID_REGISTRO_BUSCA_ATIVA_ALUNO_1, new(dataReferencia.Year, 6, 10));
            await ConsolidarReflexoFrequencia(useCase, ID_REGISTRO_BUSCA_ATIVA_ALUNO_2, new(dataReferencia.Year, 6, 10));

            var consolidacao = ObterTodos<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>();
            consolidacao.ShouldNotBeEmpty();
            consolidacao.Count.ShouldBe(4);
            consolidacao.Where(c => c.Mes == 5 && c.AlunoCodigo == ALUNO_CODIGO_1).All(c => c.PercFrequenciaAntesAcao == 100
                                                                                            && c.PercFrequenciaAposAcao == 75).ShouldBe(true);
            consolidacao.Where(c => c.Mes == 5 && c.AlunoCodigo == ALUNO_CODIGO_2).All(c => c.PercFrequenciaAntesAcao == 0
                                                                                            && c.PercFrequenciaAposAcao == 37.5).ShouldBe(true);

            consolidacao.Where(c => c.Mes == 0 && c.AlunoCodigo == ALUNO_CODIGO_1).All(c => c.PercFrequenciaAntesAcao == 100
                                                                                            && c.PercFrequenciaAposAcao == 81.82).ShouldBe(true);
            consolidacao.Where(c => c.Mes == 0 && c.AlunoCodigo == ALUNO_CODIGO_2).All(c => c.PercFrequenciaAntesAcao == 0
                                                                                            && c.PercFrequenciaAposAcao == 54.55).ShouldBe(true);
        }

        private async Task ConsolidarReflexoFrequencia(IConsolidarReflexoFrequenciaBuscaAtivaAlunoUseCase useCase, long idRegistroBuscaAtiva, DateTime dataRegistro)
        {
            var mensagem = new FiltroIdAnoLetivoDto(idRegistroBuscaAtiva, dataRegistro);
            var jsonMensagem = JsonSerializer.Serialize(mensagem);
            await useCase.Executar(new MensagemRabbit(jsonMensagem));
        }

        private async Task CriarRegistroAcaoEstudante(DateTime dataRegistroAcao, string codigoAluno)
        {
            var idRegistroAcao = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>().Count() + 1;
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtiva()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = codigoAluno,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RegistroAcaoBuscaAtivaSecao()
            {
                RegistroAcaoBuscaAtivaId = idRegistroAcao,
                SecaoRegistroAcaoBuscaAtivaId = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });

            await InserirNaBase(new QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcao,
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var idQuestaoRegistroAcao = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>().Count();
            await InserirNaBase(new RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idQuestaoRegistroAcao,
                Texto = dataRegistroAcao.ToString("yyyy-MM-dd"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcao,
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            idQuestaoRegistroAcao = ObterTodos<QuestaoRegistroAcaoBuscaAtiva>().Count();
            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP
                                                         && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_SIM).FirstOrDefault();
            await InserirNaBase(new RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idQuestaoRegistroAcao,
                RespostaId = opcaoRespostaBase.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaAulaFrequencia(long aulaId, DateTime data, TipoFrequencia tipoAluno1, TipoFrequencia tipoAluno2)
        {
            await InserirNaBase(new Dominio.Aula
            {
                CriadoPor = "",
                CriadoRF = "",
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                ProfessorRf = "",
                TipoCalendarioId = TIPO_CALENDARIO_1,
                DataAula = data,
                Quantidade = 1
            });

            await InserirNaBase(new RegistroFrequencia
            {
                Id = aulaId,
                AulaId = aulaId,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                RegistroFrequenciaId = aulaId,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)tipoAluno1,
                NumeroAula = 1,
                AulaId = aulaId
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_2,
                RegistroFrequenciaId = aulaId,
                CriadoPor = "",
                CriadoRF = "",
                Valor = (int)tipoAluno2,
                NumeroAula = 1,
                AulaId = aulaId
            });
        }
    }
}
