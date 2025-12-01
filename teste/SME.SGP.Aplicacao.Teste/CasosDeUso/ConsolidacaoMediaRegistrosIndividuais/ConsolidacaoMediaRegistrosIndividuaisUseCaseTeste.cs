using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoMediaRegistrosIndividuais
{
    public class ConsolidacaoMediaRegistrosIndividuaisUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidacaoMediaRegistrosIndividuaisUseCase _useCase;
        private readonly FiltroMediaRegistroIndividualTurmaDTO _filtro;
        private readonly MensagemRabbit _mensagemRabbit;

        public ConsolidacaoMediaRegistrosIndividuaisUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidacaoMediaRegistrosIndividuaisUseCase(_mediatorMock.Object);
            _filtro = new FiltroMediaRegistroIndividualTurmaDTO(1, 2025);

            var filtroJson = JsonConvert.SerializeObject(_filtro);

            _mensagemRabbit = new MensagemRabbit(string.Empty, filtroJson, Guid.NewGuid(), "test_user");
        }

        [Fact]
        public async Task Executar_Quando_Nao_Encontrar_Alunos_Deve_Retornar_True_Sem_Processar()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunoInfantilComRegistroIndividualDTO>());

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RegistraConsolidacaoMediaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Alunos_Nao_Possuem_Registros_Individuais_Deve_Retornar_True_Sem_Registrar_Consolidacao()
        {
            var alunos = new List<AlunoInfantilComRegistroIndividualDTO>
            {
                new AlunoInfantilComRegistroIndividualDTO { AlunoCodigo = 10, TurmaId = _filtro.TurmaId }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<RegistroIndividualAlunoDTO>());

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RegistraConsolidacaoMediaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Aluno_Possui_Apenas_Um_Registro_Deve_Calcular_Media_Zero_E_Nao_Registrar()
        {
            var aluno = new AlunoInfantilComRegistroIndividualDTO { AlunoCodigo = 10, TurmaId = _filtro.TurmaId };
            var alunos = new List<AlunoInfantilComRegistroIndividualDTO> { aluno };

            var registros = new List<RegistroIndividualAlunoDTO>
            {
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 1) }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(q => q.AlunoCodigo == aluno.AlunoCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registros);

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RegistraConsolidacaoMediaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Aluno_Possui_Dois_Registros_Deve_Calcular_Media_Corretamente_E_Registrar()
        {
            var aluno = new AlunoInfantilComRegistroIndividualDTO { AlunoCodigo = 10, TurmaId = _filtro.TurmaId };
            var alunos = new List<AlunoInfantilComRegistroIndividualDTO> { aluno };

            var dataInicio = new DateTime(2025, 9, 1);
            var dataFim = new DateTime(2025, 9, 11); 
            var registros = new List<RegistroIndividualAlunoDTO>
            {
                new RegistroIndividualAlunoDTO { DataRegistro = dataInicio },
                new RegistroIndividualAlunoDTO { DataRegistro = dataFim }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(q => q.AlunoCodigo == aluno.AlunoCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registros);

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistraConsolidacaoMediaRegistroIndividualCommand>(c => c.Quantidade == 10), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Alunos_Possuem_Mais_De_Dois_Registros_Deve_Calcular_Media_E_Registrar_Com_Sucesso()
        {
            var aluno1 = new AlunoInfantilComRegistroIndividualDTO { AlunoCodigo = 10, TurmaId = _filtro.TurmaId };
            var aluno2 = new AlunoInfantilComRegistroIndividualDTO { AlunoCodigo = 20, TurmaId = _filtro.TurmaId };
            var alunos = new List<AlunoInfantilComRegistroIndividualDTO> { aluno1, aluno2 };

            var registrosAluno1 = new List<RegistroIndividualAlunoDTO>
            {
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 1) },
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 11) },
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 21) }
            };

            var registrosAluno2 = new List<RegistroIndividualAlunoDTO>
            {
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 2) },
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 12) },
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 9, 22) },
                new RegistroIndividualAlunoDTO { DataRegistro = new DateTime(2025, 10, 2) }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(q => q.AlunoCodigo == aluno1.AlunoCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosAluno1);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterRegistrosIndividuaisPorTurmaAlunoQuery>(q => q.AlunoCodigo == aluno2.AlunoCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosAluno2);

            int mediaGeralEsperada = 10;

            var resultado = await _useCase.Executar(_mensagemRabbit);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<RegistraConsolidacaoMediaRegistroIndividualCommand>(c => c.Quantidade == mediaGeralEsperada && c.TurmaId == _filtro.TurmaId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
