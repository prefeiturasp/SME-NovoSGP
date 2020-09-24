using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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

        public ComandosPlanoCicloTeste()
        {
            repositorioPlanoCiclo = new Mock<IRepositorioPlanoCiclo>();
            repositorioMatrizSaberPlano = new Mock<IRepositorioMatrizSaberPlano>();
            repositorioObjetivoDesenvolvimentoPlano = new Mock<IRepositorioObjetivoDesenvolvimentoPlano>();
            unitOfWork = new Mock<IUnitOfWork>();
            comandosPlanoCiclo = new ComandosPlanoCiclo(repositorioPlanoCiclo.Object,
                                                        repositorioMatrizSaberPlano.Object,
                                                        repositorioObjetivoDesenvolvimentoPlano.Object,
                                                        unitOfWork.Object);
        }

        [Fact(DisplayName = "DeveDispararExcecaoAoInstanciarSemDependencias")]
        public void DeveDispararExcecaoAoInstanciarSemDependencias()
        {
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(null, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, null, repositorioObjetivoDesenvolvimentoPlano.Object, unitOfWork.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, null, unitOfWork.Object));
            Assert.Throws<ArgumentNullException>(() => new ComandosPlanoCiclo(repositorioPlanoCiclo.Object, repositorioMatrizSaberPlano.Object, repositorioObjetivoDesenvolvimentoPlano.Object, null));
        }

        [Fact(DisplayName = "DeveSalvarPlanoCiclo")]
        public void DeveSalvarPlanoCiclo()
        {
            repositorioPlanoCiclo.Setup(c => c.Salvar(It.IsAny<PlanoCiclo>()))
                .Returns(1);

            comandosPlanoCiclo.Salvar(new PlanoCicloDto()
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
        public void DeveSalvarPlanoCicloAlterandoObjetivosEMatrizDoSaber()
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

            comandosPlanoCiclo.Salvar(new PlanoCicloDto()
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

            Assert.Equal("Já existe um plano ciclo referente a este Ano/Ciclo/Escola.",
                Assert.Throws<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
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
                })).Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental")]
        public void NaoDeveSalvarPlanoCicloSemIdsDaMatrizDoSaberParaTurmasEnsinoFundamental()
        {
            Assert.Equal("A matriz de saberes deve conter ao menos 1 elemento.",
                Assert.Throws<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
                {
                    Ano = 2019,
                    CicloId = 1,
                    Descricao = "Teste",
                    EscolaId = "1",
                    IdsObjetivosDesenvolvimento = new List<long>()
                    {
                        1,2,3
                    }
                })).Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental")]
        public void NaoDeveSalvarPlanoCicloSemIdsDosObjetivosDeDesenvolvimentoParaTurmasEnsinoFundamental()
        {
            Assert.Equal("Os objetivos de desenvolvimento sustentável devem conter ao menos 1 elemento.",
                Assert.Throws<NegocioException>(() => comandosPlanoCiclo.Salvar(new PlanoCicloDto()
                {
                    Ano = 2019,
                    CicloId = 1,
                    Descricao = "Teste",
                    EscolaId = "1",
                    IdsMatrizesSaber = new List<long>()
                    {
                        1,2,3
                    }
                })).Message);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }

        [Fact(DisplayName = "NaoDeveSalvarPlanoCicloSemParametros")]
        public void NaoDeveSalvarPlanoCicloSemParametros()
        {
            Assert.Equal("planoCicloDto",
                Assert.Throws<ArgumentNullException>(() => comandosPlanoCiclo.Salvar(null)).ParamName);
            unitOfWork.Verify(c => c.PersistirTransacao(), Times.Never);
        }
    }
}