using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Commands;
using SME.SGP.TesteIntegracao.Constantes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class RegistroAcaoBuscaAtivaTesteBase : TesteBaseComuns
    {
        public RegistroAcaoBuscaAtivaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroRegistroAcaoDto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(filtro.Perfil);
            await CriarUsuarios();
            await CriarTurmaTipoCalendario(filtro);

            await Executor.ExecutarComando(new PublicarQuestionarioBuscaAtivaComando(this));
        }

        protected async Task CriarTurmaTipoCalendario(FiltroRegistroAcaoDto filtro)
        {
            await CriarTipoCalendario(filtro.TipoCalendario);
            if (filtro.CriarTurmaPadrao)
                await CriarTurma(filtro.Modalidade, filtro.AnoTurma);
        }

        protected IObterSecoesRegistroAcaoSecaoUseCase ObterUseCaseListagemSecoes()
        {
            return ServiceProvider.GetService<IObterSecoesRegistroAcaoSecaoUseCase>();
        }

        protected IObterQuestionarioRegistroAcaoUseCase ObterUseCaseListagemQuestionario()
        {
            return ServiceProvider.GetService<IObterQuestionarioRegistroAcaoUseCase>();
        }

        protected IRegistrarRegistroAcaoUseCase ObterUseCaseRegistroAcao()
        {
            return ServiceProvider.GetService<IRegistrarRegistroAcaoUseCase>();
        }

        protected IObterRegistroAcaoPorIdUseCase ObterUseCaseObtencaoRegistroAcao()
        {
            return ServiceProvider.GetService<IObterRegistroAcaoPorIdUseCase>();
        }

        protected IObterRegistrosAcaoCriancaEstudanteAusenteUseCase ObterUseCaseListagemRegistrosAcao_EstudantesAusentes()
        {
            return ServiceProvider.GetService<IObterRegistrosAcaoCriancaEstudanteAusenteUseCase>();
        }

        protected IObterRegistrosAcaoUseCase ObterUseCaseListagemRegistrosAcao()
        {
            return ServiceProvider.GetService<IObterRegistrosAcaoUseCase>();
        }

        protected IExcluirRegistroAcaoUseCase ObterUseCaseExclusaoRegistroAcao()
        {
            return ServiceProvider.GetService<IExcluirRegistroAcaoUseCase>();
        }


        protected class FiltroRegistroAcaoDto
        {
            public FiltroRegistroAcaoDto()
            {
                CriarTurmaPadrao = true;
            }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public string AnoTurma { get; set; }
            public bool CriarTurmaPadrao { get; set; }
        }

        protected async Task GerarDadosRegistroAcao_2PrimeirasQuestoes(DateTime dataRegistro, bool adicionarRespostasComplementarConseguiuContatoResponsavel = false, long turmaId = TURMA_ID_1)
        {
            await CriarRegistroAcao(turmaId);
            var registrosAcaoId = ObterTodos<Dominio.RegistroAcaoBuscaAtiva>().Max(ra => ra.Id);
            await CriarRegistroAcaoSecao(registrosAcaoId);
            var registrosAcaoSecaoId = ObterTodos<Dominio.RegistroAcaoBuscaAtivaSecao>().Max(ra => ra.Id);
            await CriarQuestoesRegistroAcao(registrosAcaoSecaoId, adicionarRespostasComplementarConseguiuContatoResponsavel);
            var registrosAcaoQuestaoId = ObterTodos<Dominio.QuestaoRegistroAcaoBuscaAtiva>().Where(ra => ra.RegistroAcaoBuscaAtivaSecaoId == registrosAcaoSecaoId).Min(ra => ra.Id);
            await CriarRespostasRegistroAcao(dataRegistro, registrosAcaoQuestaoId, adicionarRespostasComplementarConseguiuContatoResponsavel);
        }

        private async Task CriarRespostasRegistroAcao(DateTime dataRegistro, long idRegistroAcaoQuestao = 1, bool adicionarRespostasComplementarConseguiuContatoResponsavel = false)
        {
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                Texto = dataRegistro.ToString("yyyy-MM-dd"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            idRegistroAcaoQuestao++;

            var opcoesResposta = ObterTodos<OpcaoResposta>();
            var opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP
                                                         && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_OPCAO_RESPOSTA_SIM).FirstOrDefault();
            await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
            {
                QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                RespostaId = opcaoRespostaBase.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            idRegistroAcaoQuestao++;

            if (adicionarRespostasComplementarConseguiuContatoResponsavel)
            {
                opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA
                                                            && q.Nome == "Estudante grávida").FirstOrDefault();
                await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
                {
                    QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                    RespostaId = opcaoRespostaBase.Id,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                idRegistroAcaoQuestao++;

                opcaoRespostaBase = opcoesResposta.Where(q => q.QuestaoId == ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO 
                                                         && q.Nome == ConstantesQuestionarioBuscaAtiva.QUESTAO_PROCEDIMENTO_REALIZADO_RESPOSTA_LIG_TELEFONICA).FirstOrDefault();
                await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
                {
                    QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                    RespostaId = opcaoRespostaBase.Id,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                idRegistroAcaoQuestao++;

                await InserirNaBase(new Dominio.RespostaRegistroAcaoBuscaAtiva()
                {
                    QuestaoRegistroAcaoBuscaAtivaId = idRegistroAcaoQuestao,
                    Texto = "OBS GERAL",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                idRegistroAcaoQuestao++;
            }
        }

        private async Task CriarQuestoesRegistroAcao(long idRegistroAcaoSecao = 1, bool adicionarRespostasComplementarConseguiuContatoResponsavel = false)
        {
            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_1_ID_DATA_REGISTRO_ACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
            {
                RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_ID_CONSEGUIU_CONTATO_RESP,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            if (adicionarRespostasComplementarConseguiuContatoResponsavel)
            {
                await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
                {
                    RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                    QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_3_ID_JUSTIFICATIVA_MOTIVO_FALTA,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
                {
                    RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                    QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_4_ID_PROCEDIMENTO_REALIZADO,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });

                await InserirNaBase(new Dominio.QuestaoRegistroAcaoBuscaAtiva()
                {
                    RegistroAcaoBuscaAtivaSecaoId = idRegistroAcaoSecao,
                    QuestaoId = ConstantesQuestionarioBuscaAtiva.QUESTAO_2_5_ID_OBS_GERAL,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }

        }

        private async Task CriarRegistroAcaoSecao(long idRegistroAcao = 1)
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtivaSecao()
            {
                RegistroAcaoBuscaAtivaId = idRegistroAcao,
                SecaoRegistroAcaoBuscaAtivaId = ConstantesQuestionarioBuscaAtiva.SECAO_REGISTRO_ACAO_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Concluido = false
            });
        }

        private async Task CriarRegistroAcao(long turmaId)
        {
            await InserirNaBase(new Dominio.RegistroAcaoBuscaAtiva()
            {
                TurmaId = turmaId,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
