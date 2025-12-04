using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.EncaminhamentoNAAPA.ReabrirEncaminhamentoNAAPA
{
    public class ReabrirEncaminhamentoNAAPACommandHandlerTeste
    {
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAtendimentoNAAPA> repositorioEncaminhamentoNAAPA;
        private readonly ReabrirAtendimentoNAAPACommandHandler command;

        public ReabrirEncaminhamentoNAAPACommandHandlerTeste()
        {
            unitOfWork = new Mock<IUnitOfWork>();
            mediator = new Mock<IMediator>();
            repositorioEncaminhamentoNAAPA = new Mock<IRepositorioAtendimentoNAAPA>();
            command = new ReabrirAtendimentoNAAPACommandHandler(unitOfWork.Object, mediator.Object, repositorioEncaminhamentoNAAPA.Object);
        }

        [Fact(DisplayName = "ReabrirEncaminhamentoNAAPACommand - Deve validar regras considerando a última matrícula válida mesmo com data de situações iguais")]
        public async Task DeveValidarRegrasConsiderandoUltimaMatriculaValidaMesmoComDataSituacoesIguais()
        {
            mediator.Setup(x => x.Send(It.Is<ObterCabecalhoAtendimentoNAAPAQuery>(y => y.EncaminhamentoNAAPAId == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.EncaminhamentoNAAPA() { Id = 1, Situacao = SituacaoNAAPA.Encerrado, AlunoCodigo = "1" });

            var dataHoraAtual = DateTimeExtension.HorarioBrasilia();
            var matriculasAlunosEol = new List<TurmasDoAlunoDto>()
            {
                new() { CodigoTipoTurma = (int)TipoTurma.Regular, AnoLetivo = dataHoraAtual.Year, DataSituacao = dataHoraAtual, DataAtualizacaoTabela = dataHoraAtual, CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Transferido },
                new() { CodigoTipoTurma = (int)TipoTurma.Regular, AnoLetivo = dataHoraAtual.Year, DataSituacao = dataHoraAtual, DataAtualizacaoTabela = dataHoraAtual.AddMilliseconds(5), CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.Ativo }
            };

            mediator.Setup(x => x.Send(It.Is<ObterAlunosEolPorCodigosQuery>(y => y.CodigosAluno.Single() == 1 && y.TodasMatriculas), It.IsAny<CancellationToken>()))
                .ReturnsAsync(matriculasAlunosEol);

            repositorioEncaminhamentoNAAPA.Setup(x => x.EncaminhamentoContemAtendimentosItinerancia(1))
                .ReturnsAsync(true);

            var retorno = await command.Handle(new ReabrirAtendimentoNAAPACommand(1), It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.Equal((int)SituacaoNAAPA.EmAtendimento, retorno.Codigo);
            Assert.Equal("Em atendimento", retorno.Descricao);
        }
    }
}
