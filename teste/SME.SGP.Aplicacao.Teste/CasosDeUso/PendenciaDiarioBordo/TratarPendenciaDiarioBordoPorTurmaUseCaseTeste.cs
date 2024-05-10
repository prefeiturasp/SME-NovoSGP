using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class TratarPendenciaDiarioBordoPorTurmaUseCaseTeste
    {
        private readonly TratarPendenciaDiarioBordoPorTurmaUseCase tratarPendenciaDiarioBordoPorTurmaUseCase;
        private readonly Mock<IMediator> mediator;

        public TratarPendenciaDiarioBordoPorTurmaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            tratarPendenciaDiarioBordoPorTurmaUseCase = new TratarPendenciaDiarioBordoPorTurmaUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Publicar_Fila_Por_Turma()
        {
            // arrange
            IEnumerable<Turma> turmasDreUe = new List<Turma>()
            {
                new Turma()
                {
                    CodigoTurma = "123123",
                    ModalidadeCodigo = Modalidade.EducacaoInfantil,
                    Ue = new Ue()
                    {
                        TipoEscola = TipoEscola.CEMEI,
                        Nome = "CEMEI TESTE",
                        Dre = new Dre()
                        {
                            Abreviacao = "DRE"
                        }
                    }
                }
            };
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmasDreUePorCodigosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasDreUe);

            var componentesSgp = new List<ComponenteCurricularDto>()
            {
                new ComponenteCurricularDto()
                {
                    Codigo = "512",
                    Descricao = "ED.INF. EMEI 4 HS"
                }
            };
            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesSgp);

            var listaProfessores = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "111",
                    CodigosDisciplinas = "138",
                    TurmaId = 1
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "222",
                    CodigosDisciplinas = "138",
                    TurmaId = 1
                }
            };

            mediator.Setup(a =>
                    a.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaProfessores);

            var componentesEol = new List<ComponenteCurricularEol>();
            componentesEol.Add(new ComponenteCurricularEol()
            {
                TurmaCodigo = "512",
                Descricao = "Regência de Classe Infantil",
                Codigo = 512
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesEol);

            var aulaComponente = new List<AulaComComponenteDto>();
            aulaComponente.Add(new AulaComComponenteDto()
            {
                Id = 1,
                TurmaId = "123123"
            });

            mediator.Setup(a => a.Send(It.IsAny<ObterPendenciasDiarioBordoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(aulaComponente);

            mediator.Setup(a => a.Send(It.IsAny<SalvarPendenciaDiarioBordoCommand>(), It.IsAny<CancellationToken>()));

            // act
            var retorno = await tratarPendenciaDiarioBordoPorTurmaUseCase.Executar(new MensagemRabbit("2386241"));

            // assert
            Assert.True(retorno, "Pendência tratada e salva com sucesso!");
        }
    }
}