using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.RelatorioDinamico
{
    public class Ao_obter_encaminhamento_atendimento_para_relatorio_dinamico_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_obter_encaminhamento_atendimento_para_relatorio_dinamico_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico por atendimento mês")]
        public async Task Ao_obter_encaminhamento_atendimento_do_relatorio_dinamico_atendimento_por_mes()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = $"{dataAtual.Year}-01-01T12:33:04",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = $"{dataAtual.Year}-02-01T12:33:04",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolhaMes,
                        OrdemResposta = 1,
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolhaMes,
                        OrdemResposta = 2,
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico por atendimento mês sem atendimento")]
        public async Task Ao_obter_encaminhamento_atendimento_do_relatorio_dinamico_atendimento_por_mes_sem_atendimento()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = $"{dataAtual.Year}-01-01T12:33:04",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = $"{dataAtual.Year}-02-01T12:33:04",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolhaMes,
                        OrdemResposta = 3,
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.EncaminhamentosNAAPAPaginado.ShouldNotBeNull();
            retorno.EncaminhamentosNAAPAPaginado.Items.Count().ShouldBe(0);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico por atendimento modalidade e procedimento")]
        public async Task Ao_obter_encaminhamento_atendimento_do_relatorio_dinamico_atendimento_por_modalidade_procedimento()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                RespostaId = ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PROCEDIMENTO_DE_TRABALHO,
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


        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico por atendimento do total do atendimento")]
        public async Task Ao_obter_encaminhamento_relatorio_dinamico_total_atendimento()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                RespostaId = ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PROCEDIMENTO_DE_TRABALHO,
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

            //Total atendimento
            retorno.TotalDeAtendimento.ShouldNotBeNull();
            retorno.TotalDeAtendimento.Total.ShouldBe(1);
            //Aendimento por questão
            var questoes = retorno.TotalDeAtendimento.TotalAtendimentoQuestoes;
            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(2);
            var questao1 = questoes.FirstOrDefault();
            questao1.ShouldNotBeNull();
            questao1.Descricao.ShouldBe("Procedimento de trabalho");
            questao1.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao1.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(1);
            var resposta1Questao1 = questao1.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao1.ShouldNotBeNull();
            resposta1Questao1.Descricao.ShouldBe("Ações Lúdicas");
            resposta1Questao1.Total.ShouldBe(1);

            var questao2 = questoes.LastOrDefault();
            questao2.ShouldNotBeNull();
            questao2.Descricao.ShouldBe("Modalidade de atenção");
            questao2.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao2.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(1);
            var resposta1Questao2 = questao2.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao2.ShouldNotBeNull();
            resposta1Questao2.Descricao.ShouldBe("Itinerância");
            resposta1Questao2.Total.ShouldBe(1);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico com mais de um encaminhamento")]
        public async Task Ao_obter_encaminhamento_relatorio_dinamico_com_mais_de_um_encaminhamento()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            //Encaminhamento 1
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 1
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 1
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
                QuestaoEncaminhamentoId = 1,
                RespostaId = ID_OPCAO_RESPOSTA_NORMAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Encaminhamento 2
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 2",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 2
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 2
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PRIORIDADE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = ID_OPCAO_RESPOSTA_NORMAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 3
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 3
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 3,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                RespostaId = ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 4
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 3,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 4
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 5
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 4,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 5,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 6
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 4,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 6,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PRIORIDADE,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_DO_ATENDIMENTO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    },
                    new FiltroComponenteRelatorioDinamicoNAAPA()
                    {
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PROCEDIMENTO_DE_TRABALHO,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            //Total atendimento
            retorno.TotalDeAtendimento.ShouldNotBeNull();
            retorno.TotalDeAtendimento.Total.ShouldBe(1);
            //Aendimento por questão
            var questoes = retorno.TotalDeAtendimento.TotalAtendimentoQuestoes;
            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(2);
            var questao1 = questoes.FirstOrDefault();
            questao1.ShouldNotBeNull();
            questao1.Descricao.ShouldBe("Procedimento de trabalho");
            questao1.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao1.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(1);
            var resposta1Questao1 = questao1.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao1.ShouldNotBeNull();
            resposta1Questao1.Descricao.ShouldBe("Ações Lúdicas");
            resposta1Questao1.Total.ShouldBe(1);

            var questao2 = questoes.LastOrDefault();
            questao2.ShouldNotBeNull();
            questao2.Descricao.ShouldBe("Modalidade de atenção");
            questao2.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao2.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(1);
            var resposta1Questao2 = questao2.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao2.ShouldNotBeNull();
            resposta1Questao2.Descricao.ShouldBe("Itinerância");
            resposta1Questao2.Total.ShouldBe(1);
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter informações do relatório dinâmico com mais de um encaminhamento sem filtro")]
        public async Task Ao_obter_encaminhamento_relatorio_dinamico_com_mais_de_um_encaminhamento_sem_filtro()
        {
            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;

            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);

            //Encaminhamento 1
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 1
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 1
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
                QuestaoEncaminhamentoId = 1,
                RespostaId = ID_OPCAO_RESPOSTA_NORMAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //Encaminhamento 2
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = SituacaoNAAPA.EmAtendimento,
                AlunoNome = "Nome do aluno 2",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 2
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 2
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PRIORIDADE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                RespostaId = ID_OPCAO_RESPOSTA_NORMAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 3
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 3
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 3,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                RespostaId = ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 4
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 3,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //secao 4
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 2,
                SecaoEncaminhamentoNAAPAId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 5
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 4,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 5,
                RespostaId = ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            //questao 6
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 4,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 6,
                RespostaId = ID_OPCAO_RESPOSTA_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

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
                        NomeComponente = QUESTAO_NOME_COMPONENTE_PRIORIDADE,
                        TipoQuestao = TipoQuestao.ComboMultiplaEscolha,
                        OrdemResposta = 1
                    }
                }
            };

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterAtendimentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            //Total atendimento
            retorno.TotalDeAtendimento.ShouldNotBeNull();
            retorno.TotalDeAtendimento.Total.ShouldBe(2);
            //Aendimento por questão
            var questoes = retorno.TotalDeAtendimento.TotalAtendimentoQuestoes;
            questoes.ShouldNotBeNull();
            questoes.Count().ShouldBe(2);
            var questao1 = questoes.FirstOrDefault();
            questao1.ShouldNotBeNull();
            questao1.Descricao.ShouldBe("Procedimento de trabalho");
            questao1.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao1.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(1);
            var resposta1Questao1 = questao1.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao1.ShouldNotBeNull();
            resposta1Questao1.Descricao.ShouldBe("Ações Lúdicas");
            resposta1Questao1.Total.ShouldBe(2);

            var questao2 = questoes.LastOrDefault();
            questao2.ShouldNotBeNull();
            questao2.Descricao.ShouldBe("Modalidade de atenção");
            questao2.TotalAtendimentoQuestaoPorRespostas.ShouldNotBeNull();
            questao2.TotalAtendimentoQuestaoPorRespostas.Count().ShouldBe(2);
            var resposta1Questao2 = questao2.TotalAtendimentoQuestaoPorRespostas.FirstOrDefault();
            resposta1Questao2.ShouldNotBeNull();
            resposta1Questao2.Descricao.ShouldBe("Itinerância");
            resposta1Questao2.Total.ShouldBe(1);
        }
    }
}
