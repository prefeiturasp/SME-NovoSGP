using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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

        public ComandosPlanoCicloTeste()
        {
            repositorioPlanoCiclo = new Mock<IRepositorioPlanoCiclo>();
            repositorioMatrizSaberPlano = new Mock<IRepositorioMatrizSaberPlano>();
            repositorioObjetivoDesenvolvimentoPlano = new Mock<IRepositorioObjetivoDesenvolvimentoPlano>();
            unitOfWork = new Mock<IUnitOfWork>();
            mediator = new Mock<IMediator>();
            comandosPlanoCiclo = new ComandosPlanoCiclo(repositorioPlanoCiclo.Object,
                                                        repositorioMatrizSaberPlano.Object,
                                                        repositorioObjetivoDesenvolvimentoPlano.Object,
                                                        unitOfWork.Object,mediator.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(null, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object, mediator.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, null, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object, mediator.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, null, unitOfWork.Object, mediator.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, null, mediator.Object));
        }

        [Fact(DisplayName = "DeveSalvarPlanoCiclo")]
        public async Task DeveSalvarPlanoCiclo()
        {
            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);

            await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                {
                    1,2,3
                },
                IdsObjetivosDesenvolvimento = new List<long>()
                {
                    1,2,3
                }
            });
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Once);
        }

        [Fact(DisplayName = "DeveSalvarPlanoCicloAlterandoObjetivosEMatrizDoSaber")]
        public async Task DeveSalvarPlanoCicloAlterandoObjetivosEMatrizDoSaber()
        {
            repositorioObjetivoDesenvolvimentoPlano.Setup(c => c.ObterObjetivosDesenvolvimentoPorIdPlano(It.IsAny<long>()))
                .Returns(new List<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>() {
                    new RecuperacaoParalelaObjetivoDesenvolvimentoPlano(){
                        Id=1,
                        ObjetivoDesenvolvimentoId=1,
                        PlanoId=1
                    }
                });

            repositorioMatrizSaberPlano.Setup(c => c.ObterMatrizesPorIdPlano(It.IsAny<long>()))
                .Returns(new List<MatrizSaberPlano>() {
                    new MatrizSaberPlano(){
                        Id=1,
                        MatrizSaberId=1,
                        PlanoId=1
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
                    3,4,5
                },
                IdsObjetivosDesenvolvimento = new List<long>()
                {
                    2,3,4
                }
            });
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Once);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloDuplicado")]
        public void NaoDeveSalvarPlanoCicloDuplicado()
        {
            repositorioPlanoCiclo.Setup(c => c.ObterPlanoCicloPorAnoCicloEEscola(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>()))
                .Returns(true);

            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);

            Assert.ThrowsAsync<NegocioException>(async () => await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
            {
                1,2,3
            },
                IdsObjetivosDesenvolvimento = new List<long>()
            {
                1,2,3
            }
            }));
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental")]
        public void NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental()
        {
            Assert.ThrowsAsync<NegocioException>(async () => await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsObjetivosDesenvolvimento = new List<long>()
                {
                    1,2,3
                }
            }));
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental")]
        public async Task NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental()
        {
            Assert.ThrowsAsync<NegocioException>(async () => await comandosPlanoCiclo.Salvar(new PlanoCicloDto()
            {
                Ano = 2019,
                CicloId = 1,
                Descricao = "Teste",
                EscolaId = "1",
                IdsMatrizesSaber = new List<long>()
                {
                    1,2,3
                }
            }));
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemParametros")]
        public async Task NaoDeveSalvarPlanoCicloSemParametros()
        {
            Assert.Equal("planoCicloDto",
                (await Assert.ThrowsAsync<ArgumentNullException>(async() => await comandosPlanoCiclo.Salvar(null))).ParamName);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }
    }
}