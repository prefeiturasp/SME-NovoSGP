using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirFluenciaLeitora;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarFluenciaLeitora;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsolidarFluenciaLeitoraPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioFluenciaLeitora> _repositorioFluenciaLeitoraMock;
        private readonly ConsolidarFluenciaLeitoraPainelEducacionalUseCase _useCase;

        public ConsolidarFluenciaLeitoraPainelEducacionalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioFluenciaLeitoraMock = new Mock<IRepositorioFluenciaLeitora>();
            _useCase = new ConsolidarFluenciaLeitoraPainelEducacionalUseCase(
                _mediatorMock.Object,
                _repositorioFluenciaLeitoraMock.Object);
        }

        [Fact]
        public async Task Executar_Com_Registros_Null_Deve_Retornar_True()
        {
            var mensagem = new MensagemRabbit();
            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync((IEnumerable<PainelEducacionalRegistroFluenciaLeitoraDto>)null);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _repositorioFluenciaLeitoraMock.Verify(r => r.ObterRegistroFluenciaLeitoraGeralAsync(), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Com_Lista_Vazia_Deve_Retornar_True()
        {
            var mensagem = new MensagemRabbit();
            var registrosVazios = new List<PainelEducacionalRegistroFluenciaLeitoraDto>();

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registrosVazios);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _repositorioFluenciaLeitoraMock.Verify(r => r.ObterRegistroFluenciaLeitoraGeralAsync(), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Com_Registros_Validos_Deve_Processar_Completo()
        {
            var mensagem = new MensagemRabbit();
            var registros = CriarRegistrosFluenciaLeitora();

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _repositorioFluenciaLeitoraMock.Verify(r => r.ObterRegistroFluenciaLeitoraGeralAsync(), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Com_Periodo_Zero_Deve_Filtrar_Registros()
        {
            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 0, "108900", "DRE TESTE", 1), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1)  
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PainelEducacionalSalvarFluenciaLeitoraCommand>(cmd =>
                cmd.FluenciaLeitora.All(f => f.Periodo > 0)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Com_Multiplos_Anos_E_Periodos_Deve_Processar_Todos()
        {
            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE 1", 1),
                CriarRegistroFluenciaLeitora(2023, 2, "108900", "DRE TESTE 1", 2),
                CriarRegistroFluenciaLeitora(2024, 1, "108901", "DRE TESTE 2", 3)
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PainelEducacionalSalvarFluenciaLeitoraCommand>(cmd =>
                cmd.FluenciaLeitora.Count() == 3), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Calcular_Percentual_Corretamente()
        {

            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 2), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 2)  
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> consolidacoesSalvas = null;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    if (req is PainelEducacionalSalvarFluenciaLeitoraCommand cmd)
                        consolidacoesSalvas = cmd.FluenciaLeitora.ToList();
                })
                .ReturnsAsync(true);


            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.NotNull(consolidacoesSalvas);
            Assert.Equal(2, consolidacoesSalvas.Count);

            var fluencia1 = consolidacoesSalvas.First(c => c.Fluencia.Contains("Pré-leitor 1"));
            var fluencia2 = consolidacoesSalvas.First(c => c.Fluencia.Contains("Pré-leitor 2"));

            Assert.Equal(50, fluencia1.Percentual); 
            Assert.Equal(50, fluencia2.Percentual); 
            Assert.Equal(2, fluencia1.QuantidadeAluno);
            Assert.Equal(2, fluencia2.QuantidadeAluno);
        }

        [Fact]
        public async Task Executar_Sem_Registros_Filtrados_Nao_Deve_Chamar_Salvar()
        {
            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 0, "108900", "DRE TESTE", 1) 
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Testar_Todos_Tipos_Fluencia()
        {
            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 2),  
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 3), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 4), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 5), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 6), 
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 99) 
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> consolidacoesSalvas = null;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    if (req is PainelEducacionalSalvarFluenciaLeitoraCommand cmd)
                        consolidacoesSalvas = cmd.FluenciaLeitora.ToList();
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.NotNull(consolidacoesSalvas);
            Assert.Equal(7, consolidacoesSalvas.Count);

            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Pré-leitor 1");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Pré-leitor 2");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Pré-leitor 3");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Pré-leitor 4");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Leitor iniciante");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Leitor fluente");
            Assert.Contains(consolidacoesSalvas, c => c.Fluencia == "Não identificado");

            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == "Não leu");
            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == "Soletrou");
            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == "Silabou");
            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == "Leu até 10 palavras");
            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == ""); 
            Assert.Contains(consolidacoesSalvas, c => c.DescricaoFluencia == "Não identificado");
        }

        [Fact]
        public async Task Executar_Deve_Definir_UeCodigo_E_UeNome_Como_Null()
        {
            var mensagem = new MensagemRabbit();
            var registros = CriarRegistrosFluenciaLeitora();

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> consolidacoesSalvas = null;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    if (req is PainelEducacionalSalvarFluenciaLeitoraCommand cmd)
                        consolidacoesSalvas = cmd.FluenciaLeitora.ToList();
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.NotNull(consolidacoesSalvas);
            Assert.All(consolidacoesSalvas, c =>
            {
                Assert.Null(c.UeCodigo);
                Assert.Null(c.UeNome);
            });
        }

        [Fact]
        public async Task Obter_Consolidacoes_Por_Ano_E_Periodo_Sem_Registros_Filtrados_Deve_Retornar_Lista_Vazia()
        {
            var mensagem = new MensagemRabbit();
            var registros = new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1)
            };

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            List<PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto> consolidacoesSalvas = null;
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((req, token) =>
                {
                    if (req is PainelEducacionalSalvarFluenciaLeitoraCommand cmd)
                        consolidacoesSalvas = cmd.FluenciaLeitora.ToList();
                })
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            Assert.NotNull(consolidacoesSalvas);
            Assert.Single(consolidacoesSalvas);
        }

        [Fact]
        public async Task Executar_Com_Percentual_Zero_Quando_TotalAlunos_Zero()
        {         
            var mensagem = new MensagemRabbit();
            var registros = CriarRegistrosFluenciaLeitora();

            _repositorioFluenciaLeitoraMock
                .Setup(r => r.ObterRegistroFluenciaLeitoraGeralAsync())
                .ReturnsAsync(registros);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalExcluirFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalSalvarFluenciaLeitoraCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        [Fact]
        public void Construtor_Deve_Inicializar_Com_Dependencias()
        {
            var useCase = new ConsolidarFluenciaLeitoraPainelEducacionalUseCase(
                _mediatorMock.Object,
                _repositorioFluenciaLeitoraMock.Object);

            Assert.NotNull(useCase);
        }

        private List<PainelEducacionalRegistroFluenciaLeitoraDto> CriarRegistrosFluenciaLeitora()
        {
            return new List<PainelEducacionalRegistroFluenciaLeitoraDto>
            {
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 1),
                CriarRegistroFluenciaLeitora(2023, 1, "108900", "DRE TESTE", 2),
                CriarRegistroFluenciaLeitora(2023, 1, "108901", "DRE TESTE 2", 3)
            };
        }

        private PainelEducacionalRegistroFluenciaLeitoraDto CriarRegistroFluenciaLeitora(
            int anoLetivo,
            int periodo,
            string dreCodigo,
            string dreNome,
            int codigoFluencia)
        {
            return new PainelEducacionalRegistroFluenciaLeitoraDto
            {
                AnoLetivo = anoLetivo,
                Periodo = periodo,
                DreCodigo = dreCodigo,
                DreNome = dreNome,
                CodigoFluencia = codigoFluencia,
                UeCodigo = "123456",
                UeNome = "UE TESTE",
                TurmaNome = "1A"
            };
        }
    }
}