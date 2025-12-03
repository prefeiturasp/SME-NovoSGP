using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Itinerancia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.SecaoItinerancia
{
    public class Ao_manter_arquivos_anexo_encaminhamento_itinerancia : EncaminhamentoNAAPATesteBase
    {
        private Mock<IFormFile> fileMock;
        public Ao_manter_arquivos_anexo_encaminhamento_itinerancia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            fileMock = new Mock<IFormFile>();
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ArmazenarArquivoFisicoCommand, string>), typeof(ArmazenarArquivoFisicoCommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Itinerância - Removendo arquivo/anexo")]
        public async Task Ao_remover_arquivo_anexo_itinerancia()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            await GerarDadosEncaminhamentoNAAPA(DateTimeExtension.HorarioBrasilia().Date);
            await GerarDadosEncaminhamentoNAAPAItinerarioComAnexos(DateTimeExtension.HorarioBrasilia().Date);
            var arquivo = ObterTodos<Arquivo>().FirstOrDefault();
            var useCase = ObterServicoExcluirArquivoItineranciaNAAPAUseCase();
            await useCase.Executar(arquivo.Codigo);

            var arquivos = ObterTodos<Arquivo>();

            var questoes = ObterTodos<QuestaoEncaminhamentoNAAPA>();
            var questaoAnexo = questoes.Find(questao => questao.QuestaoId == ID_QUESTAO_ANEXOS_ITINERANCIA);
            questaoAnexo.ShouldNotBeNull();
            var respostas = ObterTodos<RespostaEncaminhamentoNAAPA>();
            var respostaAnexo = respostas.Where(resposta => resposta.QuestaoEncaminhamentoId == questaoAnexo.Id);
            respostaAnexo.Any(r => r.Excluido).ShouldBeTrue();
            respostaAnexo.Any(r => !r.Excluido && !r.ArquivoId.Equals(arquivo.Id)).ShouldBeTrue();

            arquivos.Count().ShouldBe(1);
        }

        [Fact(DisplayName = "Encaminhamento NAAPA Itinerância - Upload arquivo/anexo")]
        public async Task Ao_subir_arquivo_anexo_itinerancia()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.AguardandoAtendimento,
                Prioridade = NORMAL
            };

            await CriarDadosBase(filtroNAAPA);
            var useCase = ObterServicoUploadDeArquivoUseCase();
            fileMock.Setup(_ => _.FileName).Returns("Arquivo 1");
            fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
            fileMock.Setup(_ => _.Length).Returns(100);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", "Arquivo 1"));
            await useCase.Executar(fileMock.Object, TipoArquivo.ItineranciaAtendimentoNAAPA);

            var arquivos = ObterTodos<Arquivo>();
            arquivos.Count().ShouldBe(1);
            arquivos.FirstOrDefault().Nome.ShouldBe("Arquivo 1");
            arquivos.FirstOrDefault().TipoConteudo.ShouldBe("application/pdf");
        }

        private async Task GerarDadosEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPA();
            await CriarEncaminhamentoNAAPASecao();
            await CriarQuestoesEncaminhamentoNAAPA();
            await CriarRespostasEncaminhamentoNAAPA(dataQueixa);
        }


        private async Task GerarDadosEncaminhamentoNAAPAItinerarioComAnexos(DateTime dataQueixa)
        {
            await GerarDadosEncaminhamentoNAAPAItinerario(dataQueixa);

            //Anexos - Itinerancia 
            await InserirNaBase(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo 1 Itinerância NAAPA",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
            });
            await InserirNaBase(new Arquivo()
            {
                Codigo = Guid.NewGuid(),
                Nome = $"Arquivo 2 Itinerância NAAPA",
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                TipoConteudo = "application/pdf",
                Tipo = TipoArquivo.ItineranciaAtendimentoNAAPA
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 7,
                Texto = "",
                ArquivoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 7,
                Texto = "",
                ArquivoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task GerarDadosEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await CriarEncaminhamentoNAAPASecaoItinerario();
            await CriarQuestoesEncaminhamentoNAAPAItinerario();
            await CriarRespostasEncaminhamentoNAAPAItinerario(dataQueixa);
        }

        private async Task CriarQuestoesEncaminhamentoNAAPA()
        {
            //Id 1
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 2
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 1,
                QuestaoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarQuestoesEncaminhamentoNAAPAItinerario()
        {
            //Id 3
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DATA_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 4
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 5
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 6
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_DESCRICAO_ATENDIMENTO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            //Id 7
            await InserirNaBase(new QuestaoEncaminhamentoNAAPA()
            {
                EncaminhamentoNAAPASecaoId = 2,
                QuestaoId = ID_QUESTAO_ANEXOS_ITINERANCIA,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecao()
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPASecaoItinerario()
        {
            await InserirNaBase(new EncaminhamentoNAAPASecao()
            {
                EncaminhamentoNAAPAId = 1,
                SecaoEncaminhamentoNAAPAId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarEncaminhamentoNAAPA()
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
        }

        private async Task CriarRespostasEncaminhamentoNAAPA(DateTime dataQueixa)
        {
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 1,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 2,
                Texto = "1",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRespostasEncaminhamentoNAAPAItinerario(DateTime dataQueixa)
        {
            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 3,
                Texto = dataQueixa.ToString("dd/MM/yyyy"),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 4,
                RespostaId = ID_ATENDIMENTO_NAO_PRESENCIAL,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 5,
                RespostaId = ID_ACOES_LUDICAS,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new RespostaEncaminhamentoNAAPA()
            {
                QuestaoEncaminhamentoId = 6,
                Texto = "Descrição do atendimento",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
