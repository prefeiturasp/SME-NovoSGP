using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ValidarMediaAlunosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioAtividadeAvaliativa> _repositorioMock;
        private readonly ValidarMediaAlunosUseCase _useCase;

        public ValidarMediaAlunosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioMock = new Mock<IRepositorioAtividadeAvaliativa>();
            _useCase = new ValidarMediaAlunosUseCase(_mediatorMock.Object, _repositorioMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Mensagens_Para_Cada_Atividade()
        {
            var atividadesIds = new List<long> { 1, 2 };
            var alunosIds = new List<string> { "A1", "A2" };
            var usuario = new Usuario { Nome = "Usuário Teste", CodigoRf = "RF123", CriadoEm = DateTime.Now };

            var filtro = new FiltroValidarMediaAlunosDto(
                atividadesAvaliativasIds: atividadesIds,
                alunosIds: alunosIds,
                usuario: usuario,
                disciplinaId: "DISC1",
                codigoTurma: "TURMA1",
                hostAplicacao: "localhost",
                temAbrangenciaUeOuDreOuSme: true,
                consideraHistorico: false
            );

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var notasConceitos = new List<NotaConceito>
            {
                new NotaConceito { AtividadeAvaliativaID = 1, AlunoId = "A1", Nota = 7 },
                new NotaConceito { AtividadeAvaliativaID = 1, AlunoId = "A2", Nota = 5 },
                new NotaConceito { AtividadeAvaliativaID = 2, AlunoId = "A1", Nota = 6 },
                new NotaConceito { AtividadeAvaliativaID = 2, AlunoId = "A2", Nota = 4 },
            };

            var atividadesAvaliativas = new List<AtividadeAvaliativa>
            {
                new AtividadeAvaliativa { Id = 1, DescricaoAvaliacao = "Atividade 1" },
                new AtividadeAvaliativa { Id = 2, DescricaoAvaliacao = "Atividade 2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterNotasPorAlunosAtividadesAvaliativasQuery>(q =>
                    q.AtividadesAvaliativasId.SequenceEqual(atividadesIds.ToArray()) &&
                    q.AlunosId.SequenceEqual(alunosIds.ToArray()) &&
                    q.ComponenteCurricularId == filtro.DisciplinaId &&
                    q.CodigoTurma == filtro.CodigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notasConceitos);

            _repositorioMock
                .Setup(r => r.ListarPorIds(atividadesIds))
                .ReturnsAsync(atividadesAvaliativas);

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes), It.IsAny<CancellationToken>()))
                .ReturnsAsync("20");

            var publishedCommands = new List<PublicarFilaSgpCommand>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                {
                    var publicarCmd = cmd as PublicarFilaSgpCommand;
                    if (publicarCmd != null)
                        publishedCommands.Add(publicarCmd);
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            Assert.Equal(atividadesIds.Count, publishedCommands.Count);

            foreach (var cmd in publishedCommands)
            {
                Assert.Equal(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunosAtividadeAvaliativa, cmd.Rota);
                Assert.NotNull(cmd.Filtros);

                var filtroAtividade = cmd.Filtros as FiltroValidarMediaAlunosAtividadeAvaliativaDto;
                Assert.NotNull(filtroAtividade);
                Assert.Equal(20, filtroAtividade.PercentualAlunosInsuficientes);
                Assert.Contains(filtroAtividade.ChaveNotasPorAvaliacao, atividadesIds);
                Assert.NotNull(filtroAtividade.NotasPorAvaliacao);
                Assert.NotNull(filtroAtividade.AtividadesAvaliativas);

                Assert.Equal(usuario.CodigoRf, filtroAtividade.Usuario.CodigoRf);
                Assert.Equal(usuario.Nome, filtroAtividade.Usuario.Nome);
                Assert.Equal(usuario.CriadoEm, filtroAtividade.Usuario.CriadoEm);

                Assert.Equal(filtro.DisciplinaId, filtroAtividade.DisciplinaId);
                Assert.Equal(filtro.HostAplicacao, filtroAtividade.HostAplicacao);
                Assert.Equal(filtro.TemAbrangenciaUeOuDreOuSme, filtroAtividade.TemAbrangenciaUeOuDreOuSme);
                Assert.False(filtroAtividade.ConsideraHistorico);
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositorioMock.Verify(r => r.ListarPorIds(atividadesIds), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(atividadesIds.Count));
        }

        [Fact]
        public async Task Constructor_Null_Repositorio_Throws_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => Task.FromResult(new ValidarMediaAlunosUseCase(_mediatorMock.Object, null)));
        }

        [Fact]
        public async Task Executar_Com_Lista_Vazia_De_Atividades_Nao_Deve_Publicar_Mensagens()
        {
            var atividadesIds = new List<long>(); 
            var alunosIds = new List<string> { "A1", "A2" };
            var usuario = new Usuario { Nome = "Usuário Teste", CodigoRf = "RF123", CriadoEm = DateTime.Now };

            var filtro = new FiltroValidarMediaAlunosDto(
                atividadesAvaliativasIds: atividadesIds,
                alunosIds: alunosIds,
                usuario: usuario,
                disciplinaId: "DISC1",
                codigoTurma: "TURMA1",
                hostAplicacao: "localhost",
                temAbrangenciaUeOuDreOuSme: true,
                consideraHistorico: false
            );

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaConceito>()); 

            _repositorioMock
                .Setup(r => r.ListarPorIds(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new List<AtividadeAvaliativa>());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("20");

            var publishedCommands = new List<PublicarFilaSgpCommand>();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                {
                    var publicarCmd = cmd as PublicarFilaSgpCommand;
                    if (publicarCmd != null)
                        publishedCommands.Add(publicarCmd);
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            Assert.Empty(publishedCommands);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterNotasPorAlunosAtividadesAvaliativasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositorioMock.Verify(r => r.ListarPorIds(It.IsAny<IEnumerable<long>>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
