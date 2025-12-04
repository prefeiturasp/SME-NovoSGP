using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente
{
    public class Ao_publicar_fila_notificacao_freq_mensal_insuficiente : TesteBaseComuns
    {
        public Ao_publicar_fila_notificacao_freq_mensal_insuficiente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeNotificacaoFreqMesInsuficiente), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAtribuicaoResponsaveisPorUeTipoQuery, IEnumerable<AtribuicaoResponsavelDto>>), typeof(ObterAtribuicaoResponsaveisPorUeTipoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Ao publicar msg notificação de freq. mensal insuficiente deve notificar profissionais (EF, EM, EJA)")]
        public async Task Deve_notificar_profissionais_sobre_alunos_freq_insuficiente_ensino_fund_medio_eja_menor_75_por_cento()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarTurma(Modalidade.Fundamental, ANO_7, TURMA_CODIGO_1, TipoTurma.Regular);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await Executor.ExecutarComando(new PublicarQuestionarioBuscaAtivaComando(this));
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_1);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 17), CODIGO_ALUNO_1);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_3);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 17), CODIGO_ALUNO_3);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 18), CODIGO_ALUNO_2);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 19), CODIGO_ALUNO_2);

            var useCase = ServiceProvider.GetService<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase>();
            var jsonMensagem = JsonSerializer.Serialize(new FiltroIdAnoLetivoDto(0, new DateTime(dataReferencia.Year,
                                                                                                 5, 31)));

            var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));
            retorno.ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.Count().ShouldBe(3);
            notificacoes.All(n => n.Titulo.Equals("Crianças/Estudantes com baixa frequência - EMEF Nome da UE")).ShouldBeTrue();
            notificacoes.All(n => n.Mensagem.Contains("Na lista abaixo encontram-se as crianças/estudantes que não atingiram o percentual mínimo de frequência no mês de Maio na EMEF Nome da UE (DRE 1)")).ShouldBeTrue();
            notificacoes.All(n => n.Mensagem.Contains("EF - Turma Nome 1")).ShouldBeTrue();
            notificacoes.All(n => n.Mensagem.Contains("1 - Nome aluno 1 (1)")).ShouldBeTrue();
            notificacoes.Any(n => n.Mensagem.Contains("2 - Nome aluno 2 (2)")).ShouldBeFalse();
            notificacoes.All(n => n.Mensagem.Contains("3 - Nome aluno 3 (3)")).ShouldBeTrue();
        }

        [Fact(DisplayName = "Ao publicar msg notificação de freq. mensal insuficiente deve notificar profissionais (EI)")]
        public async Task Deve_notificar_profissionais_sobre_alunos_freq_insuficiente_educacao_infantil_menor_60_por_cento()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio, false);
            await CriarTurma(Modalidade.EducacaoInfantil, ANO_2, TURMA_CODIGO_1, TipoTurma.Regular);
            await CriarRegistrosConsolidacaoFrequenciaAlunoMensal();
            await Executor.ExecutarComando(new PublicarQuestionarioBuscaAtivaComando(this));
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_1);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 17), CODIGO_ALUNO_1);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 16), CODIGO_ALUNO_3);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 17), CODIGO_ALUNO_3);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 18), CODIGO_ALUNO_2);
            await CriarRegistroAcaoEstudante(new(dataReferencia.Year, 5, 19), CODIGO_ALUNO_2);

            var useCase = ServiceProvider.GetService<IExecutarNotificacaoAlunosFreqMensalInsuficienteBuscaAtivaUseCase>();
            var jsonMensagem = JsonSerializer.Serialize(new FiltroIdAnoLetivoDto(0, new DateTime(dataReferencia.Year,
                                                                                                 5, 31)));

            var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));
            retorno.ShouldBeTrue();

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.Count().ShouldBe(3);
            notificacoes.All(n => n.Titulo.Equals("Crianças/Estudantes com baixa frequência - EMEF Nome da UE")).ShouldBeTrue();
            notificacoes.All(n => n.Mensagem.Contains("Na lista abaixo encontram-se as crianças/estudantes que não atingiram o percentual mínimo de frequência no mês de Maio na EMEF Nome da UE (DRE 1)")).ShouldBeTrue();
            notificacoes.All(n => n.Mensagem.Contains("EI - Turma Nome 1")).ShouldBeTrue();
            notificacoes.Any(n => n.Mensagem.Contains("1 - Nome aluno 1 (1)")).ShouldBeFalse();
            notificacoes.Any(n => n.Mensagem.Contains("2 - Nome aluno 2 (2)")).ShouldBeFalse();
            notificacoes.All(n => n.Mensagem.Contains("3 - Nome aluno 3 (3)")).ShouldBeTrue();
        }

        private async Task CriarRegistrosConsolidacaoFrequenciaAlunoMensal()
        {
            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Mes = 5,
                Percentual = 70,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 3,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                Mes = 5,
                Percentual = 80,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 8,
                QuantidadeCompensacoes = 0
            });

            await InserirNaBase(new Dominio.ConsolidacaoFrequenciaAlunoMensal()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                Mes = 5,
                Percentual = 40,
                QuantidadeAulas = 10,
                QuantidadeAusencias = 6,
                QuantidadeCompensacoes = 0
            });
        }

        private async Task CriarRegistroAcaoEstudante(DateTime dataRegistroAcao, string codigoAluno)
        {
            var idRegistroAcao = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>().Count() + 1;
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtiva()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = codigoAluno,
                AlunoNome = $"Nome do aluno {codigoAluno}",
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
    }
}
