using MediatR;
using Moq;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtribuicaoResponsavel
{
    public class Ao_atribuir_ue_responsavel : TesteBase
    {
        private readonly AtribuirUeResponsavelUseCase atribuirUeResponsavelUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioSupervisorEscolaDre> repositorioSupervisorEscolaDre;

        public Ao_atribuir_ue_responsavel(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            mediator = new Mock<IMediator>();
            repositorioSupervisorEscolaDre = new Mock<IRepositorioSupervisorEscolaDre>();
            atribuirUeResponsavelUseCase = new AtribuirUeResponsavelUseCase(mediator.Object, repositorioSupervisorEscolaDre.Object);
        }

        [Fact]
        public async Task Deve_inserir_ue_para_o_responsavel_tipo_supervisor_escolar()
        {
            await CriarItensBasicos();

            var responsavelUe = new AtribuicaoResponsavelUEDto()
            {
                DreId = "1",
                ResponsavelId = "1",
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.SupervisorEscolar,
                UesIds = new()
                {
                    "1",
                    "2",
                    "3"
                }
            };

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

            await atribuirUeResponsavelUseCase.Executar(responsavelUe);

        }

        [Fact]
        public async Task Deve_inserir_ue_para_o_responsavel_tipo_paai()
        {
            await CriarItensBasicos();

            var responsavelUe = new AtribuicaoResponsavelUEDto()
            {
                DreId = "1",
                ResponsavelId = "2",
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.PAAI,
                UesIds = new()
                {
                    "1",
                    "2",
                    "3"
                }
            };

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

            await atribuirUeResponsavelUseCase.Executar(responsavelUe);

        }

        [Fact]
        public async Task Deve_inserir_ue_para_o_responsavel_tipo_psicologo_escolar()
        {
            await CriarItensBasicos();

            var responsavelUe = new AtribuicaoResponsavelUEDto()
            {
                DreId = "1",
                ResponsavelId = "3",
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.PsicologoEscolar,
                UesIds = new()
                {
                    "1",
                    "2",
                    "3"
                }
            };

            var parametro = new ObterResponsaveisPorDreDto("1", TipoResponsavelAtribuicao.PsicologoEscolar);

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

            await atribuirUeResponsavelUseCase.Executar(responsavelUe);

        }

        [Fact]
        public async Task Deve_inserir_ue_para_o_responsavel_tipo_psicopedagogo()
        {
            await CriarItensBasicos();

            var responsavelUe = new AtribuicaoResponsavelUEDto()
            {
                DreId = "1",
                ResponsavelId = "4",
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.Psicopedagogo,
                UesIds = new()
                {
                    "1",
                    "2",
                    "3"
                }
            };

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

            await atribuirUeResponsavelUseCase.Executar(responsavelUe);

        }

        [Fact]
        public async Task Deve_inserir_ue_para_o_responsavel_tipo_asistente_social()
        {
            await CriarItensBasicos();

            var responsavelUe = new AtribuicaoResponsavelUEDto()
            {
                DreId = "1",
                ResponsavelId = "5",
                TipoResponsavelAtribuicao = TipoResponsavelAtribuicao.AssistenteSocial,
                UesIds = new()
                {
                    "1",
                    "2",
                    "3"
                }
            };

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

            await atribuirUeResponsavelUseCase.Executar(responsavelUe);

        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE - 1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "UE - 1",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "2",
                DreId = 1,
                Nome = "UE - 1",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "3",
                DreId = 1,
                Nome = "UE - 1",
                TipoEscola = TipoEscola.Nenhum
            });

            mediator.Setup(c => c.Send(It.IsAny<ObterDREIdPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mediator.Setup(c => c.Send(It.IsAny<ObterUeComDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Ue()
                {
                    Id = 1,
                    CodigoUe = "1",
                    Nome = "UE - 1",
                    DreId = 1,
                    Dre = new Dre()
                    {
                        Id = 1,
                        Nome = "DRE - 1",
                        CodigoDre = "1",
                        Abreviacao = "DRE - 1"
                    },
                    TipoEscola = TipoEscola.Nenhum
                });
        }
    }
}
