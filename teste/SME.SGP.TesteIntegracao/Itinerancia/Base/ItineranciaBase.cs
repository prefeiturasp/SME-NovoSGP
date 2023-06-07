using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.Itinerancia.Base
{
    public class ItineranciaBase  : TesteBaseComuns
    {
        protected const long ID_QUESTIONARIO_ITINERANCIA = 1;
        protected const long ID_QUESTIONARIO_ITINERANCIA_ALUNO = 2;
        protected const string NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_GERAL = "ACOMPANHAMENTO_SITUACAO_GERAL";
        protected const string NOME_COMPONENTE_ENCAMINHAMENTOS_GERAL = "ENCAMINHAMENTOS_GERAL";
        protected const string NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_ALUNO = "ACOMPANHAMENTO_SITUACAO_ALUNO";
        protected const string NOME_COMPONENTE_ENCAMINHAMENTOS_ALUNO = "ENCAMINHAMENTOS_ALUNO";
        protected const string NOME_COMPONENTE_DESCRITIVO_ESTUDANTE = "DESCRITIVO_ESTUDANTE";

        protected const int ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_GERAL = 1;
        protected const int ID_QUESTAO_ENCAMINHAMENTOS_GERAL = 2;
        protected const int ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_ALUNO = 3;
        protected const int ID_QUESTAO_ENCAMINHAMENTOS_ALUNO = 4;
        protected const int ID_QUESTAO_DESCRITIVO_ESTUDANTE = 5;

        protected const int ID_OBJ_BASE_MAPEAMENTO_ESTUDANTES_PUBLICO_EE = 1;
        protected const int ID_OBJ_BASE_REORGANIZACAO_REMANEJAMENTO_APOIOS_SERVICOS = 2;
        protected const int ID_OBJ_BASE_ATENDIMENTO_SOLICITACAO_UE = 3;
        protected const int ID_OBJ_BASE_ACOMPANHAMENTO_PROFESSOR_SALA_REGULAR = 4;
        protected const int ID_OBJ_BASE_ACOMPANHAMENTO_PROFESSOR_SRM = 5;
        protected const int ID_OBJ_BASE_ACAO_FORMATIVA_JEIF = 6;
        protected const int ID_OBJ_BASE_REUNIAO = 7;
        protected const int ID_OBJ_BASE_OUTROS = 8;


        public ItineranciaBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected IExcluirArquivoItineranciaUseCase ExcluirArquivoItineranciaUseCase()
        {
            return ServiceProvider.GetService<IExcluirArquivoItineranciaUseCase>();
        }
        
        protected IUploadDeArquivoItineranciaUseCase UploadDeArquivoItineranciaUseCase()
        {
            return ServiceProvider.GetService<IUploadDeArquivoItineranciaUseCase>();
        }        
        protected IExcluirArmazenamentoPorAquivoUseCase ExcluirArmazenamentoPorAquivoUseCase()
        {
            return ServiceProvider.GetService<IExcluirArmazenamentoPorAquivoUseCase>();
        }

        protected ISalvarItineranciaUseCase SalvarItineranciaUseCase()
        {
            return ServiceProvider.GetService<ISalvarItineranciaUseCase>();
        }

        protected IAlterarItineranciaUseCase AlterarItineranciaUseCase()
        {
            return ServiceProvider.GetService<IAlterarItineranciaUseCase>();
        }
        
        protected async Task<IFormFile> GerarAquivoFakeParaUpload(string nomeArquivo = "arquivo.png",string extensaoArquivo = "image/png")
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Fake File";
            var fileName = nomeArquivo;
            var contentType = extensaoArquivo;
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            await writer.WriteAsync(content);
            await writer.FlushAsync();
            
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock.Object;
        }

        protected async Task CriarDadosBase(FiltroItineranciaDto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();
            CriarClaimUsuario(filtro.Perfil);
            await CriarUsuarios();
            if (filtro.CriarTurmaPadrao)
                await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior, tipoTurno: 2);
            await CriarQuestionario();
            await CriarQuestoes();
            await CriarObjetivosBase();
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Registro Itinerância",
                Tipo = TipoQuestionario.RegistroItinerancia,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTIONARIO_ITINERANCIA
                });
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Registro Itinerância do Aluno",
                Tipo = TipoQuestionario.RegistroItineranciaAluno,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTIONARIO_ITINERANCIA_ALUNO
            });
        }

        private async Task CriarQuestoes()
        {
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_ITINERANCIA,
                Ordem = 0,
                Nome = "Acompanhamento da situação",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_GERAL,
                NomeComponente = NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_GERAL
            });
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_ITINERANCIA,
                Ordem = 1,
                Nome = "Encaminhamentos",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTAO_ENCAMINHAMENTOS_GERAL,
                NomeComponente = NOME_COMPONENTE_ENCAMINHAMENTOS_GERAL
            });

            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_ITINERANCIA_ALUNO,
                Ordem = 0,
                Nome = "Descritivo do estudante",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTAO_DESCRITIVO_ESTUDANTE,
                NomeComponente = NOME_COMPONENTE_DESCRITIVO_ESTUDANTE
            });
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_ITINERANCIA_ALUNO,
                Ordem = 1,
                Nome = "Acompanhamento da situação",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTAO_ACOMPANHAMENTO_SITUACAO_ALUNO,
                NomeComponente = NOME_COMPONENTE_ACOMPANHAMENTO_SITUACAO_ALUNO
            });
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_ITINERANCIA_ALUNO,
                Ordem = 2,
                Nome = "Encaminhamentos",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Id = ID_QUESTAO_ENCAMINHAMENTOS_ALUNO,
                NomeComponente = NOME_COMPONENTE_ENCAMINHAMENTOS_ALUNO
            });
        }

        private async Task CriarObjetivosBase()
        {
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Mapeamento dos estudantes público da Educação Especial",
                Ordem = 0,
                TemDescricao = false,
                Id = ID_OBJ_BASE_MAPEAMENTO_ESTUDANTES_PUBLICO_EE
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Reorganização e / ou remanejamento de apoios e serviços",
                Ordem = 10,
                TemDescricao = false,
                Id = ID_OBJ_BASE_REORGANIZACAO_REMANEJAMENTO_APOIOS_SERVICOS
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Atendimento de solicitação da U.E",
                Ordem = 20,
                TemDescricao = true,
                Id = ID_OBJ_BASE_ATENDIMENTO_SOLICITACAO_UE
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Acompanhamento professor de sala regular",
                Ordem = 30,
                TemDescricao = false,
                Id = ID_OBJ_BASE_ACOMPANHAMENTO_PROFESSOR_SALA_REGULAR
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Acompanhamento professor de SRM",
                Ordem = 40,
                TemDescricao = false,
                Id = ID_OBJ_BASE_ACOMPANHAMENTO_PROFESSOR_SRM
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Ação Formativa em JEIF",
                Ordem = 50,
                TemDescricao = false,
                Id = ID_OBJ_BASE_ACAO_FORMATIVA_JEIF
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Reunião",
                Ordem = 60,
                TemDescricao = false,
                Id = ID_OBJ_BASE_REUNIAO
            });
            await InserirNaBase(new Dominio.ItineranciaObjetivoBase()
            {
                Nome = "Outros",
                Ordem = 70,
                TemDescricao = true,
                Id = ID_OBJ_BASE_OUTROS
            });
        }

        protected class FiltroItineranciaDto
        {
            public FiltroItineranciaDto()
            {}
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public bool CriarTurmaPadrao { get; set; } = true;
        }
    }
}