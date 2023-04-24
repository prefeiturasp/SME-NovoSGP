using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Itinerancia.Base;
using SME.SGP.TesteIntegracao.Itinerancia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Itinerancia
{
    public class Ao_salvar_registro_itinerancia : ItineranciaBase
    {
        public Ao_salvar_registro_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ArmazenarArquivoFisicoCommand, string>),typeof(ArmazenarArquivoFisicoCommandHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Registro de itinerância - Salvar itinerância sem alunos, somente com informações globais")]
        public async Task Salvar_itinerancia_sem_alunos()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            var useCase = SalvarItineranciaUseCase();
            var retorno =  await useCase.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), ObterQuestoesItinerancia()));
            retorno.ShouldNotBeNull("Itinerância não persistida ao salvar");
            retorno.Id.ShouldBe(1, "Id da itinerância persistida deveria ser 1");

            var itinerancias = ObterTodos<Dominio.Itinerancia>();
            itinerancias.ShouldNotBe(null, "Itinerância não persistida ao salvar");
            itinerancias.Count.ShouldBe(1, "Quantidade de itinerâncias persistidas ao salvar deveria ser 1");

            var objetivosItinerancia = ObterTodos<Dominio.ItineranciaObjetivo>();
            objetivosItinerancia.ShouldNotBe(null, "Objetivos da Itinerância não persistida ao salvar");
            objetivosItinerancia.Count.ShouldBe(3, "Quantidade de objetivos na itinerância persistida ao salvar deveria ser 3");

            var questoesItinerancia = ObterTodos<Dominio.ItineranciaQuestao>();
            questoesItinerancia.ShouldNotBe(null, "Questões/Repostas da Itinerância não persistidas ao salvar");
            questoesItinerancia.Count.ShouldBe(3, "Quantidade de questões na itinerância persistida ao salvar deveria ser 3");

            var itineranciasAluno = ObterTodos<Dominio.ItineranciaAluno>();
            itineranciasAluno.ShouldNotBe(null, "Itinerâncias Alunos persistidas deveria ser null");
            
            /*var arquivos = ObterTodos<Arquivo>();
            arquivos.Count.ShouldBeEquivalentTo(1);
            arquivos.FirstOrDefault()?.Codigo.ShouldBeEquivalentTo(salvar.Codigo);
            arquivos.FirstOrDefault()?.Nome.ShouldBeEquivalentTo(nomeArquivo);
            arquivos.FirstOrDefault()?.TipoConteudo.ShouldBeEquivalentTo(extensaoArquivo);
            arquivos.FirstOrDefault()?.Tipo.ShouldBeEquivalentTo(TipoArquivo.Itinerancia);*/
        }

        private List<ItineranciaObjetivoDto> ObterObjetivosVisita()
        {
            return new List<ItineranciaObjetivoDto>()
            {
                new ItineranciaObjetivoDto()
                {
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_MAPEAMENTO_ESTUDANTES_PUBLICO_EE,
                    TemDescricao = false
                },
                new ItineranciaObjetivoDto()
                {
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_ATENDIMENTO_SOLICITACAO_UE,
                    TemDescricao = true,
                    Descricao = "Descrição base atendimento solicitação Ue"
                },
                new ItineranciaObjetivoDto()
                {
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_REUNIAO,
                    TemDescricao = false
                },
            };
        }

        private List<ItineranciaQuestaoDto> ObterQuestoesItinerancia()
        {
            return new List<ItineranciaQuestaoDto>()
            {
                new ItineranciaQuestaoDto()
                {
                    ItineranciaId = 1,
                    Descricao = "Acompanhamento da situação",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_GERAL,
                    TipoQuestao = TipoQuestao.Texto,
                    Resposta = "Teste Acompanhamento da situação"
                },
                new ItineranciaQuestaoDto()
                {
                    ItineranciaId = 1,
                    Descricao = "Encaminhamentos",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ENCAMINHAMENTOS_GERAL,
                    TipoQuestao = TipoQuestao.Texto,
                    Resposta = "Teste Encaminhamentos"
                }
            };
        }

        private List<ItineranciaAlunoDto> ObterItineranciasAluno()
        {
            return new List<ItineranciaAlunoDto>()
            {
                new ItineranciaAlunoDto()
                {
                    AlunoCodigo = ALUNO_CODIGO_1,
                    NomeAlunoComTurmaModalidade = "Aluno 1",
                    TurmaId = TURMA_ID_1,
                    Questoes = ObterQuestoesItineranciaAluno()
                },
                new ItineranciaAlunoDto()
                {
                    AlunoCodigo = ALUNO_CODIGO_2,
                    NomeAlunoComTurmaModalidade = "Aluno 2",
                    TurmaId = TURMA_ID_1,
                    Questoes = ObterQuestoesItineranciaAluno()
                },
            };
        }

        private List<ItineranciaAlunoQuestaoDto> ObterQuestoesItineranciaAluno()
        {
            return new List<ItineranciaAlunoQuestaoDto>()
            {
                new ItineranciaAlunoQuestaoDto()
                {
                    Descricao = "Descritivo do estudante",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_DESCRITIVO_ESTUDANTE,
                    Resposta = "Teste Descritivo do estudante",
                },
                new ItineranciaAlunoQuestaoDto()
                {
                    Descricao = "Acompanhamento da situação",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_ALUNO,
                    Resposta = "Teste Acompanhamento da situação Aluno",
                },
                new ItineranciaAlunoQuestaoDto()
                {
                    Descricao = "Encaminhamentos",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ENCAMINHAMENTOS_ALUNO,
                    Resposta = "Teste Encaminhamentos Aluno"
                }
            };
        }

        private ItineranciaDto ObterItineranciaDTO(List<ItineranciaObjetivoDto> ObjetivosVisita = null, List<ItineranciaQuestaoDto> Questoes = null, 
                                                   List<ItineranciaAlunoDto> Alunos = null)
        {
            var itinerancia = new ItineranciaDto
            {
                Id = 1,
                CriadoRF = SISTEMA_CODIGO_RF,
                DataRetornoVerificacao = DateTimeExtension.HorarioBrasilia().Date.AddDays(5),
                DataVisita = DateTimeExtension.HorarioBrasilia().Date,
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                AnoLetivo = 2023
            } ;

            if (ObjetivosVisita != null)
                itinerancia.ObjetivosVisita = ObjetivosVisita;
            if (Questoes != null)
                itinerancia.Questoes = Questoes;
            if (Alunos != null)
                itinerancia.Alunos = Alunos;

            return itinerancia;
        }


    }
    }