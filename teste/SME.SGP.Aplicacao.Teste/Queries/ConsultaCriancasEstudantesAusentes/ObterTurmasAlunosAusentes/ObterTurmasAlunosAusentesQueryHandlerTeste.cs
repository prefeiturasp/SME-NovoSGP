using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ConsultaCriancasEstudantesAusentes.ObterTurmasAlunosAusentes
{
    public class ObterTurmasAlunosAusentesQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasAlunosAusentesQueryHandler query;
        private readonly Mock<IRepositorioConsultaCriancasEstudantesAusentes> repositorio;

        public ObterTurmasAlunosAusentesQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorio = new Mock<IRepositorioConsultaCriancasEstudantesAusentes>();
            query = new ObterTurmasAlunosAusentesQueryHandler(repositorio.Object,mediator.Object);
        }

        [Fact(DisplayName = "ObterTurmasAlunosAusentesQueryHandler - Deve Obter Alunos na turma ausentes")]
        public async Task Deve_Obter_Turmas_Alunos_Ausentes()
        {
            var alunosAusentes = new List<AlunosAusentesDto>
            {
                new AlunosAusentesDto { CodigoEol = "1" },
                new AlunosAusentesDto { CodigoEol = "2" }
            };

            var alunosTurma = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "1", NomeAluno = "Aluno1", NumeroAlunoChamada = 1 },
            };

            repositorio.Setup(r => r.ObterTurmasAlunosAusentes(It.IsAny<FiltroObterAlunosAusentesDto>()))
                .ReturnsAsync(alunosAusentes);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunosEolPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosTurma);

            var filtroObterAlunosAusentesDto = new FiltroObterAlunosAusentesDto() 
            {
                CodigoUe = "1",
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
            };

            var retorno = await query.Handle(new ObterTurmasAlunosAusentesQuery(filtroObterAlunosAusentesDto), new CancellationToken());
            
            Assert.Equal(alunosTurma.First().CodigoAluno, retorno.First().CodigoEol);
        }
    }
}
