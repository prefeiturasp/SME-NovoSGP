using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Autenticar
{
    public class Ao_logar_com_usuario_professor_infantil : TesteBaseComuns
    {
        public Ao_logar_com_usuario_professor_infantil(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>), typeof(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilAutenticacaoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPerfisPorLoginQuery, PerfisApiEolDto>), typeof(ObterPerfisPorLoginAutenticacaoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoAutenticacaoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterLoginAtualQuery, string>), typeof(ObterLoginAtualAutenticacaoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_retornar_somente_turmas_infantis()
        {
            CriarClaimUsuario(Perfis.PERFIL_PROFESSOR_INFANTIL.ToString());

            await CriaItens();
            var comando = ServiceProvider.GetService<IComandosUsuario>();
            var retorno = await comando.ModificarPerfil(Perfis.PERFIL_PROFESSOR_INFANTIL);

            retorno.ShouldNotBeNull();

            var abrangencia = ObterTodos<Abrangencia>();
            var turmas = ObterTodos<Dominio.Turma>();

            abrangencia.ShouldNotBeNull();
            abrangencia.FirstOrDefault().TurmaId.Equals("3");
            turmas.FirstOrDefault(t => t.Id == abrangencia.FirstOrDefault().TurmaId).ModalidadeCodigo.ShouldBe(Modalidade.EducacaoInfantil);
            abrangencia.FirstOrDefault().Perfil.ShouldBe(Perfis.PERFIL_PROFESSOR_INFANTIL);

        }

        private async Task CriaItens()
        {
            await InserirNaBase(new Usuario()
            {   
                Id = 1,
                CodigoRf = "PROFINF1",
                Login = "PROFINF1",
                Nome = "PROFINF1",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                NomePerfil = "Perfil Professor Infantil",
                CodigoPerfil = Perfis.PERFIL_PROFESSOR_INFANTIL,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1111111",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 2,
                Nome = "2A",
                CodigoTurma = "2222222",
                Ano = "2",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Programa,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 3,
                Nome = "3A",
                CodigoTurma = "3333333",
                Ano = "3",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                UeId = 1
            });
        }
    }
}
