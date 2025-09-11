using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Nota.VerificaNotasTodosComponentesCurriculares
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly VerificaNotasTodosComponentesCurricularesQueryHandler verificaNotasTodosComponentesCurricularesQueryHandler;

        public VerificaNotasTodosComponentesCurricularesQueryHandlerTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.verificaNotasTodosComponentesCurricularesQueryHandler = new VerificaNotasTodosComponentesCurricularesQueryHandler(mediator.Object);
        }

        //[Fact(DisplayName = "VerificaNotasTodosComponentesCurricularesQueryHandler - Verifica se há nota somente para turmas ativas no período")]
        //public async Task Verifica_Se_Ha_Nota_Somente_Para_Turmas_Ativas_No_Periodo()
        //{
        //    var alunoCodigo = "123";
        //    var usuario = new Usuario() { Id = 1, CodigoRf = "123456", PerfilAtual = Perfis.PERFIL_PROFESSOR, Login = "123456" };
        //    var turma = new Turma()
        //    {
        //        CodigoTurma = "1234",
        //        ModalidadeCodigo = Modalidade.EJA,
        //        Historica = false,
        //        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
        //        Semestre = 2,
        //        AnoLetivo = 2023,
        //        Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } }
        //    };
        //    var dreUeDaTurmaDto = new DreUeDaTurmaDto() { DreCodigo = "1", UeCodigo = "321" };
        //    var periodoEscolar = new PeriodoEscolar() { Id = 1, Bimestre = 2, TipoCalendarioId = 35, PeriodoInicio = new DateTime(2023, 10, 02), PeriodoFim = new DateTime(2023, 12, 21) };
        //    var verificaNotasTodosComponentesCurricularesQuery = new VerificaNotasTodosComponentesCurricularesQuery(alunoCodigo, turma, 2, false, periodoEscolar);

        //    var turmas = new List<Turma>
        //    {
        //        new()
        //        {
        //            CodigoTurma = "1234",
        //            ModalidadeCodigo = Modalidade.EJA,
        //            Historica = false,
        //            TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
        //            Semestre = 2,
        //            AnoLetivo = 2023,
        //            Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } }
        //        },
        //        new()
        //        {
        //            CodigoTurma = "2",
        //            ModalidadeCodigo = Modalidade.EJA,
        //            Historica = false,
        //            TipoTurma = Dominio.Enumerados.TipoTurma.EdFisica,
        //            Semestre = 2,
        //            AnoLetivo = 2023,
        //            Ue = new Ue { CodigoUe = "321", Dre = new Dre { CodigoDre = "1" } }
        //        },
        //    };

        //    var listaTurmaItinerarioEnsinoMedioDto = new List<TurmaItinerarioEnsinoMedioDto>
        //    {
        //        new() { Id = 1, Nome = "1", Serie = 4 }
        //    };

        //    var listaAlunoTurmaResposta = new List<AlunoPorTurmaResposta>
        //    {
        //        new() { CodigoAluno = "123", CodigoTurma = 1234, CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado, DataSituacao = new DateTime(2023, 11, 21), DataMatricula = new DateTime(2023, 11, 17) },
        //        new() { CodigoAluno = "123", CodigoTurma = 2, CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido, DataSituacao = new DateTime(2023, 08, 17), DataMatricula = new DateTime(2023, 08, 16)  },
        //        new() { CodigoAluno = "123", CodigoTurma = 12345, CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido, DataSituacao = new DateTime(2023, 06, 28), DataMatricula = new DateTime(2023, 03, 07)  },
        //    };

        //    var codigosTurmas = new string[] { "1234", "2", "1234" };
        //    var conselhoClasseId = new long[] { 1, 2, 3 };

        //    var notaConceitoBimestreComponenteDto = new List<NotaConceitoBimestreComponenteDto>
        //    {
        //        new() {AlunoCodigo = "123", ComponenteCurricularCodigo = 1, Nota = 5, Bimestre = 2, ComponenteCurricularNome = "1"},
        //    };

        //    var disciplinaDto = new List<DisciplinaDto>
        //    {
        //        new() {Id = 1, CodigoComponenteCurricular = 1, LancaNota =true, Nome = "1" ,Professor ="123456",TurmaCodigo = "1234"},
        //    };

        //    mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(turma);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterTurmasPorCodigosQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(turmas);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterCodigosDreUePorTurmaQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(dreUeDaTurmaDto);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(listaTurmaItinerarioEnsinoMedioDto);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(codigosTurmas);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterMatriculasTurmaPorCodigoAlunoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(listaAlunoTurmaResposta);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEBimestreQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(conselhoClasseId);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterConselhoClasseNotasAlunoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(notaConceitoBimestreComponenteDto);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterNotasFinaisBimestresAlunoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(notaConceitoBimestreComponenteDto);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(listaAlunoTurmaResposta);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(usuario);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(disciplinaDto);

        //    mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(disciplinaDto);
            
        //    var resultado = await verificaNotasTodosComponentesCurricularesQueryHandler
        //        .Handle(verificaNotasTodosComponentesCurricularesQuery, It.IsAny<CancellationToken>());
            
        //    Assert.True(resultado);
        //}
    }
}
