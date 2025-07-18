using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.AtividadeAvaliativa
{
    public class SalvarNotaAtividadeAvaliativaGsaCommandHandlerTeste
    {
        private readonly Mock<IRepositorioNotasConceitos> repositorioMock;
        private readonly SalvarNotaAtividadeAvaliativaGsaCommandHandler handler;

        public SalvarNotaAtividadeAvaliativaGsaCommandHandlerTeste()
        {
            repositorioMock = new Mock<IRepositorioNotasConceitos>();
            handler = new SalvarNotaAtividadeAvaliativaGsaCommandHandler(repositorioMock.Object);
        }

        private NotaTipoValor CriarTipoNota(TipoNota tipoNota)
        {
            return new NotaTipoValor
            {
                TipoNota = tipoNota,
                Ativo = true,
                Descricao = tipoNota.ToString(),
                InicioVigencia = DateTime.Now.AddYears(-1),
                FimVigencia = DateTime.Now.AddYears(1)
            };
        }

        [Fact]
        public async Task Handle_Deve_Salvar_Nova_Nota_Quando_NotaConceito_Eh_Nula()
        {
            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: null,
                nota: 8.5,
                statusGsa: StatusGSA.Entregue,
                atividadeId: 123,
                tipoNota: CriarTipoNota(TipoNota.Nota),
                codigoAluno: "9999",
                componenteCurricular: "MAT"
            );


            var handlerFake = new SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake(repositorioMock.Object);

            await handlerFake.Executar(comando);

            repositorioMock.Verify(x => x.SalvarAsync(It.Is<NotaConceito>(nc =>
                nc.Nota == 8.5 &&
                nc.AlunoId == "9999" &&
                nc.AtividadeAvaliativaID == 123 &&
                nc.DisciplinaId == "MAT" &&
                nc.TipoNota == TipoNota.Nota &&
                nc.StatusGsa == StatusGSA.Entregue
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Salvar_Nova_Nota_Com_Conceito_Quando_Nao_Eh_Nota()
        {
            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: null,
                nota: 4.0, 
                statusGsa: StatusGSA.Entregue,
                atividadeId: 456,
                tipoNota: CriarTipoNota(TipoNota.Conceito),
                codigoAluno: "777",
                componenteCurricular: "CIE"
            );

            var handlerFake = new SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake(repositorioMock.Object);

            await handlerFake.Executar(comando);

            repositorioMock.Verify(x => x.SalvarAsync(It.Is<NotaConceito>(nc =>
                nc.ConceitoId == (long)ConceitoValores.NS &&
                nc.TipoNota == TipoNota.Conceito &&
                nc.AlunoId == "777"
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Alterar_NotaExistente_Com_Nota()
        {
            var notaConceito = new NotaConceito
            {
                AlunoId = "555",
                AtividadeAvaliativaID = 1,
                TipoNota = TipoNota.Nota,
                DisciplinaId = "ART",
                StatusGsa = StatusGSA.NaoEspecificado
            };

            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: notaConceito,
                nota: 9.3,
                statusGsa: StatusGSA.Entregue,
                atividadeId: 1,
                tipoNota: CriarTipoNota(TipoNota.Nota),
                codigoAluno: "555",
                componenteCurricular: "ART"
            );

            var handlerFake = new SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake(repositorioMock.Object);

            await handlerFake.Executar(comando);

            repositorioMock.Verify(x => x.SalvarAsync(It.Is<NotaConceito>(nc =>
                nc.Nota == 9.3 &&
                nc.StatusGsa == StatusGSA.Entregue
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Deve_Alterar_NotaExistente_Com_Conceito()
        {
            var notaConceito = new NotaConceito
            {
                AlunoId = "888",
                AtividadeAvaliativaID = 88,
                TipoNota = TipoNota.Conceito,
                StatusGsa = StatusGSA.NaoEspecificado
            };

            var comando = new SalvarNotaAtividadeAvaliativaGsaCommand(
                notaConceito: notaConceito,
                nota: 6.0, 
                statusGsa: StatusGSA.Entregue,
                atividadeId: 88,
                tipoNota: CriarTipoNota(TipoNota.Conceito),
                codigoAluno: "888",
                componenteCurricular: "HIS"
            );

            var handlerFake = new SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake(repositorioMock.Object);

            await handlerFake.Executar(comando);

            repositorioMock.Verify(x => x.SalvarAsync(It.Is<NotaConceito>(nc =>
                nc.ConceitoId == (long)ConceitoValores.S &&
                nc.StatusGsa == StatusGSA.Entregue
            )), Times.Once);
        }

        public class SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake : SalvarNotaAtividadeAvaliativaGsaCommandHandler
        {
            public SalvarNotaAtividadeAvaliativaGsaCommandHandlerFake(IRepositorioNotasConceitos repositorioConceitos)
          : base(repositorioConceitos)
            {
            }

            public async Task Executar(SalvarNotaAtividadeAvaliativaGsaCommand command)
            {
                await base.Handle(command, CancellationToken.None);
            }
        }
    }
}
