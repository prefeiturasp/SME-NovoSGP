using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
    public class Ao_lancar_frequencia_componente : ListaoTesteBase
    {
        public Ao_lancar_frequencia_componente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Não deve Lançar frequência para componente que não lança frequencia")]
        public async Task Nao_deve_lancar_frequencia_para_componente_que_nao_lanca_frequencia()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = 3,
                Modalidade = Modalidade.Medio,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_APRENDIZAGEM_E_LEITURA_ID_1359,
                TipoTurno = (int)TipoTurnoEOL.Noite
            };

            await ExecutarTeste(filtroListao);
        }
        
        private async Task ExecutarTeste(FiltroListao filtroListao)
        {
            await CriarDadosBasicos(filtroListao);

            var listaAulaId = ObterTodos<Dominio.Aula>().Select(c => c.Id).Distinct().ToList();
            listaAulaId.ShouldNotBeNull();

            var frequenciasSalvar = listaAulaId.Select(aulaId => new FrequenciaSalvarAulaAlunosDto
                { AulaId = aulaId, Alunos = ObterListaFrequenciaSalvarAluno() }).ToList();

            //-> Salvar a frequencia
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();
            
            await useCaseSalvar.Executar(frequenciasSalvar)
                .ShouldThrowAsync<NegocioException>(MensagensNegocioFrequencia.Nao_e_permitido_registro_de_frequencia_para_este_componente);
        }       
    }
}