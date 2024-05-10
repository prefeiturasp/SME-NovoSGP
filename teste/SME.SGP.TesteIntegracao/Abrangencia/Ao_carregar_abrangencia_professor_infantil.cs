using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Abrangencia
{
    public class Ao_carregar_abrangencia_professor_infantil : TesteBaseComuns
    {
        public Ao_carregar_abrangencia_professor_infantil(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesPorTurmaIdQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesPorTurmaIdQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAtribuicoesPorTurmaEProfessorQuery, IEnumerable<AtribuicaoCJ>>), typeof(ObterAtribuicoesPorTurmaEProfessorQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<AtribuirPerfilCommand, Unit>), typeof(AtribuirPerfilCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverPerfisUsuarioAtualCommand>), typeof(RemoverPerfisUsuarioAtualCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>), typeof(PublicarFilaGoogleClassroomCommandHandlerFake), ServiceLifetime.Scoped));

        }


        [Fact]
        public async Task Nao_deve_remover_abrangencia_professor_infantil_somente_turmas_nao_infanfil()
        {
            CriarClaimUsuario(ObterPerfilProfessorInfantil());

            await InserirNaBase(new Usuario
            {
                Id = 1,
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = USUARIO_PROFESSOR_NOME_2222222,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
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

            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_1,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_1,
                DreId = 1,
                Nome = UE_NOME_1,
                TipoEscola = TipoEscola.EMEF
            });

            await CriarTurma(Modalidade.EducacaoInfantil, turmasMesmaUe: true);

            await InserirNaBase(new Dominio.Turma
            {
                Id = 5,
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = false,
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });

            var usuario = ObterTodos<Usuario>();

            await InserirNaBase(new Dominio.Abrangencia()
            {
                Historico = false,
                Perfil = Perfis.PERFIL_PROFESSOR_INFANTIL,
                TurmaId = TURMA_ID_1,
                UsuarioId = USUARIO_ID_1
            });

            var abrangenciasInseridas = ObterTodos<Dominio.Abrangencia>();

            var mediator = ServiceProvider.GetService<IMediator>();

            await mediator.Send(new CarregarAbrangenciaUsuarioCommand(USUARIO_PROFESSOR_LOGIN_2222222, Perfis.PERFIL_PROFESSOR_INFANTIL));

            var abrangenciasAtuais = ObterTodos<Dominio.Abrangencia>();

            Assert.NotNull(abrangenciasAtuais);
            Assert.NotEmpty(abrangenciasAtuais);
            abrangenciasAtuais.Any(a => a.TurmaId == 1).ShouldBeTrue();
        }
    }
}
