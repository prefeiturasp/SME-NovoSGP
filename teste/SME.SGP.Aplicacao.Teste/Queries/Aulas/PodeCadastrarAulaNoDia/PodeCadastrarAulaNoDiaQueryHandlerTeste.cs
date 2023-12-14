using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Aulas.PodeCadastrarAulaNoDia
{
    public class PodeCadastrarAulaNoDiaQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly PodeCadastrarAulaNoDiaQueryHandler podeCadastrarAulaNoDiaQueryHandler;

        public PodeCadastrarAulaNoDiaQueryHandlerTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.repositorioAula = new Mock<IRepositorioAula>();
            this.podeCadastrarAulaNoDiaQueryHandler = new PodeCadastrarAulaNoDiaQueryHandler(repositorioAula.Object, mediator.Object);
        }

        [Fact]
        public async Task Deve_Permitir_ProfessoresCJ_Cadastrar_Aula_Para_Mesmo_Componente_No_Mesmo_Dia()
        {
            // Arrange
            var usuario = new Usuario() { Id = 1, CodigoRf = "1234", PerfilAtual = Perfis.PERFIL_CJ, Login = "1234" };
            var turma = new Turma() { CodigoTurma = "1234", Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } } };
            var dreUeDaTurmaDto = new DreUeDaTurmaDto() { DreCodigo = "1", UeCodigo = "321" };
            var componenteCienciasCodigo = new long[] { 89 };
            var dataAula = new DateTime(2023, 11, 14);
            var podeCadastrarAulaNoDiaQuery = new PodeCadastrarAulaNoDiaQuery(dataAula, turma.CodigoTurma, componenteCienciasCodigo, TipoAula.Normal, usuario.CodigoRf);

            var listaAulaConsultaDto = new List<AulaConsultaDto>() {
                new AulaConsultaDto(){Id = 1,AulaCJ = false, CriadoRF = "2222" },
                new AulaConsultaDto(){Id = 2,AulaCJ = true, CriadoRF = "3333"}
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(a => a.Send(It.IsAny<ObterCodigosDreUePorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dreUeDaTurmaDto);

            mediator.Setup(a => a.Send(It.IsAny<ObterPerfilAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario.PerfilAtual);

            mediator.Setup(a => a.Send(It.IsAny<ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAulaConsultaDto);

            mediator.Setup(a => a.Send(It.IsAny<ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            mediator.Setup(a => a.Send(It.IsAny<ValidarSeEhDiaLetivoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            //Act
            var resultado = await podeCadastrarAulaNoDiaQueryHandler.Handle(podeCadastrarAulaNoDiaQuery, new CancellationToken());

            // Assert
            Assert.True(resultado);
        }
    }
}
