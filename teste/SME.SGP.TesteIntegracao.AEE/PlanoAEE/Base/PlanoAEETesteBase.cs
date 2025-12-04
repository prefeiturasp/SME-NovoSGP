using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public abstract class PlanoAEETesteBase : TesteBaseComuns
    {
        public PlanoAEETesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorDreEolQuery, IEnumerable<UsuarioEolRetornoDto>>),
                typeof(ObterFuncionariosPorDreEolQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(PlanoAEE.ServicosFakes.ObterAlunoPorCodigoEolQueryHandlerFake), ServiceLifetime.Scoped));
        }
        protected IGerarPendenciaValidadePlanoAEEUseCase ObterServicoGerarPendenciaValidadePlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IGerarPendenciaValidadePlanoAEEUseCase>();
        }
        protected ISalvarPlanoAEEUseCase ObterServicoSalvarPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ISalvarPlanoAEEUseCase>();
        }

        protected IAtualizarTurmaDoPlanoAEEUseCase ObterServicoAtualizarTurmaDoPlanoAEE()
        {
            return ServiceProvider.GetService<IAtualizarTurmaDoPlanoAEEUseCase>();
        }

        protected IAtualizarInformacoesDoPlanoAEEUseCase ObterServicoAtualizarInformacoesDoPlanoAEE()
        {
            return ServiceProvider.GetService<IAtualizarInformacoesDoPlanoAEEUseCase>();
        }     

        protected IObterAlunosPorCodigoEolNomeUseCase ObterAlunosPorCodigoEolNomeUseCase()
        {
            return ServiceProvider.GetService<IObterAlunosPorCodigoEolNomeUseCase>();
        }
        protected IObterResponsaveisPlanosAEEUseCase ObterServicoObterResponsaveisPlanosAEEUseCase()
        {
            return ServiceProvider.GetService<IObterResponsaveisPlanosAEEUseCase>();
        }

        protected IObterPlanosAEEUseCase ObterServicoObterPlanosAEEUseCase()
        {
            return ServiceProvider.GetService<IObterPlanosAEEUseCase>();
        }

        protected IObterPlanoAEEPorIdUseCase ObterServicoObterPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEPorIdUseCase>();
        }

        protected IObterQuestoesPlanoAEEPorVersaoUseCase ObterServicoObterQuestoesPlanoAEEPorVersaoUseCase()
        {
            return ServiceProvider.GetService<IObterQuestoesPlanoAEEPorVersaoUseCase>();
        }

        protected IVerificarExistenciaPlanoAEEPorEstudanteUseCase ObterServicoVerificarExistenciaPlanoAEEPorEstudanteUseCase()
        {
            return ServiceProvider.GetService<IVerificarExistenciaPlanoAEEPorEstudanteUseCase>();
        }

        protected IObterRestruturacoesPlanoAEEPorIdUseCase ObterServicoObterRestruturacoesPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterRestruturacoesPlanoAEEPorIdUseCase>();
        }

        protected IObterVersoesPlanoAEEUseCase ObterServicoObterVersoesPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IObterVersoesPlanoAEEUseCase>();
        }

        protected IObterParecerPlanoAEEPorIdUseCase ObterServicoObterParecerPlanoAEEPorIdUseCase()
        {
            return ServiceProvider.GetService<IObterParecerPlanoAEEPorIdUseCase>();
        }

        protected IEncerrarPlanoAEEUseCase ObterServicoEncerrarPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IEncerrarPlanoAEEUseCase>();
        }

        protected ICadastrarParecerCPPlanoAEEUseCase ObterServicoCadastrarParecerCPPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ICadastrarParecerCPPlanoAEEUseCase>();
        }

        protected ICadastrarParecerPAAIPlanoAEEUseCase ObterServicoCadastrarParecerPAAIPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<ICadastrarParecerPAAIPlanoAEEUseCase>();
        }

        protected IAtribuirResponsavelPlanoAEEUseCase ObterServicoAtribuirResponsavelPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IAtribuirResponsavelPlanoAEEUseCase>();
        }
        
        protected IRemoverResponsavelPlanoAEEUseCase ObterServicoRemocaoAtribuirResponsavelPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IRemoverResponsavelPlanoAEEUseCase>();
        }

        protected IDevolverPlanoAEEUseCase ObterServicoDevolverPlanoAEEUseCase()
        {
            return ServiceProvider.GetService<IDevolverPlanoAEEUseCase>();
        }

        protected IAtribuirResponsavelGeralDoPlanoUseCase ObterServicoAtribuirResponsavelGeralDoPlanoUseCase()
        {
            return ServiceProvider.GetService<IAtribuirResponsavelGeralDoPlanoUseCase>();
        }

        protected IObterPlanoAEEPorCodigoEstudanteUseCase ObterServicoObterPlanoAEEPorCodigoEstudanteUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEPorCodigoEstudanteUseCase>();
        }

        protected IObterPlanoAEEObservacaoUseCase ObterServicoObterPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IObterPlanoAEEObservacaoUseCase>();
        }

        protected ICriarPlanoAEEObservacaoUseCase ObterServicoCriarPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<ICriarPlanoAEEObservacaoUseCase>();
        }
        protected IAlterarPlanoAEEObservacaoUseCase ObterServicoAlterarPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IAlterarPlanoAEEObservacaoUseCase>();
        }
        protected IExcluirPlanoAEEObservacaoUseCase ObterServicoExcluirPlanoAEEObservacaoUseCase()
        {
            return ServiceProvider.GetService<IExcluirPlanoAEEObservacaoUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroPlanoAee filtroPlanoAee)
        {
            await CriarTipoCalendario(filtroPlanoAee.TipoCalendario);

            await CriarDreUePerfil();

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroPlanoAee.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroPlanoAee.Modalidade, filtroPlanoAee.TurmaHistorica, filtroPlanoAee.TurmasMesmaUe);

            await CriarQuestionario();
            
            await CriarQuestoes();

            await CriarRespostas();

            await CriarRespostasComplementares();
        }

        private async Task CriarRespostasComplementares()
        {
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 3,
                QuestaoComplementarId = 5,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 5,
                QuestaoComplementarId = 5,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 10,
                QuestaoComplementarId = 5,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 14,
                QuestaoComplementarId = 9,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 1,
                QuestaoComplementarId = 11,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 2,
                QuestaoComplementarId = 13,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 6,
                QuestaoComplementarId = 15,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 9,
                QuestaoComplementarId = 9,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 8,
                QuestaoComplementarId = 11,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 7,
                QuestaoComplementarId = 13,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = 12,
                QuestaoComplementarId = 15,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
        {
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 10,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 12,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 4,
                Ordem = 1,
                Nome = "Individual",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 1,
                Nome = "Colaborativo",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 4,
                Ordem = 2,
                Nome = "Em grupo",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 14,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 12,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 10,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 8,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 4,
                Ordem = 3,
                Nome = "Misto",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 3,
                Nome = "Itinerante",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 14,
                Ordem = 2,
                Nome = "Não",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 2,
                Ordem = 2,
                Nome = "Contraturno",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new OpcaoResposta()
            {
                QuestaoId = 8,
                Ordem = 1,
                Nome = "Sim",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Plano AEE",
                Tipo = TipoQuestionario.PlanoAEE,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Bimestre de vigência do plano",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.PeriodoEscolar,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 2,
                Nome = "Organização do AEE",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 3,
                Nome = "Dias e horários de frequência do estudante no AEE",
                Tipo = TipoQuestao.FrequenciaEstudanteAEE,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 4,
                Nome = "Forma de atendimento educacional especializado do estudante",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 5,
                Nome = "Objetivos do AEE",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 6,
                Nome = "Orientações e ações para o desenvolvimento/atividades do AEE",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 7,
                Nome = "Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala regular (Seleção de materiais, equipamentos e mobiliário)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 8,
                Nome = "Tem necessidade de recursos de Acessibilidade/Materiais para eliminação de barreiras para a sala de recursos multifuncionais (Seleção de materiais, equipamentos e mobiliário)",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 9,
                Nome = "Mobilização dos Recursos Humanos da U.E. ou parcerias na unidade educacional",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 10,
                Nome = "Mobilização dos Recursos Humanos com profissionais externos à unidade educacional",
                Obrigatorio = true,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 1,
                Ordem = 1,
                Nome = "Justifique",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }
    }
}
