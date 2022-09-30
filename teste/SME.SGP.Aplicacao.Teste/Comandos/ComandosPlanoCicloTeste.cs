using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;
using Xunit;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosPlanoCicloTeste
    {
        private readonly ComandosPlanoCiclo comandosPlanoCiclo;
        private readonly Mock<IRepositorioMatrizSaberPlano> repositorioMatrizSaberPlano;
        private readonly Mock<IRepositorioObjetivoDesenvolvimentoPlano> repositorioObjetivoDesenvolvimentoPlano;
        private readonly Mock<IRepositorioPlanoCiclo> repositorioPlanoCiclo;
        private readonly Mock<IUnitOfWork> unitOfWork;

        private readonly Mock<IMediator> mediator;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public ComandosPlanoCicloTeste()
        {
            repositorioPlanoCiclo = new Mock<IRepositorioPlanoCiclo>();
            repositorioMatrizSaberPlano = new Mock<IRepositorioMatrizSaberPlano>();
            repositorioObjetivoDesenvolvimentoPlano = new Mock<IRepositorioObjetivoDesenvolvimentoPlano>();
            unitOfWork = new Mock<IUnitOfWork>();
            mediator = new Mock<IMediator>();
            configuracaoArmazenamentoOptions = Options.Create<ConfiguracaoArmazenamentoOptions>(new ConfiguracaoArmazenamentoOptions()
            {
                BucketArquivos = "Teste",
                BucketTemp = "Teste",
                EndPoint = "http://teste/",
                Port = 0
            });
            comandosPlanoCiclo = new ComandosPlanoCiclo(repositorioPlanoCiclo.Object,
                repositorioMatrizSaberPlano.Object,
                repositorioObjetivoDesenvolvimentoPlano.Object,
                unitOfWork.Object, mediator.Object, configuracaoArmazenamentoOptions);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(null, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object, mediator.Object, configuracaoArmazenamentoOptions));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, null, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object, mediator.Object, configuracaoArmazenamentoOptions));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, null, unitOfWork.Object, mediator.Object, configuracaoArmazenamentoOptions));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, null, mediator.Object, configuracaoArmazenamentoOptions));
        }

        [Fact(DisplayName = "DeveSalvarPlanoCiclo")]
        public async Task DeveSalvarPlanoCiclo()
        {
            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);


            mediator.Setup(a => a.Send(It.IsAny<MoverArquivosTemporariosCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("Teste");


            await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                {
                    1, 2, 3
                },
                IdsObjetivosDesenvolvimento = new List<long>()
                {
                    1, 2, 3
                }
            });
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Once);
        }

        [Fact(DisplayName = "DeveSalvarPlanoCicloAlterandoObjetivosEMatrizDoSaber")]
        public async Task DeveSalvarPlanoCicloAlterandoObjetivosEMatrizDoSaber()
        {
            repositorioObjetivoDesenvolvimentoPlano.Setup(c => c.ObterObjetivosDesenvolvimentoPorIdPlano(It.IsAny<long>()))
                .Returns(new List<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>()
                {
                    new RecuperacaoParalelaObjetivoDesenvolvimentoPlano()
                    {
                        Id = 1,
                        ObjetivoDesenvolvimentoId = 1,
                        PlanoId = 1
                    }
                });

            repositorioMatrizSaberPlano.Setup(c => c.ObterMatrizesPorIdPlano(It.IsAny<long>()))
                .Returns(new List<MatrizSaberPlano>()
                {
                    new MatrizSaberPlano()
                    {
                        Id = 1,
                        MatrizSaberId = 1,
                        PlanoId = 1
                    }
                });

            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);

            await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Id = 1,
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                {
                    3, 4, 5
                },
                IdsObjetivosDesenvolvimento = new List<long>()
                {
                    2, 3, 4
                }
            });
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Once);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloDuplicado")]
        public async Task NaoDeveSalvarPlanoCicloDuplicado()
        {
            repositorioPlanoCiclo.Setup(c => c.ObterPlanoCicloPorAnoCicloEEscola(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>()))
                .Returns(true);

            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                    {
                        1, 2, 3
                    },
                IdsObjetivosDesenvolvimento = new List<long>()
                    {
                        1, 2, 3
                    }
            }));
            Assert.Equal("Já existe um plano ciclo referente a este Ano/Ciclo/Escola.",
                exception.Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental")]
        public async Task NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental()
        {
            var exception = await Assert.ThrowsAsync<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsObjetivosDesenvolvimento = new List<long>()
                    {
                        1, 2, 3
                    }
            }));
            Assert.Equal("A matriz de saberes deve conter ao menos 1 elemento.",
                exception.Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental")]
        public async Task NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental()
        {
            var exception = await Assert.ThrowsAsync<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                    {
                        1, 2, 3
                    }
            }));

            Assert.Equal("Os objetivos de desenvolvimento sustentável devem conter ao menos 1 elemento.",
               exception.Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemParametros")]
        public async Task NaoDeveSalvarPlanoCicloSemParametros()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => comandosPlanoCiclo.Salvar(null));
            Assert.Equal("planoCicloDto",
                exception.ParamName);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }
    }
}