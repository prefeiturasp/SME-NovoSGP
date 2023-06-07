using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nest;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
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
        protected const string RESPOSTA_ACOMPANHAMENTO_SITUACAO_GERAL = "Teste Acompanhamento da situação";
        protected const string RESPOSTA_ENCAMINHAMENTOS_GERAL = "Teste Encaminhamentos";
        protected const string RESPOSTA_ACOMPANHAMENTO_SITUACAO_ALUNO = "Teste Acompanhamento da situação Aluno";
        protected const string RESPOSTA_ENCAMINHAMENTOS_ALUNO = "Teste Encaminhamentos Aluno";
        protected const string RESPOSTA_DESCRITIVO_ESTUDANTE = "Teste Descritivo do estudante";
        

        public Ao_salvar_registro_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ArmazenarArquivoFisicoCommand, string>),typeof(ArmazenarArquivoFisicoCommandHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Registro de itinerância - Salvar itinerância sem alunos, somente com informações/questões globais")]
        public async Task Salvar_itinerancia_sem_alunos()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            var useCase = SalvarItineranciaUseCase();
            var retorno =  await useCase.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), ObterQuestoesItinerancia()));
            retorno.ShouldNotBeNull("Itinerância não persistida ao salvar");
            retorno.Id.ShouldBe(1, "Id da itinerância persistida deveria ser 1");

            var itinerancias = ObterTodos<Dominio.Itinerancia>();
            itinerancias.ShouldNotBe(null, "Itinerância não persistida ao salvar");
            itinerancias.Count.ShouldBe(1, "Quantidade de itinerâncias persistidas ao salvar incorreta");

            var objetivosItinerancia = ObterTodos<Dominio.ItineranciaObjetivo>();
            objetivosItinerancia.ShouldNotBe(null, "Objetivos da Itinerância não persistida ao salvar");
            objetivosItinerancia.Count.ShouldBe(3, "Quantidade de objetivos na itinerância persistida ao salvar incorreta");

            var questoesItinerancia = ObterTodos<Dominio.ItineranciaQuestao>();
            questoesItinerancia.ShouldNotBe(null, "Questões/Repostas da Itinerância não persistidas ao salvar");
            questoesItinerancia.Count.ShouldBe(2, "Quantidade de questões na itinerância persistida ao salvar incorreta");

            var itineranciasAluno = ObterTodos<Dominio.ItineranciaAluno>();
            itineranciasAluno.ShouldNotBe(null, "Itinerâncias Alunos persistidas deveria ser null");
        }

        [Fact(DisplayName = "Registro de itinerância - Salvar itinerância com alunos, sem informações/questões globais")]
        public async Task Salvar_itinerancia_com_alunos()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            var useCase = SalvarItineranciaUseCase();
            var retorno = await useCase.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), null, ObterItineranciasAluno()));
            retorno.ShouldNotBeNull("Itinerância não persistida ao salvar");
            retorno.Id.ShouldBe(1, "Id da itinerância persistida deveria ser 1");

            var itinerancias = ObterTodos<Dominio.Itinerancia>();
            itinerancias.ShouldNotBe(null, "Itinerância não persistida ao salvar");
            itinerancias.Count.ShouldBe(1, "Quantidade de itinerâncias persistidas ao salvar incorreta");

            var objetivosItinerancia = ObterTodos<Dominio.ItineranciaObjetivo>();
            objetivosItinerancia.ShouldNotBe(null, "Objetivos da Itinerância não persistida ao salvar");
            objetivosItinerancia.Count.ShouldBe(3, "Quantidade de objetivos na itinerância persistida ao salvar incorreta");

            var questoesItinerancia = ObterTodos<Dominio.ItineranciaQuestao>();
            questoesItinerancia.ShouldNotBe(null, "Questões/Repostas da Itinerância persistida deveria ser null");

            var itineranciasAluno = ObterTodos<Dominio.ItineranciaAluno>();
            itineranciasAluno.ShouldNotBe(null, "Itinerância por alunos da Itinerância não persistidas ao salvar");
            itineranciasAluno.Count.ShouldBe(2, "Quantidade de Itinerâncias por aluno na itinerância persistida ao salvar incorreta");

            var questoesItineranciasAluno = ObterTodos<Dominio.ItineranciaAlunoQuestao>();
            questoesItineranciasAluno.ShouldNotBe(null, "Questões das itinerâncias por alunos não persistidas ao salvar");
            questoesItineranciasAluno.Count.ShouldBe(6, "Quantidade de Questões das itinerâncias por aluno persistidas ao salvar incorreta");

            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_ACOMPANHAMENTO_SITUACAO_ALUNO).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_ALUNO}]");
            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_DESCRITIVO_ESTUDANTE).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_DESCRITIVO_ESTUDANTE}]");
            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_ENCAMINHAMENTOS_ALUNO).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ENCAMINHAMENTOS_ALUNO}]");
        }

        [Fact(DisplayName = "Registro de itinerância Misto - Salvar itinerância com alunos e informações/questões globais")]
        public async Task Salvar_itinerancia_com_alunos_e_questoes_globais()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            var useCase = SalvarItineranciaUseCase();
            var retorno = await useCase.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), ObterQuestoesItinerancia(), ObterItineranciasAluno()));
            retorno.ShouldNotBeNull("Itinerância não persistida ao salvar");
            retorno.Id.ShouldBe(1, "Id da itinerância persistida deveria ser 1");

            var itinerancias = ObterTodos<Dominio.Itinerancia>();
            itinerancias.ShouldNotBe(null, "Itinerância não persistida ao salvar");
            itinerancias.Count.ShouldBe(1, "Quantidade de itinerâncias persistidas ao salvar incorreta");

            var objetivosItinerancia = ObterTodos<Dominio.ItineranciaObjetivo>();
            objetivosItinerancia.ShouldNotBe(null, "Objetivos da Itinerância não persistida ao salvar");
            objetivosItinerancia.Count.ShouldBe(3, "Quantidade de objetivos na itinerância persistida ao salvar incorreta");

            var questoesItinerancia = ObterTodos<Dominio.ItineranciaQuestao>();
            questoesItinerancia.ShouldNotBe(null, "Questões/Repostas da Itinerância não persistidas ao salvar");
            questoesItinerancia.Count.ShouldBe(2, "Quantidade de questões na itinerância persistida ao salvar incorreta");
            questoesItinerancia.Where(resposta => resposta.Resposta == RESPOSTA_ACOMPANHAMENTO_SITUACAO_GERAL).Count().ShouldBe(1, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_GERAL}]");
            questoesItinerancia.Where(resposta => resposta.Resposta == RESPOSTA_ENCAMINHAMENTOS_GERAL).Count().ShouldBe(1, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ENCAMINHAMENTOS_GERAL}]");

            var itineranciasAluno = ObterTodos<Dominio.ItineranciaAluno>();
            itineranciasAluno.ShouldNotBe(null, "Itinerância por alunos da Itinerância não persistidas ao salvar");
            itineranciasAluno.Count.ShouldBe(2, "Quantidade de Itinerâncias por aluno na itinerância persistida ao salvar incorreta");

            var questoesItineranciasAluno = ObterTodos<Dominio.ItineranciaAlunoQuestao>();
            questoesItineranciasAluno.ShouldNotBe(null, "Questões das itinerâncias por alunos não persistidas ao salvar");
            questoesItineranciasAluno.Count.ShouldBe(6, "Quantidade de Questões das itinerâncias por aluno persistidas ao salvar incorreta");

            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_ACOMPANHAMENTO_SITUACAO_ALUNO).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_ALUNO}]");
            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_DESCRITIVO_ESTUDANTE).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_DESCRITIVO_ESTUDANTE}]");
            questoesItineranciasAluno.Where(resposta => resposta.Resposta == RESPOSTA_ENCAMINHAMENTOS_ALUNO).Count().ShouldBe(2, $"Quantidade de Respostas incorreta para questão [{NOME_COMPONENTE_ENCAMINHAMENTOS_ALUNO}]");
        }

        [Fact(DisplayName = "Registro de itinerância Misto - Editar itinerância com alunos e informações/questões globais limpando as respostas")]
        public async Task Editar_itinerancia_com_alunos_e_questoes_globais()
        {
            await CriarDadosBase(new FiltroItineranciaDto() { AnoTurma = "5", ConsiderarAnoAnterior = false, Modalidade = Modalidade.Fundamental, Perfil = ObterPerfilCoordenadorCefai() });

            var useCaseSalvar = SalvarItineranciaUseCase();
            var retorno = await useCaseSalvar.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), ObterQuestoesItinerancia(), ObterItineranciasAluno()));
            retorno.ShouldNotBeNull("Itinerância não persistida ao salvar");
            retorno.Id.ShouldBe(1, "Id da itinerância persistida deveria ser 1");

            var useCaseAlterar = AlterarItineranciaUseCase();
            retorno = await useCaseAlterar.Executar(ObterItineranciaDTO(ObterObjetivosVisita(), ObterQuestoesItinerancia(""), ObterItineranciasAluno("Nova resposta")));

            var itinerancias = ObterTodos<Dominio.Itinerancia>();
            itinerancias.ShouldNotBe(null, "Itinerância não persistida ao salvar");
            itinerancias.Count.ShouldBe(1, "Quantidade de itinerâncias persistidas ao salvar incorreta");

            var questoesItinerancia = ObterTodos<Dominio.ItineranciaQuestao>();
            questoesItinerancia.ShouldNotBe(null, "Questões/Repostas da Itinerância não persistidas ao salvar");
            questoesItinerancia.Where(q => !q.Excluido).Count().ShouldBe(2, "Quantidade de questões na itinerância persistida ao salvar incorreta");
            questoesItinerancia.Where(resposta => string.IsNullOrEmpty(resposta.Resposta) && !resposta.Excluido).Count().ShouldBe(2, $"Quantidade de Respostas vazias incorreta para questão");
            
            var itineranciasAluno = ObterTodos<Dominio.ItineranciaAluno>();
            itineranciasAluno.ShouldNotBe(null, "Itinerância por alunos da Itinerância não persistidas ao salvar");
            itineranciasAluno.Where(q => !q.Excluido).Count().ShouldBe(2, "Quantidade de Itinerâncias por aluno na itinerância persistida ao salvar incorreta");

            var questoesItineranciasAluno = ObterTodos<Dominio.ItineranciaAlunoQuestao>();
            questoesItineranciasAluno.ShouldNotBe(null, "Questões das itinerâncias por alunos não persistidas ao salvar");
            questoesItineranciasAluno.Where(q => !q.Excluido).Count().ShouldBe(6, "Quantidade de Questões das itinerâncias por aluno persistidas ao salvar incorreta");

            questoesItineranciasAluno.Where(resposta => resposta.Resposta.Equals("Nova resposta") && !resposta.Excluido).Count().ShouldBe(6, $"Quantidade de Respostas vazias incorreta para questão");
        }

        private List<ItineranciaObjetivoDto> ObterObjetivosVisita()
        {
            return new List<ItineranciaObjetivoDto>()
            {
                new ItineranciaObjetivoDto()
                {
                    Id = 1,
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_MAPEAMENTO_ESTUDANTES_PUBLICO_EE,
                    TemDescricao = false
                },
                new ItineranciaObjetivoDto()
                {
                    Id = 2,
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_ATENDIMENTO_SOLICITACAO_UE,
                    TemDescricao = true,
                    Descricao = "Descrição base atendimento solicitação Ue"
                },
                new ItineranciaObjetivoDto()
                {
                    Id = 3,
                    ItineranciaObjetivoBaseId = ID_OBJ_BASE_REUNIAO,
                    TemDescricao = false
                },
            };
        }

        private List<ItineranciaQuestaoDto> ObterQuestoesItinerancia(string resposta = null)
        {
            return new List<ItineranciaQuestaoDto>()
            {
                new ItineranciaQuestaoDto()
                {
                    Id = 1,
                    ItineranciaId = 1,
                    Descricao = "Acompanhamento da situação",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_GERAL,
                    TipoQuestao = TipoQuestao.Texto,
                    Resposta = resposta ?? RESPOSTA_ACOMPANHAMENTO_SITUACAO_GERAL
                },
                new ItineranciaQuestaoDto()
                {
                    Id = 2,
                    ItineranciaId = 1,
                    Descricao = "Encaminhamentos",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ENCAMINHAMENTOS_GERAL,
                    TipoQuestao = TipoQuestao.Texto,
                    Resposta = resposta ?? RESPOSTA_ENCAMINHAMENTOS_GERAL
                }
            };
        }

        private List<ItineranciaAlunoDto> ObterItineranciasAluno(string resposta = null)
        {
            return new List<ItineranciaAlunoDto>()
            {
                new ItineranciaAlunoDto()
                {
                    Id = 1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    NomeAlunoComTurmaModalidade = "Aluno 1",
                    TurmaId = TURMA_ID_1,
                    Questoes = ObterQuestoesItineranciaAluno(1, resposta)
                },
                new ItineranciaAlunoDto()
                {   Id = 2,
                    AlunoCodigo = ALUNO_CODIGO_2,
                    NomeAlunoComTurmaModalidade = "Aluno 2",
                    TurmaId = TURMA_ID_1,
                    Questoes = ObterQuestoesItineranciaAluno(2, resposta)
                },
            };
        }

        private List<ItineranciaAlunoQuestaoDto> ObterQuestoesItineranciaAluno(int idItineranciaAluno = 1, string resposta = null)
        {
            return new List<ItineranciaAlunoQuestaoDto>()
            {
                new ItineranciaAlunoQuestaoDto()
                {
                    ItineranciaAlunoId = idItineranciaAluno,
                    Descricao = "Descritivo do estudante",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_DESCRITIVO_ESTUDANTE,
                    Resposta = resposta ?? RESPOSTA_DESCRITIVO_ESTUDANTE,
                },
                new ItineranciaAlunoQuestaoDto()
                {
                    ItineranciaAlunoId = idItineranciaAluno,
                    Descricao = "Acompanhamento da situação",
                    Obrigatorio = true,
                    QuestaoId = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_ALUNO,
                    Resposta = resposta ?? RESPOSTA_ACOMPANHAMENTO_SITUACAO_ALUNO,
                },
                new ItineranciaAlunoQuestaoDto()
                {
                    ItineranciaAlunoId = idItineranciaAluno,
                    Descricao = "Encaminhamentos",
                    Obrigatorio = false,
                    QuestaoId = ID_QUESTAO_ENCAMINHAMENTOS_ALUNO,
                    Resposta = resposta ?? RESPOSTA_ENCAMINHAMENTOS_ALUNO
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