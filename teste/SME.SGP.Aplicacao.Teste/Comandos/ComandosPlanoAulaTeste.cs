using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosPlanoAulaTeste
    {
        private readonly ComandosPlanoAula comandosPlanoAula;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly Mock<IConsultasObjetivoAprendizagem> consultasObjetivosAprendizagem;
        private readonly Mock<IConsultasPlanoAnual> consultasPlanoAnual;
        private readonly Mock<IConsultasProfessor> consultasProfessor;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioObjetivoAprendizagemPlano> repositorioObjetivoAprendizagemPlano;
        private readonly Mock<IRepositorioObjetivoAprendizagemAula> repositorioObjetivosAula;
        private readonly Mock<IRepositorioObjetivoAprendizagemPlano> repositorioObjetivosPlano;
        private readonly Mock<IRepositorioPlanoAula> repositorioPlanoAula;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly Mock<IMediator> mediator;
        private AbrangenciaFiltroRetorno abrangencia;
        private Aula aula;
        private Guid PERFIL_CJ = Guid.Parse("41e1e074-37d6-e911-abd6-f81654fe895d");
        private Guid PERFIL_PROFESSOR = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D");
        private PlanoAulaDto planoAulaDto;
        private Usuario usuario;

        public ComandosPlanoAulaTeste()
        {
            repositorioPlanoAula = new Mock<IRepositorioPlanoAula>();
            repositorioObjetivosAula = new Mock<IRepositorioObjetivoAprendizagemAula>();
            repositorioObjetivosPlano = new Mock<IRepositorioObjetivoAprendizagemPlano>();
            repositorioAula = new Mock<IRepositorioAula>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            repositorioObjetivoAprendizagemPlano = new Mock<IRepositorioObjetivoAprendizagemPlano>();
            repositorioAtribuicaoCJ = new Mock<IRepositorioAtribuicaoCJ>();
            unitOfWork = new Mock<IUnitOfWork>();
            servicoEOL = new Mock<IServicoEol>();
            consultasAbrangencia = new ConsultasAbrangencia(repositorioAbrangencia.Object, servicoUsuario.Object, servicoEOL.Object);
            consultasPlanoAnual = new Mock<IConsultasPlanoAnual>();
            consultasProfessor = new Mock<IConsultasProfessor>();
            consultasObjetivosAprendizagem = new Mock<IConsultasObjetivoAprendizagem>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            mediator = new Mock<IMediator>();

            comandosPlanoAula = new ComandosPlanoAula(repositorioPlanoAula.Object,
                                                    repositorioObjetivosAula.Object,
                                                    repositorioObjetivoAprendizagemPlano.Object,
                                                    repositorioAula.Object,
                                                    repositorioAtribuicaoCJ.Object,
                                                    consultasAbrangencia,
                                                    consultasObjetivosAprendizagem.Object,
                                                    consultasPlanoAnual.Object,
                                                    consultasProfessor.Object,
                                                    servicoUsuario.Object,
                                                    unitOfWork.Object,
                                                    repositorioPeriodoEscolar.Object,
                                                    mediator.Object);
            Setup();
        }

        [Fact]
        public async void Deve_Consistir_Plano_Aula_Sem_Objetivos_Modalidade_Fundamental()
        {

            repositorioAula
                .Setup(x => x.ObterPorId(It.IsAny<long>()))
                .Returns(new Aula());

            servicoEOL.Setup(x => x.ObterAbrangenciaCompactaVigente(It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new AbrangenciaCompactaVigenteRetornoEOLDTO()
                {
                    Abrangencia = new AbrangenciaCargoRetornoEolDTO()
                    {
                        Abrangencia = Infra.Enumerados.Abrangencia.Dre
                    }
                }));
            
            // ACT
            await Assert.ThrowsAsync<NegocioException>(() => comandosPlanoAula.Salvar(planoAulaDto));
        }

        //[Fact]
        //public async void Deve_Incluir_Plano_Aula_Com_Objetivos()
        //{
        //    // ARRANGE
        //    planoAulaDto.ObjetivosAprendizagemJurema = new List<long>() { 5 };

        //    // ACT
        //    await comandosPlanoAula.Salvar(planoAulaDto);

        //    // ASSERT
        //    Assert.True(true);
        //}

        //[Fact]
        //public async void Deve_Incluir_Plano_Aula_Sem_Objetivos_Libras()
        //{
        //    //ARRANGE
        //    aula.DisciplinaId = "218"; // Libras

        //    // ACT
        //    await comandosPlanoAula.Salvar(planoAulaDto);

        //    Assert.True(true);
        //}

        //[Fact]
        //public async void Deve_Incluir_Plano_Aula_Sem_Objetivos_Modalidade_EJA()
        //{
        //    //ARRANGE
        //    repositorioAbrangencia.Setup(a => a.ObterAbrangenciaTurma(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>()))
        //        .Returns(Task.FromResult(new AbrangenciaFiltroRetorno()
        //        {
        //            Ano = "2019",
        //            CodigoTurma = "123",
        //            Modalidade = Modalidade.EJA
        //        }));

        //    // ACT
        //    await comandosPlanoAula.Salvar(planoAulaDto);

        //    Assert.True(true);
        //}

        //[Fact]
        //public async void Deve_Incluir_Plano_Aula_Sem_Objetivos_Professor_CJ()
        //{
        //    //ARRANGE
        //    usuario.DefinirPerfis(new List<PrioridadePerfil>()
        //    {
        //        new PrioridadePerfil() { CodigoPerfil = PERFIL_PROFESSOR },
        //        new PrioridadePerfil() { CodigoPerfil = PERFIL_CJ }
        //    });

        //    usuario.DefinirPerfilAtual(PERFIL_CJ);

        //    // ACT
        //    await comandosPlanoAula.Salvar(planoAulaDto);

        //    Assert.True(true);
        //}

        private void Setup()
        {
            // Aula
            aula = new Aula()
            {
                DataAula = new DateTime(2019, 11, 18),
                TurmaId = "123",
                DisciplinaId = "7",
                Quantidade = 3,
                TipoAula = TipoAula.Normal
            };

            repositorioAula.Setup(a => a.ObterPorId(It.IsAny<long>()))
                .Returns(aula);

            // Plano Aula
            planoAulaDto = new PlanoAulaDto()
            {
                AulaId = 1,
                Descricao = "Teste de inclusão",
                DesenvolvimentoAula = "Desenvolvimento da aula",
            };

            repositorioPlanoAula.Setup(a => a.ObterPlanoAulaPorAula(It.IsAny<long>()))
                .Returns(Task.FromResult(new PlanoAula()));

            // Usuario
            usuario = new Usuario()
            {
                CodigoRf = "ABC",
            };
            usuario.DefinirPerfis(new List<PrioridadePerfil>()
                {
                    new PrioridadePerfil() { CodigoPerfil = PERFIL_PROFESSOR }
                });

            servicoUsuario.Setup(a => a.ObterLoginAtual()).Returns("teste");
            servicoUsuario.Setup(a => a.ObterPerfilAtual()).Returns(new Guid());
            servicoUsuario.Setup(a => a.ObterUsuarioLogado()).Returns(Task.FromResult(usuario));
            // Abrangencia
            abrangencia = new AbrangenciaFiltroRetorno()
            {
                Ano = "2019",
                CodigoTurma = "123",
                QtDuracaoAula = 3,
                Modalidade = Modalidade.Fundamental
            };

            repositorioAbrangencia.Setup(a => a.ObterAbrangenciaTurma(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(abrangencia));
        }
    }
}