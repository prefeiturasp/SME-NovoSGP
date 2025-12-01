using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ConsolidarFrequenciaPorTurmaAnualUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarFrequenciaPorTurmaAnualUseCaseFake useCaseFake;

        public ConsolidarFrequenciaPorTurmaAnualUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCaseFake = new ConsolidarFrequenciaPorTurmaAnualUseCaseFake(mediatorMock.Object);
            useCaseFake.DefinirAnoAnterior(true); 
        }

        [Fact]
        public async Task Obter_Frequencia_Consideradas_Deve_Retornar_Apenas_Alunos_Ativos()
        {
            var codigoTurma = "TURMA789";
            var dataAtual = new DateTime(2025, 7, 18);

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "1",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataMatricula = dataAtual.AddDays(-30),
                    DataSituacao = dataAtual.AddDays(-10),
                    DataNascimento = new DateTime(2010, 1, 1)
                },
                new AlunoPorTurmaResposta
                {
                    CodigoAluno = "2",
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                    DataMatricula = dataAtual.AddDays(-30),
                    DataSituacao = dataAtual.AddDays(-5),
                    DataNascimento = new DateTime(2010, 1, 1)
                }
            };

            var frequencias = new List<FrequenciaAlunoDto>
            {
                new FrequenciaAlunoDto { AlunoCodigo = "1", PeriodoFim = dataAtual },
                new FrequenciaAlunoDto { AlunoCodigo = "2", PeriodoFim = dataAtual }
            };

            mediatorMock
                .Setup(x => x.Send(It.Is<ObterAlunosDentroPeriodoQuery>(q => q.CodigoTurma == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock
                .Setup(x => x.Send(It.Is<ObterFrequenciaGeralPorTurmaQuery>(q => q.TurmaCodigo == codigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(frequencias);

            useCaseFake.SetFiltro(new FiltroConsolidacaoFrequenciaTurma
            {
                TurmaCodigo = codigoTurma,
                Data = dataAtual,
                PercentualFrequenciaMinimo = 50,
                TurmaId = 1
            });

            var resultado = await useCaseFake.ObterFrequenciaConsideradasPublic(codigoTurma);

            var codigos = resultado.Select(r => r.AlunoCodigo).ToList();

            resultado.Should().HaveCount(1);
            resultado.Should().ContainSingle(f => f.AlunoCodigo == "1");
        }
    }

    public class ConsolidarFrequenciaPorTurmaAnualUseCaseFake : ConsolidarFrequenciaPorTurmaAnualUseCase
    {
        public ConsolidarFrequenciaPorTurmaAnualUseCaseFake(IMediator mediator) : base(mediator) { }

        public void SetFiltro(FiltroConsolidacaoFrequenciaTurma filtro)
        {
            base.Filtro = filtro;
        }

        public void DefinirAnoAnterior(bool valor)
        {
            base.AnoAnterior = valor;
        }

        public Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradasPublic(string codigoTurma)
        {
            return base.ObterFrequenciaConsideradas(codigoTurma);
        }
    }
}
