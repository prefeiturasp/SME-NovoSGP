using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake;
using SME.SGP.TesteIntegracao.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtribuicaoCJs
{
    public class Ao_remover_uma_atribuicao_cj_nao_deve_remover_abrangencia_da_turma : TesteBaseComuns
    {
        public Ao_remover_uma_atribuicao_cj_nao_deve_remover_abrangencia_da_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesPorTurmaIdQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesPorTurmaIdQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAtribuicoesPorTurmaEProfessorQuery, IEnumerable<AtribuicaoCJ>>),typeof(ObterAtribuicoesPorTurmaEProfessorQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<AtribuirPerfilCommand,Unit>), typeof(AtribuirPerfilCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverPerfisUsuarioAtualCommand>), typeof(RemoverPerfisUsuarioAtualCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaGoogleClassroomCommand, bool>), typeof(PublicarFilaGoogleClassroomCommandHandlerFake), ServiceLifetime.Scoped));

        }


        [Fact]
        public async Task Ao_remover_uma_atribuicao_cj_nao_deve_remover_abrangencia()
        {
            CriarClaimUsuario(ObterPerfilProfessor());
            await CriarUsuarios();
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.EducacaoInfantil);
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = Perfis.PERFIL_CJ_INFANTIL,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });

            var disciplinasAtribuicaoCJ = new List<AtribuicaoCJPersistenciaItemDto>
            {
                new AtribuicaoCJPersistenciaItemDto() { DisciplinaId = 1, Substituir = false },
                new AtribuicaoCJPersistenciaItemDto() { DisciplinaId = 2, Substituir = true }
            };

            var atribuicaoCJ = new AtribuicaoCJPersistenciaDto()
            {
                AnoLetivo = "2023",
                DreId = DRE_ID_1.ToString(),
                UeId = UE_ID_1.ToString(),
                TurmaId = TURMA_ID_1.ToString(),
                Disciplinas = disciplinasAtribuicaoCJ,
                Modalidade = Modalidade.EducacaoInfantil,
                Historico = false,
                UsuarioRf = USUARIO_PROFESSOR_LOGIN_2222222.ToString()
            };

            var mediator = ServiceProvider.GetService<IMediator>();

            var salvarAtribuicao = new SalvarAtribuicaoCJUseCase(mediator);
            await salvarAtribuicao.Executar(atribuicaoCJ);

            var abrangenciasAtuais = ObterTodos<Abrangencia>();
            Assert.NotNull(abrangenciasAtuais);
            Assert.NotEmpty(abrangenciasAtuais);
        }
    }
}
