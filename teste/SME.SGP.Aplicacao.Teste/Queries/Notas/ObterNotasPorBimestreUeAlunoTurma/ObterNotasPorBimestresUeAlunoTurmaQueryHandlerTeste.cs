using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Notas.ObterNotasPorBimestreUeAlunoTurma
{
    public class ObterNotasPorBimestresUeAlunoTurmaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioConselhoClasseNotaConsulta> repositorioConselhoClasseNotaConsulta;
        private readonly Mock<IMediator> mediator;

        public ObterNotasPorBimestresUeAlunoTurmaQueryHandlerTeste()
        {
            this.repositorioConselhoClasseNotaConsulta = new Mock<IRepositorioConselhoClasseNotaConsulta>();
            this.mediator = new Mock<IMediator>();
        }

        [Fact(DisplayName = "Notas - DeveObterNotasPorBimestreUeAlunoTurmaComComponenteRegencia")]
        public async Task DeveObterNotasPorBimestreUeAlunoTurmaComComponenteRegencia()
        {
            var componentesTurma = new List<DisciplinaResposta>()
            {
                new DisciplinaResposta() { CodigoComponenteCurricular = 1, RegistroFrequencia = true },
                new DisciplinaResposta() { CodigoComponenteCurricular = 2, RegistroFrequencia = true },
                new DisciplinaResposta() { CodigoComponenteCurricular = 3, RegistroFrequencia = false },
                new DisciplinaResposta() { CodigoComponenteCurricular = 4, RegistroFrequencia = true, Regencia = true },
                new DisciplinaResposta() { CodigoComponenteCurricular = 5, RegistroFrequencia = true }
            };

            var componentesRegencia = new List<DisciplinaDto>()
            {
                new DisciplinaDto() { CodigoComponenteCurricular = 6 },
                new DisciplinaDto() { CodigoComponenteCurricular = 7 }
            };

            var notasConceitos = new List<NotaConceitoBimestreComponenteDto>()
            {
                new NotaConceitoBimestreComponenteDto() { ComponenteCurricularCodigo = 2, Nota = 5, Bimestre = 1, AlunoCodigo = "1" },
                new NotaConceitoBimestreComponenteDto() { ComponenteCurricularCodigo = 2, Nota = 7, Bimestre = 2, AlunoCodigo = "1" },
                new NotaConceitoBimestreComponenteDto() { ComponenteCurricularCodigo = 6, ConceitoId = 1, Bimestre = 1, AlunoCodigo = "1" },
                new NotaConceitoBimestreComponenteDto() { ComponenteCurricularCodigo = 6, ConceitoId = 2, Bimestre = 2, AlunoCodigo = "1" }
            };

            var bimestres = new int[] { 1, 2, 3 };

            mediator.Setup(x => x.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(y => y.CodigoTurma == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesTurma);           

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesRegencia);

            repositorioConselhoClasseNotaConsulta.Setup(x => x.ObterNotasBimestresAluno("1", "000001", "1", bimestres))
                .ReturnsAsync(notasConceitos);

            var query = new ObterNotasPorBimestresUeAlunoTurmaQueryHandler(repositorioConselhoClasseNotaConsulta.Object, mediator.Object);

            var resultado = await query
                .Handle(new ObterNotasPorBimestresUeAlunoTurmaQuery(bimestres, "1", "000001", "1"), It.IsAny<CancellationToken>());

            Assert.Equal(12, resultado.Count());
            //1º Bimestre:
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 1).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 1).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 1).ConceitoId);
            Assert.Equal(5, resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 1).Nota);            
            Assert.Equal(1, resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 1).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 1).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 1).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 1).Nota);
            //2º Bimestre:
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 2).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 2).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 2).ConceitoId);
            Assert.Equal(7, resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 2).Nota);
            Assert.Equal(2, resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 2).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 2).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 2).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 2).Nota);
            //3º Bimestre:
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 3).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 1 && x.Bimestre == 3).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 3).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 2 && x.Bimestre == 3).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 3).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 4 && x.Bimestre == 3).Nota);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 3).ConceitoId);
            Assert.Null(resultado.Single(x => x.ComponenteCurricularCodigo == 5 && x.Bimestre == 3).Nota);

            Assert.Empty(resultado.Where(x => x.ComponenteCurricularCodigo == 3));
            Assert.Empty(resultado.Where(x => x.ComponenteCurricularCodigo == 7));
        }
    }
}
