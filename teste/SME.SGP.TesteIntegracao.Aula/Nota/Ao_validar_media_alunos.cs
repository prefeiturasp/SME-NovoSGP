using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CadastrarAulaRepetirTodosBimestres
{
    public class Ao_validar_media_alunos : TesteBaseComuns
    {
        //private const long TIPO_CALENDARIO_1 = 1;

        //private readonly DateTime DATA_03_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 03);
        //private readonly DateTime DATA_29_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 29);

        //private readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        //private readonly DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        //private readonly DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        //private readonly DateTime DATA_30_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);

        //private readonly DateTime DATA_03_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        //private readonly DateTime DATA_22_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        //private readonly DateTime DATA_24_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 24);

        public Ao_validar_media_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        }

        [Fact]
        public async Task Deve_permitir_cadastrar_aula_normal_bimestral_professor_especialista_fundamental()
        {
            //FiltroValidarMediaAlunosDto

            await InserirNaBase(new ParametrosSistema() 
            {
                Nome = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_NOME,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Descricao = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_DESCRICAO,
                Valor = PARAMETRO_PERCENTUAL_ALUNOS_INSUFICIENTES_TIPO_15_VALOR_50,
                Ano = DateTimeExtension.HorarioBrasilia().Date.Year,
                Ativo = true,
                CriadoEm = System.DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            

            //await CriarPeriodoEscolarEPeriodoReabertura();

            //var retorno = await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirTodosBimestres, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            //var aulasCadastradas = ObterTodos<Aula>();

            //aulasCadastradas.Count().ShouldBeEquivalentTo(49);

            //aulasCadastradas.Where(w => !w.Excluido).Count().ShouldBeEquivalentTo(49);

            //aulasCadastradas.Where(w => w.DisciplinaId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString()).Count().ShouldBeEquivalentTo(49);
        }
    }
}