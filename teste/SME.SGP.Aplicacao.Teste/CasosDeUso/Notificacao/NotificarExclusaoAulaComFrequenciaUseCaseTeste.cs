using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificarExclusaoAulaComFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotificarExclusaoAulaComFrequenciaUseCase _useCase;

        public NotificarExclusaoAulaComFrequenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new NotificarExclusaoAulaComFrequenciaUseCase(_mediatorMock.Object);
        }

        [Fact(DisplayName = "Executar deve retornar false quando não houver titulares")]
        public async Task Deve_RetornarFalse_QuandoNaoHouverTitulares()
        {
            var turma = CriarTurma();
            var dto = new NotificarExclusaoAulasComFrequenciaDto(turma, new[] { DateTime.Today });
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<ProfessorTitularDisciplinaEol>)null);

            var resultado = await _useCase.Executar(mensagem);

            Assert.False(resultado);
        }

        [Fact(DisplayName = "Executar deve notificar titulares com lista simples")]
        public async Task Deve_NotificarTitulares_Simples()
        {
            var turma = CriarTurma();
            var datas = new[] { new DateTime(2025, 10, 10), new DateTime(2025, 10, 12) };
            var dto = new NotificarExclusaoAulasComFrequenciaDto(turma, datas);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            var professores = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "123456" },
                new ProfessorTitularDisciplinaEol { ProfessorRf = "654321" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professores);

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "123456" || q.CodigoRf == "654321"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "123456" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Executar deve notificar titulares quando houver um único RF separado por vírgula")]
        public async Task Deve_NotificarTitulares_ComRfSeparadoPorVirgula()
        {
            var turma = CriarTurma();
            var dto = new NotificarExclusaoAulasComFrequenciaDto(turma, new[] { DateTime.Today });
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            var professores = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "111111,222222" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professores);

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "111111" || q.CodigoRf == "222222"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Usuario { CodigoRf = "111111" });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Executar não deve notificar quando usuário for nulo")]
        public async Task NaoDeve_Notificar_QuandoUsuarioNulo()
        {
            var turma = CriarTurma();
            var dto = new NotificarExclusaoAulasComFrequenciaDto(turma, new[] { DateTime.Today });
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            var professores = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { ProfessorRf = "333333" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professores);

            _mediatorMock
                .Setup(m => m.Send(It.Is<ObterUsuarioPorRfQuery>(q => q.CodigoRf == "333333"), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private static Turma CriarTurma()
        {
            return new Turma
            {
                CodigoTurma = "TURMA123",
                Nome = "Turma Teste",
                ModalidadeCodigo = Modalidade.Medio,
                Ue = new Ue
                {
                    Nome = "Escola Municipal Teste",
                    TipoEscola = TipoEscola.EMEF,
                    Dre = new SME.SGP.Dominio.Dre { Abreviacao = "DRE-TST" }
                }
            };
        }
    }
}
