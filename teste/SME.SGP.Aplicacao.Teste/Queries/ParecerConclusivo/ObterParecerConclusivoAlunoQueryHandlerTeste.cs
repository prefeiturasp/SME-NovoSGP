using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ParecerConclusivo
{
    public class ObterParecerConclusivoAlunoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> repositorioFrequenciaAlunoDisciplinaPeriodoConsulta;
        private readonly Mock<IMediator> mediator;
        private readonly ObterParecerConclusivoAlunoQueryHandler queryHandler;

        public ObterParecerConclusivoAlunoQueryHandlerTeste()
        {
            repositorioFrequenciaAlunoDisciplinaPeriodoConsulta = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            mediator = new Mock<IMediator>();
            queryHandler = new ObterParecerConclusivoAlunoQueryHandler(mediator.Object, repositorioFrequenciaAlunoDisciplinaPeriodoConsulta.Object);
        }

        [Fact(DisplayName = "ObterParecerConclusivoAlunoQueryHandler - Deve definir códigos de turmas quando não retornado itens ao obter por aluno, ano letivo e tipo turma")]
        public async Task DefinirCodigosTurmasQuandoListaPorAlunoAnoLetivoTipoSemItens()
        {
            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    CodigoTurma = "1",
                    TipoTurma = TipoTurma.Regular,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    Ue = new()
                });

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>() { new() });

            var tiposConsulta = new List<int>() { (int)TipoTurma.Regular, (int)TipoTurma.EdFisica, (int)TipoTurma.ItinerarioEnsMedio };

            mediator.Setup(x => x.Send(It.Is<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(y => y.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year &&
                y.CodigoAluno == "1" && y.TiposTurmas.Equals(tiposConsulta)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Enumerable.Empty<string>().ToArray());

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y =>
                (y.Tipo == TipoParametroSistema.PercentualFrequenciaCritico || y.Tipo == TipoParametroSistema.PercentualFrequenciaCriticoBaseNacional) &&
                 y.Ano == DateTimeExtension.HorarioBrasilia().Year), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("0");

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(y => y.TurmasCodigo.All(t => t == "1")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() });

            var request = new ObterParecerConclusivoAlunoQuery("1", "1", new List<ConselhoClasseParecerConclusivo>() { new() });
            var resultado = await queryHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.Null(resultado);

            mediator.Verify(x => x.Send(It.IsAny<ObterTurmasComMatriculasValidasQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "ObterParecerConclusivoAlunoQueryHandler - Não deve obter parecer conclusivo 'Retido por frequencia' caso aluno não tenha frequencia")]
        public async Task ValidaParecerConclusivoAlunoSemFrequencia()
        {

            var pareceresDaTurma = new List<ConselhoClasseParecerConclusivo>() { 
                new ConselhoClasseParecerConclusivo() { Id = 5, Nome = "Retido por frequencia", Aprovado = false, Nota = false},
                new ConselhoClasseParecerConclusivo() { Id = 7, Nome = "Promovido", Aprovado = true, Nota = true},
                new ConselhoClasseParecerConclusivo() { Id = 2, Nome = "Promovido por Conselho", Aprovado = true, Nota = false,Conselho = true},
                new ConselhoClasseParecerConclusivo() { Id = 4, Nome = "Retido", Aprovado = false, Nota = true,Conselho = true}
            };

            var notasFinaisConselhoFechamentoPorAlunoTurmas = new List<NotaConceitoFechamentoConselhoFinalDto>() { 
                new NotaConceitoFechamentoConselhoFinalDto(){ComponenteCurricularCodigo = 2, Nota = 5, ConselhoClasseAlunoId = 1, FechamentoNotaId = null },
                new NotaConceitoFechamentoConselhoFinalDto(){ComponenteCurricularCodigo = 6, Nota = 5, ConselhoClasseAlunoId = 1, FechamentoNotaId = null },
                new NotaConceitoFechamentoConselhoFinalDto(){ComponenteCurricularCodigo = 7, Nota = 5, ConselhoClasseAlunoId = 1, FechamentoNotaId = null },
                new NotaConceitoFechamentoConselhoFinalDto(){ComponenteCurricularCodigo = 8, Nota = 5, ConselhoClasseAlunoId = 1, FechamentoNotaId = null },
            };

            mediator.Setup(x => x.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(y => y.TurmaCodigo == "1"), It.IsAny<CancellationToken>()))
              .ReturnsAsync(new Turma()
              {
                  CodigoTurma = "1",
                  TipoTurma = TipoTurma.Regular,
                  AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                  Ue = new()
              });

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>() { new() });

            var tiposConsulta = new List<int>() { (int)TipoTurma.Regular, (int)TipoTurma.EdFisica, (int)TipoTurma.ItinerarioEnsMedio };

            mediator.Setup(x => x.Send(It.Is<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(y => y.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year &&
                y.CodigoAluno == "1" && y.TiposTurmas.Equals(tiposConsulta)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(Enumerable.Empty<string>().ToArray());

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y =>
                (y.Tipo == TipoParametroSistema.PercentualFrequenciaCritico || y.Tipo == TipoParametroSistema.PercentualFrequenciaCriticoBaseNacional) &&
                 y.Ano == DateTimeExtension.HorarioBrasilia().Year), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("0");
            
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario());

            mediator.Setup(x => x.Send(It.Is<ObterComponentesCurricularesPorTurmasCodigoQuery>(y => y.TurmasCodigo.All(t => t == "1")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DisciplinaDto>() { new() });

            mediator.Setup(x => x.Send(It.IsAny<ObterNotasFinaisConselhoFechamentoPorAlunoTurmasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notasFinaisConselhoFechamentoPorAlunoTurmas);

            mediator.Setup(x => x.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(y =>
                y.Tipo == TipoParametroSistema.MediaBimestre && y.Ano == DateTimeExtension.HorarioBrasilia().Year), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("5");


            var request = new ObterParecerConclusivoAlunoQuery("1", "1", pareceresDaTurma);
            var resultado = await queryHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.True(resultado.Id != pareceresDaTurma.First().Id);
        } 
    }
}
