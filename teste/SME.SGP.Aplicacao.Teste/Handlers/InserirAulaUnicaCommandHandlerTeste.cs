using MediatR;
using Moq;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class InserirAulaUnicaCommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly InserirAulaUnicaCommandHandler inserirAulaUnicaCommandHandler;

        public InserirAulaUnicaCommandHandlerTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.repositorioAula = new Mock<IRepositorioAula>();
            this.inserirAulaUnicaCommandHandler = new InserirAulaUnicaCommandHandler(repositorioAula.Object, mediator.Object); ;
        }

        [Fact]
        public async Task Deve_Inserir_Aula()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_PROFESSOR };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(true);
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 1, "1234", 139, "Arte", 123, TipoAula.Normal, "321", false);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AulaConsultaDto>)null);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken());

            // Assert
            repositorioAula.Verify(x => x.SalvarAsync(It.IsAny<Aula>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);
        }

        [Fact]
        public async Task Dia_Nao_Letivo_Nao_Deve_Inserir_Aula()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", };
            var aulasConsulta = new List<AulaConsultaDto>();
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(false, "Apenas é possível consultar este registro pois existe um evento de dia não letivo");
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 1, "1234", 139, "Arte", 123, TipoAula.Normal, "321", false);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AulaConsultaDto>)null);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Assert
            await Assert.ThrowsAsync<NegocioException>(() => inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken()));
        }

        [Fact]
        public async Task ProfessorCJ_Ja_Existe_Aula_Cadastrada_Nao_Deve_Inserir_Aula()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_CJ };
            var aulasConsulta = new List<AulaConsultaDto>() { new AulaConsultaDto() { Id = 1, TipoAula = TipoAula.Normal, ProfessorRf = "1234" } };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(false);
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 1, "1234", 139, "Arte", 123, TipoAula.Normal, "321", false);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AulaConsultaDto>)null);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Assert
            await Assert.ThrowsAsync<NegocioException>(() => inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken()));
        }

        [Fact]
        public async Task ProfessorCJ_Nao_Possui_Atibuicao_Nao_Deve_Inserir_Aula()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_CJ };
            var aulasConsulta = new List<AulaConsultaDto>() { new AulaConsultaDto() { Id = 1, TipoAula = TipoAula.Normal, ProfessorRf = "1234" } };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(false);
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 1, "1234", 139, "Arte", 123, TipoAula.Normal, "321", false);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AulaConsultaDto>)null);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<PossuiAtribuicaoCJPorDreUeETurmaQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(false);

            mediator.Setup(a => a.Send(It.IsAny<ObterAtribuicoesPorRFEAnoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(Enumerable.Empty<AtribuicaoEsporadica>());

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Assert
            await Assert.ThrowsAsync<NegocioException>(() => inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken()));
        }

        [Fact]
        public async Task Diretor_Deve_Inserir_Aula_Reposicao()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_DIRETOR };
            var aulasConsulta = new List<AulaConsultaDto>() { new AulaConsultaDto() { Id = 1, TipoAula = TipoAula.Reposicao, ProfessorRf = "1234" } };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(true);
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 1, "1234", 139, "Arte", 123, TipoAula.Reposicao, "321", false);

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<AulaConsultaDto>)null);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<PossuiAtribuicaoCJPorDreUeETurmaQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(false);

            mediator.Setup(a => a.Send(It.IsAny<ObterAtribuicoesPorRFEAnoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(Enumerable.Empty<AtribuicaoEsporadica>());

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken());

            // Assert
            repositorioAula.Verify(x => x.SalvarAsync(It.IsAny<Aula>()), Times.Once);
            Assert.True(auditoriaDto.Id > 0);
        }

        [Fact]
        public async Task Professor_Deve_Cadastrar_Aula_Reposicao_E_Enviar_Para_Aprovacao()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_PROFESSOR };
            var aulasConsulta = new List<AulaConsultaDto>() { new AulaConsultaDto() { Id = 1, TipoAula = TipoAula.Reposicao, ProfessorRf = "1234" } };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var podeCadastrarAula = new PodeCadastrarAulaPorDataRetornoDto(true);
            var inserirAulaUnicaCommand = new InserirAulaUnicaCommand(usuario, DateTime.Today, 2, "1234", 139, "Arte", 123, TipoAula.Reposicao, "321", false);
            var aulasExistentes = new List<AulaConsultaDto>() { new AulaConsultaDto() { DataAula = DateTime.Today, TipoAula = TipoAula.Normal, Quantidade = 2 } };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulasExistentes);

            mediator.Setup(a => a.Send(It.IsAny<ValidarComponentesDoProfessorCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<ObterPodeCadastrarAulaPorDataQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(podeCadastrarAula);

            mediator.Setup(a => a.Send(It.IsAny<PossuiAtribuicaoCJPorDreUeETurmaQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(false);

            mediator.Setup(a => a.Send(It.IsAny<ObterAtribuicoesPorRFEAnoQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(Enumerable.Empty<AtribuicaoEsporadica>());

            mediator.Setup(a => a.Send(It.IsAny<ValidarGradeAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((true, string.Empty));

            mediator.Setup(a => a.Send(It.IsAny<InserirWorkflowReposicaoAulaCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(1);

            repositorioAula.Setup(a => a.SalvarAsync(It.IsAny<Aula>()))
                .ReturnsAsync(1);

            // Act
            var auditoriaDto = await inserirAulaUnicaCommandHandler.Handle(inserirAulaUnicaCommand, new CancellationToken());

            // Assert                     
            Assert.Equal("Aula cadastrada e enviada para aprovação com sucesso.", auditoriaDto.Mensagens.FirstOrDefault());
        }
    }
}
