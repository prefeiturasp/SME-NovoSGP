using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioPAP.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioPAP
{
    public class Ao_copiar_relatorio_pap : RelatorioPAPTesteBase
    {
        public Ao_copiar_relatorio_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        [Fact(DisplayName = "Copiar relatório pap para aluno")]
        public async Task Ao_copiar_relatorio_pap_aluno()
        {
            var resultado = await CriarPapPorAluno();
            var useCaseCopiar = ServiceProvider.GetService<ICopiarRelatorioPAPUseCase>();
            var relatorioTurmaOrigem = ObterTodos<RelatorioPeriodicoPAPTurma>();
            var relatorioAlunoOrigem = ObterTodos<RelatorioPeriodicoPAPAluno>();
            var relatorioSecaoOrigem = ObterTodos<RelatorioPeriodicoPAPSecao>();
            var relatorioQuestaoOrigem = ObterTodos<RelatorioPeriodicoPAPQuestao>();
            var relatorioRespostaOrigem = ObterTodos<RelatorioPeriodicoPAPResposta>();

            var dtoCopiar = new CopiarPapDto
            {
                PeriodoRelatorioPAPId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                CodigoAlunoOrigem = CODIGO_ALUNO_1,
                CodigoTurmaOrigem = TURMA_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_2,
                Estudantes = new List<CopiarPapEstudantesDto>()
                {
                    new() { AlunoCodigo = CODIGO_ALUNO_2, AlunoNome = "Aluno PAP Copiar" }
                },
                Secoes = new List<CopiarSecaoDto>()
                {
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                            ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                        }
                    },
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4
                        }
                    },
                    new()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                        QuestoesIds = new[]
                        {
                            ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                        }
                    },
                }
            };
            await useCaseCopiar.Executar(dtoCopiar);
            var relatorioTurmaAposCopiar = ObterTodos<RelatorioPeriodicoPAPTurma>();
            var relatorioAlunoAposCopiar = ObterTodos<RelatorioPeriodicoPAPAluno>();
            var relatorioSecaoAposCopiar = ObterTodos<RelatorioPeriodicoPAPSecao>();
            var relatorioQuestaoAposCopiar = ObterTodos<RelatorioPeriodicoPAPQuestao>();
            var relatorioRespostaAposCopiar = ObterTodos<RelatorioPeriodicoPAPResposta>();
        }

        private async Task<ResultadoRelatorioPAPDto> CriarPapPorAluno()
        {
            await CriarDadosBase(true, true);
            
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 2,
                CodigoTurma = "2",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio
            });
            
            var relatorio = new RelatorioPAPDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNome = "Pap",
                periodoRelatorioPAPId = ConstantesTestePAP.PERIODO_RELATORIO_PAP_ID_1,
                TurmaId = TURMA_ID_1,
                Secoes = new List<RelatorioPAPSecaoDto>
                {
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_DIFICULDADES_APRESENTADAS_ID_2,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_DIFICULDADES_APRESENTADAS_ID_2,
                                TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                                Resposta = ConstantesTestePAP.OPCAO_RESPOSTA_LEITURA_ID
                            },
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACAO_ID_3,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Observação dificuldades apresentadas"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_AVANC_APREND_BIMES_ID_3,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_AVANÇOS_NA_APRENDIZAGEM_DURANTE_O_BIMESTRE_ID_4,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Avanços na aprendizagem durante o bimestre"
                            }
                        }
                    },
                    new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ConstantesTestePAP.SECAO_RELATORIO_PERIODICO_PAP_SECAO_OBS_ID_4,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = ConstantesTestePAP.QUESTAO_OBSERVACOES_ID_5,
                                TipoQuestao = TipoQuestao.EditorTexto,
                                Resposta = "Observações"
                            }
                        }
                    }
                }
            };

            var useCase = ServiceProvider.GetService<ISalvarRelatorioPAPUseCase>();

            return await useCase.Executar(relatorio);
        }
    }
}