using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.TesteIntegracao.ServicosFake;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    public abstract class DiarioBordoTesteBase : TesteBaseComuns
    {
        protected const long DIARIO_BORDO_ID_1 = 1;
        protected new const long AULA_ID_1 = 1;
        protected const long DIARIO_BORDO_OBS_ID_1 = 1;
        protected const long DIARIO_BORDO_OBS_ID_2 = 2;
        protected const long DEVOLUTIVA_DIARIO_BORDO_ID_1 = 1;

        public DiarioBordoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery,AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>),
                typeof(ObterUsuarioLogadoPaai4444444QueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>),
                typeof(DiarioBordo.ServicosFakes.ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeDiarioClasse), ServiceLifetime.Scoped));

            
        }

        protected IExcluirDiarioBordoUseCase ObterServicoExcluirDiarioBordoUseCase()
        {
            return ServiceProvider.GetService<IExcluirDiarioBordoUseCase>();
        }
                
        protected async Task CriarDadosBasicos(FiltroDiarioBordoDto filtroDiarioBordoDto, bool criarDiario = true)
        {
            await CriarDreUePerfil();
            CriarClaimUsuario(PerfilUsuario.PROFESSOR.Name());
            await CriarUsuarios();
            await CriarPeriodoEscolar(DateTimeExtension.HorarioBrasilia().AddDays(-7), DateTimeExtension.HorarioBrasilia().AddDays(60), 3);
            await CriarComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarTurma(Modalidade.Fundamental);
            await CriarAula(filtroDiarioBordoDto.DataAulaDiarioBordo, RecorrenciaAula.AulaUnica, 
                            TipoAula.Normal, 
                            USUARIO_PROFESSOR_CODIGO_RF_1111111, 
                            TURMA_CODIGO_1, UE_CODIGO_1,
                            filtroDiarioBordoDto.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            if (criarDiario)
                await CriarDiarioBordo(filtroDiarioBordoDto);
        }

        protected async Task CriarDiarioBordo(FiltroDiarioBordoDto filtroDiarioBordoDto)
        {
            if (filtroDiarioBordoDto.ContemDevolutiva)
            {
                await InserirNaBase(new Devolutiva()
                {
                    Id = DEVOLUTIVA_DIARIO_BORDO_ID_1,
                    CodigoComponenteCurricular = filtroDiarioBordoDto.ComponenteCurricularId,
                    PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddDays(-7),
                    PeriodoFim = DateTimeExtension.HorarioBrasilia().AddDays(60),
                    Descricao = "Devolutiva Diário de Bordo 01",
                    Excluido = false,
                    DiariosBordo = null,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = "Sistema",
                    CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
                });
            }

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = filtroDiarioBordoDto.ComponenteCurricularId,
                TurmaId = TURMA_ID_1,
                DevolutivaId = filtroDiarioBordoDto.ContemDevolutiva ? DEVOLUTIVA_DIARIO_BORDO_ID_1 : null,
                Planejamento = "Planejado",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            if (filtroDiarioBordoDto.ContemObservacoes)
            {
                await InserirNaBase(new DiarioBordoObservacao() { 
                    Id = DIARIO_BORDO_OBS_ID_1,
                    Observacao = "Observação Diário de Bordo 01",
                    DiarioBordoId = DIARIO_BORDO_ID_1,
                    UsuarioId = USUARIO_ID_1,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = "Sistema",
                    CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
                });

                await InserirNaBase(new DiarioBordoObservacao()
                {
                    Id = DIARIO_BORDO_OBS_ID_2,
                    Observacao = "Observação Diário de Bordo 02",
                    DiarioBordoId = DIARIO_BORDO_ID_1,
                    UsuarioId = USUARIO_ID_2,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = "Sistema",
                    CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
                });
            }
        }
    }
}
