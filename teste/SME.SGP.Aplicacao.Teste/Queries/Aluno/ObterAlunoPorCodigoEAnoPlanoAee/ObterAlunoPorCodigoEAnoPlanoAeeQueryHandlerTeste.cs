using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterAlunoPorCodigoEAnoPlanoAeeQueryHandler query;
        public ObterAlunoPorCodigoEAnoPlanoAeeQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            query = new ObterAlunoPorCodigoEAnoPlanoAeeQueryHandler(mediator.Object);
        }


        [Fact(DisplayName = "ObterAlunoPorCodigoEAnoPlanoAeeQuery -  Obter Aluno Por Codigo e Ano Plano AEE")]
        public async Task Deve_Obter_Aulo_reduzido()
        {
            var ano = DateTimeExtension.HorarioBrasilia().Year;
            var listaTurmasAlunoPorFiltroPlanoAee = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                    { 
                        NomeAluno = "Aluno teste 1",
                        Ano = ano, 
                        CodigoAluno = "1",
                        CodigoComponenteCurricular = 138,
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        CodigoTurma = 1,
                        DataNascimento = new DateTime(1988,2,10),
                        DataMatricula = DateTimeExtension.HorarioBrasilia(),
                        DataSituacao = DateTimeExtension.HorarioBrasilia(),
                        NumeroAlunoChamada = 1,
                        PossuiDeficiencia = false,
                        SituacaoMatricula = "Ativo",
                        Transferencia_Interna = false,
                        DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                        CodigoEscola = "1",
                        CodigoTipoTurma = 1
                    }
            };
            var turma = new Turma
            {
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                Historica = false,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo =  DateTimeExtension.HorarioBrasilia().Year,
                Semestre = 1,
                Nome = "Turma de teste 1",
                TipoTurma = TipoTurma.Regular
            };

            
            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasAlunoPorFiltroPlanoAeeQuery>(),It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaTurmasAlunoPorFiltroPlanoAee);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);


            var retornoConsulta = await query.Handle(new ObterAlunoPorCodigoEAnoPlanoAeeQuery("1",ano),new CancellationToken());

            Assert.NotNull(retornoConsulta);
            Assert.NotNull(retornoConsulta.Nome);
            Assert.NotNull(retornoConsulta.TurmaEscola);
            Assert.NotNull(retornoConsulta.CodigoAluno);
            Assert.NotNull(retornoConsulta.Situacao);
        }
    }
}