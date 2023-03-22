using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_lancar_frequencia_bimestre_encerrado : ListaoTesteBase
    {
        public Ao_lancar_frequencia_bimestre_encerrado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));            
        }

        [Fact(DisplayName = "Frequência Listão - Não deve Lançar frequência para bimestre encerrado sem reabertura")]
        public async Task Nao_deve_lancar_frequencia_bimestre_encerrado_sem_reabertura()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = true,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CriarPeriodoReaberturaTodosBimestres = false
            };

            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            
            await useCaseSalvar.Executar(frequenciasSalvar)
                .ShouldThrowAsync<NegocioException>(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);
        }
        
        [Fact(DisplayName = "Frequência Listão - Lançamento de frequência para bimestre encerrado com reabertura")]
        public async Task Deve_lancar_frequencia_bimestre_encerrado_com_reabertura()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138
            };

            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            var retorno = await useCaseSalvar.Executar(frequenciasSalvar);
            retorno.ShouldNotBeNull();
            retorno.AulasIDsComErros.Any().ShouldBeFalse();
        }              
    }
}