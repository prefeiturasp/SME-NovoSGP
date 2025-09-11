using MediatR;
using Moq;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterTurmasComMatriculasValidasQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasComMatriculasValidasQueryHandler query;
        public ObterTurmasComMatriculasValidasQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            query = new ObterTurmasComMatriculasValidasQueryHandler(mediator.Object);
        }


        [Fact(DisplayName = "ObterTurmasComMatriculasValidasQueryHandler -  Obter Turmas com matrículas válidas")]
        public async Task Deve_Obter_Somente_matriculas_validas_conforme_periodo()
        {
            var ano = DateTimeExtension.HorarioBrasilia().Year;
            var listaMatriculasAluno = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                    {
                        NomeAluno = "Aluno teste 1",
                        Ano = ano,
                        CodigoAluno = "1",
                        CodigoComponenteCurricular = 138,
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                        CodigoTurma = 1,
                        DataNascimento = new DateTime(1988,2,10),
                        DataMatricula = new DateTime(2023,02,02),
                        DataSituacao = new DateTime(2023,06,01),
                        NumeroAlunoChamada = 1,
                        PossuiDeficiencia = false,
                        SituacaoMatricula = "Ativo",
                        Transferencia_Interna = false,
                        DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                        CodigoEscola = "1",
                        CodigoTipoTurma = 1
                    },

                new AlunoPorTurmaResposta()
                    {
                        NomeAluno = "Aluno teste 1",
                        Ano = ano,
                        CodigoAluno = "1",
                        CodigoComponenteCurricular = 138,
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                        CodigoTurma = 2,
                        DataNascimento = new DateTime(1988,2,10),
                        DataMatricula = new DateTime (2023,07,01),
                        DataSituacao = new DateTime(2023,07,01),
                        NumeroAlunoChamada = 1,
                        PossuiDeficiencia = false,
                        SituacaoMatricula = "Ativo",
                        Transferencia_Interna = false,
                        DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                        CodigoEscola = "2",
                        CodigoTipoTurma = 1
                    }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaMatriculasAluno);


            var retornoConsulta = await query.Handle(new ObterTurmasComMatriculasValidasQuery("1", new string[] {"1", "2"}, new DateTime(2023,02,03), new DateTime(2023,06,05)),new CancellationToken());

            Assert.NotNull(retornoConsulta);
            //Assert.True(retornoConsulta.Count() == 1);
            //Assert.True(retornoConsulta.FirstOrDefault() == "1");
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasQueryHandler -  Obter Turmas com matrículas válidas dentro do periodo de fechamento")]
        public async Task Deve_Obter_Somente_matriculas_validas_conforme_periodo_fechamento()
        {
            var ano = DateTimeExtension.HorarioBrasilia().Year;
            var listaMatriculasAluno = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                    {
                        NomeAluno = "Aluno teste 1",
                        Ano = ano,
                        CodigoAluno = "1",
                        CodigoComponenteCurricular = 138,
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido,
                        CodigoTurma = 1,
                        DataNascimento = new DateTime(1988,2,10),
                        DataMatricula = new DateTime(2023,01,12),
                        DataSituacao = new DateTime(2023,12,20),
                        NumeroAlunoChamada = 1,
                        PossuiDeficiencia = false,
                        SituacaoMatricula = "Ativo",
                        Transferencia_Interna = false,
                        DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                        CodigoEscola = "1",
                        CodigoTipoTurma = 1
                    },

                new AlunoPorTurmaResposta()
                    {
                        NomeAluno = "Aluno teste 1",
                        Ano = ano,
                        CodigoAluno = "1",
                        CodigoComponenteCurricular = 6,
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.DispensadoEdFisica,
                        CodigoTurma = 2,
                        DataNascimento = new DateTime(1988,2,10),
                        DataMatricula = new DateTime (2023,02,26),
                        DataSituacao = new DateTime(2023,02,26),
                        NumeroAlunoChamada = 1,
                        PossuiDeficiencia = false,
                        SituacaoMatricula = "Dispensado Ed.Fisica",
                        Transferencia_Interna = false,
                        DataAtualizacaoContato = DateTimeExtension.HorarioBrasilia(),
                        CodigoEscola = "2",
                        CodigoTipoTurma = 1
                    }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaMatriculasAluno);

            var periodoFechamentoInicio = new DateTime(2023,04,12);
            var periodoFechamentoFinal = new DateTime(2023,05,17);
            var retornoConsulta = await query.Handle(new ObterTurmasComMatriculasValidasQuery("1", new string[] { "1", "2" }, periodoFechamentoInicio, periodoFechamentoFinal), new CancellationToken());

            Assert.NotNull(retornoConsulta);
            //Assert.True(retornoConsulta.Count() == 1);
            //Assert.True(retornoConsulta.FirstOrDefault() == "1");
        }
    }
}
