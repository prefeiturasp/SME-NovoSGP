using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.RelatorioDinamico
{
    public class Ao_obter_encaminhamento_para_relatorio_dinamico_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_obter_encaminhamento_para_relatorio_dinamico_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico para todas modalidades")]
        public async Task Ao_obter_encaminhamento_do_relatorio_dinamico_para_todas_modalidade()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa);

            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { },
                Anos = new List<string> { ANO_6, ANO_8 },
                FiltroAvancado = new List<FiltroComponenteRelatorioDinamicoNAAPA>()
                {
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DE_ENTRADA_DA_QUEIXA,
                        TipoQuestao = TipoQuestao.Periodo,
                        Resposta = $"[{dataQueixa.ToString("yyyy-MM-dd")}, {dataQueixa.AddMonths(1).ToString("yyyy-MM-dd")}]",
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PRIORIDADE,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_GENERO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_GENERO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 2
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_GRUPO_ETNICO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_ESTUDANTE_MIGRANTE,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico modalidade infantil")]
        public async Task Ao_obter_encaminhamento_do_relatorio_dinamico_modalidade_infantil()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                Modalidade = Modalidade.EducacaoInfantil,
                AnoTurma = ANO_1,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa);
            await InserirEncaminhamentoModalidadeInfantil(6);


            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { },
                Anos = new List<string> { ANO_1 },
                FiltroAvancado = new List<FiltroComponenteRelatorioDinamicoNAAPA>()
                {
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_DESENVOLVIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1,
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROTECAO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(1);
        }


        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico modalidade fundamental médio")]
        public async Task Ao_obter_encaminhamento_do_relatorio_dinamico_modalidade_fundamental_medio()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa);
            await InserirEncaminhamentoModalidadeFundamentalMedio(6);


            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { },
                Anos = new List<string> { ANO_6, ANO_8 },
                FiltroAvancado = new List<FiltroComponenteRelatorioDinamicoNAAPA>()
                {
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_ENSINO_APRENDIZAGEM,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1,
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_HIPOTESE_ESCRITA,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PERMANENCIA_ESCOLAR,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico sem filtro avançado")]
        public async Task Ao_obter_encaminhamento_do_relatorio_dinamico_sem_filtro_avancado()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_8,
                DreId = DRE_ID_1,
                CodigoUe = UE_CODIGO_1,
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa);

            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { },
                Anos = new List<string> { ANO_6, ANO_8 },
                FiltroAvancado = null
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(1);
        }

        private async Task InserirEncaminhamento(DateTime dataQueixa)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            var QuestaoEncaminhamentoId = 1;

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ENTRADA_QUEIXA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                Texto = dataQueixa.ToString("yyyy-MM-dd"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PRIORIDADE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_NORMAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_GENERO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_MASCULINO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_GRUPO_ETNICO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_BRANCO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            QuestaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_ESTUDANTE_MIGRANTE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = QuestaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_SIM,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task InserirEncaminhamentoModalidadeInfantil(int questaoEncaminhamentoId = 1)
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_AGRUPAMENTO_DESENVOLVIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_AGITACAO_MOTORA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            questaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_CARTEIRA_VACINAS_ATRASADA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            questaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROTECAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_SITUACAO_RISCO_SOCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            questaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ASSADURA_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task InserirEncaminhamentoModalidadeFundamentalMedio(int questaoEncaminhamentoId = 1)
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_ENSINO_APRENDIZAGEM,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_ALFABETIZACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            questaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_HIPOTESE_ESCRITA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_PRE_SILABICO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            questaoEncaminhamentoId++;

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PERMANENCIA_ESCOLAR,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = questaoEncaminhamentoId,
                RespostaId = ID_OPCAO_RESPOSTA_BAIXA_FREQUENCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
