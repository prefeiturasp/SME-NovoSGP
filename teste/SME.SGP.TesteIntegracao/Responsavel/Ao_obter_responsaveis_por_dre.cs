using MediatR;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ResponsaveisPorDre
{
    public class Ao_obter_responsaveis_por_dre : TesteBase
    {
        private readonly ObterResponsaveisPorDreUseCase obterResponsaveisPorDreUseCase;
        private readonly Mock<IMediator> mediator;

        public Ao_obter_responsaveis_por_dre(CollectionFixture collectionFixture) : base(collectionFixture)
        {            
            mediator = new Mock<IMediator>();
            obterResponsaveisPorDreUseCase = new ObterResponsaveisPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_tipo_supervisor_escolar()
        {
            var supervisores = new List<SupervisoresRetornoDto>
            {
                new SupervisoresRetornoDto()
                {
                    CodigoRf = "1",
                    NomeServidor = "Nome 001"
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterSupervisoresPorDreEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(supervisores);

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.SupervisorEscolar);            
            var resultados = await ObterResultados(parametro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_tipo_paai()
        {
            var funcionariosEol = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto()
                {
                    UsuarioId = 2,
                    CodigoRf = "2",
                    NomeServidor = "Nome 002",
                    EstaAfastado = false
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterFuncionariosPorDreECargoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionariosEol);

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.PAAI);
            var resultados = await ObterResultados(parametro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_tipo_psicologo_escolar()
        {
            var funcionariosUnidades = new List<FuncionarioUnidadeDto>
            {
                new FuncionarioUnidadeDto()
                {
                    Login = "3",
                    NomeServidor = "Nome 003"
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterFuncionariosDreOuUePorPerfisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionariosUnidades);

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.PsicologoEscolar);
            var resultados = await ObterResultados(parametro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_tipo_psicopedagogo()
        {
            var funcionariosUnidades = new List<FuncionarioUnidadeDto>
            {
                new FuncionarioUnidadeDto()
                {
                    Login = "4",
                    NomeServidor = "Nome 004"
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterFuncionariosDreOuUePorPerfisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionariosUnidades);

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.Psicopedagogo);
            var resultados = await ObterResultados(parametro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_responsavel_tipo_assistente_social()
        {
            var funcionariosUnidades = new List<FuncionarioUnidadeDto>
            {
                new FuncionarioUnidadeDto()
                {
                    Login = "5",
                    NomeServidor = "Nome 005"
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterFuncionariosDreOuUePorPerfisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(funcionariosUnidades);

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.AssistenteSocial);
            var resultados = await ObterResultados(parametro);

            resultados.ShouldNotBeEmpty();
            resultados.Count().ShouldBe(1);
        }

        private async Task<IEnumerable<SupervisorDto>> ObterResultados(ObterResponsaveisPorDreDto parametro)
        {
            return await obterResponsaveisPorDreUseCase.Executar(parametro);
        }

        /* TODO
        private async Task CriarRegistrosParaConsulta()
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                Id = 1,
                SupervisorId = "1",
                EscolaId = "1",
                DreId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE INTEGRAÇÃO",
                CriadoRF = "",
                Excluido = false,
                Tipo = 1
            });

            await InserirNaBase(new SupervisorEscolaDre()
            {
                Id = 1,
                SupervisorId = "2",
                EscolaId = "1",
                DreId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE INTEGRAÇÃO",
                CriadoRF = "",
                Excluido = false,
                Tipo = 2
            });

            await InserirNaBase(new SupervisorEscolaDre()
            {
                Id = 1,
                SupervisorId = "3",
                EscolaId = "1",
                DreId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE INTEGRAÇÃO",
                CriadoRF = "",
                Excluido = false,
                Tipo = 3
            });

            await InserirNaBase(new SupervisorEscolaDre()
            {
                Id = 1,
                SupervisorId = "4",
                EscolaId = "1",
                DreId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE INTEGRAÇÃO",
                CriadoRF = "",
                Excluido = false,
                Tipo = 4
            });

            await InserirNaBase(new SupervisorEscolaDre()
            {
                Id = 1,
                SupervisorId = "5",
                EscolaId = "1",
                DreId = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "TESTE INTEGRAÇÃO",
                CriadoRF = "",
                Excluido = false,
                Tipo = 5
            });
        }
        */
    }
}
