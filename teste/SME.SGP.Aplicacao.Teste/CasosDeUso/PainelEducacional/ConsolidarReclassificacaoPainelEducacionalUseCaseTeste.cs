using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.Reclassificacao.ExcluirReclassificacaoAnual;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarReclassificacaoAnual;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarReclassificacaoPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioTurmaConsulta> repositorioTurmaConsultaMock;
        private readonly ConsolidarReclassificacaoPainelEducacionalUseCase useCase;

        public ConsolidarReclassificacaoPainelEducacionalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioTurmaConsultaMock = new Mock<IRepositorioTurmaConsulta>();
            useCase = new ConsolidarReclassificacaoPainelEducacionalUseCase(mediatorMock.Object, repositorioTurmaConsultaMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Dados_Deve_Processar_E_Salvar_Com_Sucesso()
        {
            var mensagem = new MensagemRabbit();

            var turmasDto = new List<Turma>
            {
                new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ue = new Ue
                    {
                        Nome = "UE 1",
                        Dre = new Dominio.Dre { Nome = "DRE 1" }
                    }
                }
            };

            var dres = new List<Dominio.Dre> { new Dominio.Dre { CodigoDre = "DRE-1", Nome = "DRE 1" } };

            var alunos = new List<AlunosSituacaoTurmas>
            {
                new AlunosSituacaoTurmas
                {
                    CodigoTurma = "T1",
                    QuantidadeAlunos = 5,
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.ReclassificadoSaida
                },
                new AlunosSituacaoTurmas
                {
                    CodigoTurma = "T2-Fora",
                    QuantidadeAlunos = 10,
                    CodigoSituacaoMatricula = (int)SituacaoMatriculaAluno.ReclassificadoSaida
                }
            };

            repositorioTurmaConsultaMock.Setup(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>())).ReturnsAsync(turmasDto);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dres);
            mediatorMock.Setup(m => m.Send(It.Is<ObterAlunosSituacaoTurmasQuery>(q => q.CodigoDre == "DRE-1"), It.IsAny<CancellationToken>())).ReturnsAsync(alunos);
            mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            repositorioTurmaConsultaMock.Verify(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosSituacaoTurmasQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.Is<PainelEducacionalSalvarReclassificacaoAnualCommand>(cmd => cmd.ReclassificacaoAnual.First().QuantidadeAlunosReclassificados == 5), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Turmas_Deve_Retornar_Sem_Salvar()
        {
            var mensagem = new MensagemRabbit();

            repositorioTurmaConsultaMock.Setup(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>())).ReturnsAsync(new List<Turma>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            repositorioTurmaConsultaMock.Verify(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Dres_Deve_Retornar_Sem_Salvar()
        {
            var mensagem = new MensagemRabbit();

            var turmasDto = new List<Turma>
            {
                new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ue = new Ue
                    {
                        Nome = "UE 1",
                        Dre = new Dominio.Dre { Nome = "DRE 1" }
                    }
                }
            };

            repositorioTurmaConsultaMock.Setup(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>())).ReturnsAsync(turmasDto);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Dominio.Dre>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            repositorioTurmaConsultaMock.Verify(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosSituacaoTurmasQuery>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Alunos_Reclassificados_Deve_Retornar_Sem_Salvar()
        {
            var mensagem = new MensagemRabbit();

            var turmasDto = new List<Turma>
            {
                new Turma
                {
                    CodigoTurma = "T1",
                    ModalidadeCodigo = Modalidade.Fundamental,
                    Ue = new Ue
                    {
                        Nome = "UE 1",
                        Dre = new Dominio.Dre { Nome = "DRE 1" }
                    }
                }
            };

            var dres = new List<Dominio.Dre> { new Dominio.Dre { CodigoDre = "DRE-1", Nome = "DRE 1" } };

            repositorioTurmaConsultaMock.Setup(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>())).ReturnsAsync(turmasDto);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(dres);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosSituacaoTurmasQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunosSituacaoTurmas>());

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            repositorioTurmaConsultaMock.Verify(r => r.ObterTurmasPorAnoLetivo(It.IsAny<int>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasDresQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAlunosSituacaoTurmasQuery>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarReclassificacaoAnualCommand>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
