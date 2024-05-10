using MediatR;
using Moq;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class InserirDiarioBordoUseCaseTeste
    {
        private readonly InserirDiarioBordoUseCase inserirDiarioBordoUseCase;
        private readonly Mock<IConsultasDisciplina> consultaDisciplina;
        private readonly Mock<IRepositorioComponenteCurricularConsulta> repositorioComponenteCurricularConsulta;
        private readonly Mock<IMediator> mediator;

        public InserirDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consultaDisciplina = new Mock<IConsultasDisciplina>();
            repositorioComponenteCurricularConsulta = new Mock<IRepositorioComponenteCurricularConsulta>();
            inserirDiarioBordoUseCase = new InserirDiarioBordoUseCase(mediator.Object, consultaDisciplina.Object);
        }

        [Fact]
        public async Task Deve_Inserir_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<InserirDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Infra.AuditoriaDto()
                {
                    Id = 1
                });
            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.Turma { CodigoTurma = "1"});

            mediator.Setup(x => x.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.Aula { Id = 1, TurmaId = "1" });

            var disciplinaDto = RetornaDisciplinaDto();

            consultaDisciplina.Setup(x => x.ObterComponentesCurricularesPorProfessorETurma("1", false, false, false)).Returns(disciplinaDto);

            //Act
            var auditoriaDto = await inserirDiarioBordoUseCase.Executar(new Infra.InserirDiarioBordoDto()
            {
                AulaId = 1,
                Planejamento = "teste",
                ComponenteCurricularId = 1
            });

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<InserirDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(auditoriaDto.Id == 1);
        }

        private async Task<List<DisciplinaDto>> RetornaDisciplinaDto()
        {
            var listaDisciplinaDto = new List<DisciplinaDto>();
            var disciplinaDto = new DisciplinaDto() { Id = 1, CodigoComponenteCurricular = 1, CdComponenteCurricularPai = 1, Nome = "Matematica", TurmaCodigo = "1" };

            listaDisciplinaDto.Add(disciplinaDto);

            return await Task.FromResult(listaDisciplinaDto);
        }
    }
}
