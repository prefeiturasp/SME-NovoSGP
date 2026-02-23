using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtribuicaoCJ
{
    public class SalvarAtribuicaoCJUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarAtribuicaoCJUseCase useCase;
        private readonly AtribuicaoCJPersistenciaDto persistenciaDto;
        public static class TesteControl
        {
            public static readonly int AnoAtual = 2026;
        }

        public SalvarAtribuicaoCJUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarAtribuicaoCJUseCase(mediatorMock.Object);

            persistenciaDto = new AtribuicaoCJPersistenciaDto
            {
                AnoLetivo = TesteControl.AnoAtual.ToString(),
                TurmaId = "111",
                UeId = "U001",
                UsuarioRf = "12345",
                DreId = "D001",
                Modalidade = Modalidade.Fundamental,
                Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
                {
                    new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = true },
                    new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 102, Substituir = false }
                }
            };

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Dominio.AtribuicaoCJ>());
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma { Id = 500 });
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterProfessoresTitularesPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProfessorTitularDisciplinaEol>());
            mediatorMock.Setup(x => x.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "RF_LOGADO" });
            mediatorMock.Setup(x => x.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync("RF_LOGADO");
            mediatorMock.Setup(x => x.Send(It.IsAny<RemoverChaveCacheCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            mediatorMock.Setup(x => x.Send(It.IsAny<InserirAtribuicaoCJCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            mediatorMock.Setup(x => x.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
        }

        [Fact]
        public async Task Executar_Quando_AnoAtual_E_Substituir_True_Deve_Executar_Fluxo_Completo_E_Publicar_Inclusao()
        {
            persistenciaDto.Modalidade = Modalidade.Fundamental;
            persistenciaDto.Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = true }
            };

            await useCase.Executar(persistenciaDto);

            mediatorMock.Verify(x => x.Send(It.Is<AtribuirPerfilCommand>(c => c.Perfil == Perfis.PERFIL_CJ), default(CancellationToken)), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.Is<InserirAtribuicaoCJCommand>(c => c.AtribuicaoCJ.DisciplinaId == 101 && !c.ExcluiAbrangencia), default), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<RemoverChaveCacheCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            mediatorMock.Verify(x => x.Send(It.Is<PublicarFilaGoogleClassroomCommand>(c => c.Fila == RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir), default), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Substituir_False_Deve_Executar_Publicar_Remocao_E_ExcluiAbrangencia_True()
        {
            persistenciaDto.TurmaId = "500";

            persistenciaDto.Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = false }
            };

            await useCase.Executar(persistenciaDto);
            mediatorMock.Verify(x => x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediatorMock.Verify(x => x.Send(It.Is<PublicarFilaGoogleClassroomCommand>(c => c.Fila == RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoRemover), CancellationToken.None), Times.Once);

        }

        [Fact]
        public async Task Executar_Quando_Modalidade_Infantil_Deve_Atribuir_Perfil_CJ_Infantil()
        {
            persistenciaDto.Modalidade = Modalidade.EducacaoInfantil;
            persistenciaDto.Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = true }
            };

            await useCase.Executar(persistenciaDto);

            mediatorMock.Verify(x => x.Send(It.Is<AtribuirPerfilCommand>(c => c.Perfil == Perfis.PERFIL_CJ_INFANTIL), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ano_Nao_Atual_Deve_Ignorar_Publicacao_No_Google()
        {
            persistenciaDto.AnoLetivo = (TesteControl.AnoAtual - 1).ToString();
            persistenciaDto.Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = true }
            };

            await useCase.Executar(persistenciaDto);

            mediatorMock.Verify(x => x.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task AtribuirPerfilCJ_Quando_Ja_Atribuiu_Deve_Retornar_True_E_Ignorar_Chamadas()
        {
            persistenciaDto.Modalidade = Modalidade.Fundamental;
            persistenciaDto.Disciplinas = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 101, Substituir = true },
                new AtribuicaoCJPersistenciaItemDto { DisciplinaId = 102, Substituir = true }
            };

            await useCase.Executar(persistenciaDto);

            mediatorMock.Verify(x => x.Send(It.IsAny<AtribuirPerfilCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<RemoverPerfisUsuarioAtualCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task PublicarAtribuicaoNoGoogleClassroomApiAsync_Quando_RF_Invalido_Deve_Salvar_Log()
        {
            persistenciaDto.UsuarioRf = "NAO_NUMERICO";
            var atribuicao = new Dominio.AtribuicaoCJ { ProfessorRf = "NAO_NUMERICO", TurmaId = "T001", DisciplinaId = 101, Substituir = true };

            var metodo = useCase.GetType().GetMethod("PublicarAtribuicaoNoGoogleClassroomApiAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)metodo.Invoke(useCase, new object[] { atribuicao });

            mediatorMock.Verify(x => x.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("O RF informado é inválido.")), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task PublicarAtribuicaoNoGoogleClassroomApiAsync_Quando_TurmaId_Invalido_Deve_Salvar_Log()
        {
            persistenciaDto.TurmaId = "NAO_NUMERICO";
            var atribuicao = new Dominio.AtribuicaoCJ { ProfessorRf = "12345", TurmaId = "NAO_NUMERICO", DisciplinaId = 101, Substituir = true };

            var metodo = useCase.GetType().GetMethod("PublicarAtribuicaoNoGoogleClassroomApiAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)metodo.Invoke(useCase, new object[] { atribuicao });

            mediatorMock.Verify(x => x.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("A turma informada é inválida.")), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(x => x.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task PublicarAtribuicaoNoGoogleClassroomApiAsync_Quando_Falha_Publicar_Fila_Deve_Salvar_Log()
        {
            var atribuicao = new Dominio.AtribuicaoCJ { ProfessorRf = "12345", TurmaId = "111", DisciplinaId = 101, Substituir = true };
            mediatorMock.Setup(x => x.Send(It.IsAny<PublicarFilaGoogleClassroomCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var metodo = useCase.GetType().GetMethod("PublicarAtribuicaoNoGoogleClassroomApiAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);     
            await (Task)metodo.Invoke(useCase, new object[] { atribuicao });

            mediatorMock.Verify(x => x.Send(It.Is<PublicarFilaGoogleClassroomCommand>(c => c.Fila == RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(x => x.Send(It.Is<SalvarLogViaRabbitCommand>(c => c.Mensagem.Contains("Não foi possível publicar na fila")), It.IsAny<CancellationToken>()), Times.Once);
        }    
    }
}
