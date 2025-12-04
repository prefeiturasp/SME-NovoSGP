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
    public class Ao_obter_total_registros_encaminhamento_para_relatorio_dinamico_naapa : AtendimentoNAAPATesteBase
    {
        private const long ENCAMINHAMENTO_ID_1 = 1;
        private const long ENCAMINHAMENTO_ID_2 = 2;
        private const long ENCAMINHAMENTO_ID_3 = 3;
        private const long TOTAL_QUESTAO_6 = 6;
        private const long TOTAL_QUESTAO_11 = 11;
        public Ao_obter_total_registros_encaminhamento_para_relatorio_dinamico_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter total de registro do relatório dinâmico para todas modalidades")]
        public async Task Ao_obter_total_registros_do_relatorio_dinamico_para_todas_modalidade()
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

            await CriarTurma(Modalidade.EducacaoInfantil, ANO_1);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa, ALUNO_CODIGO_1, TURMA_ID_1, ENCAMINHAMENTO_ID_1);
            await InserirEncaminhamento(dataQueixa, ALUNO_CODIGO_2, TURMA_ID_2, ENCAMINHAMENTO_ID_2, TOTAL_QUESTAO_6);

            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { },
                Anos = new List<string> { ANO_1, ANO_6, ANO_8 },
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

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.TotalRegistroPorModalidadesAno.ShouldNotBeNull();
            retorno.TotalRegistro.ShouldBe(2);
            var totalRegistroInfantil = retorno.TotalRegistroPorModalidadesAno.FirstOrDefault(total => total.Modalidade == Modalidade.EducacaoInfantil);
            totalRegistroInfantil.ShouldNotBeNull();
            totalRegistroInfantil.Total.ShouldBe(1);
            var totalRegistroFundamental = retorno.TotalRegistroPorModalidadesAno.FirstOrDefault(total => total.Modalidade == Modalidade.Fundamental);
            totalRegistroFundamental.ShouldNotBeNull();
            totalRegistroFundamental.Total.ShouldBe(1);
        }


        [Fact(DisplayName = "Relatório dinâmico do Encaminhamento NAAPA - Obter total de registro do relatório dinâmico para ano turma")]
        public async Task Ao_obter_total_registros_do_relatorio_dinamico_para_ano_turma()
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

            await CriarTurma(Modalidade.Fundamental, ANO_7);
            await CriarTurma(Modalidade.Fundamental, ANO_6);

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var dataQueixa = new DateTime(dataAtual.Year, 11, 18);

            await InserirEncaminhamento(dataQueixa, ALUNO_CODIGO_1, TURMA_ID_1, ENCAMINHAMENTO_ID_1);
            await InserirEncaminhamento(dataQueixa, ALUNO_CODIGO_2, TURMA_ID_2, ENCAMINHAMENTO_ID_2, TOTAL_QUESTAO_6);
            await InserirEncaminhamento(dataQueixa, ALUNO_CODIGO_3, TURMA_ID_3, ENCAMINHAMENTO_ID_3, TOTAL_QUESTAO_11);

            var filtro = new FiltroRelatorioDinamicoNAAPADto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = dataAtual.Year,
                Modalidades = new Modalidade[] { Modalidade.Fundamental },
                Anos = new List<string> { ANO_6, ANO_7, ANO_8 },
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

            var useCase = ServiceProvider.GetService<IRelatorioDinamicoObterEncaminhamentoNAAPAUseCase>();
            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();

            retorno.TotalRegistroPorModalidadesAno.ShouldNotBeNull();
            retorno.TotalRegistro.ShouldBe(3);
            var totalRegistroAno6 = retorno.TotalRegistroPorModalidadesAno.FirstOrDefault(total => total.Ano == ANO_6);
            totalRegistroAno6.ShouldNotBeNull();
            totalRegistroAno6.Total.ShouldBe(1);
            var totalRegistroAno7 = retorno.TotalRegistroPorModalidadesAno.FirstOrDefault(total => total.Ano == ANO_7);
            totalRegistroAno7.ShouldNotBeNull();
            totalRegistroAno7.Total.ShouldBe(1);
            var totalRegistroAno8 = retorno.TotalRegistroPorModalidadesAno.FirstOrDefault(total => total.Ano == ANO_8);
            totalRegistroAno8.ShouldNotBeNull();
            totalRegistroAno8.Total.ShouldBe(1);
        }

        private async Task InserirEncaminhamento(DateTime dataQueixa, string codigoAluno, long turmaId, long encaminhamentoId, long questaoId = 1)
        {
            await InserirNaBase(new Dominio.EncaminhamentoNAAPA()
            {
                TurmaId = turmaId,
                AlunoCodigo = codigoAluno,
                Situacao = SituacaoNAAPA.AguardandoAtendimento,
                AlunoNome = $"Nome do aluno {codigoAluno}",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });


            var QuestaoEncaminhamentoId = questaoId;

            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = encaminhamentoId,
                SecaoEncaminhamentoNAAPAId = encaminhamentoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = encaminhamentoId,
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
                EncaminhamentoNAAPASecaoId = encaminhamentoId,
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
                EncaminhamentoNAAPASecaoId = encaminhamentoId,
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
                EncaminhamentoNAAPASecaoId = encaminhamentoId,
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
                EncaminhamentoNAAPASecaoId = encaminhamentoId,
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
    }
}
