using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Teste : RegistroIndividualTesteBase
    {
        public Teste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        [Fact(DisplayName = "Registro Individual - Teste básico")]
        public async Task Testar()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            await CriarDadosBasicos(new FiltroRegistroIndividualDto()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
            });

            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                Data = DateTimeExtension.HorarioBrasilia().Date,
                ComponenteCurricularId = 512,
                Registro = "Descrição do registro individual"
            };
            
            var retorno = await inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            
            var registrosIndividuais = ObterTodos<Dominio.RegistroIndividual>();
            registrosIndividuais.Any().ShouldBeTrue();
            registrosIndividuais.FirstOrDefault().Id.ShouldBe(1);
        }
    }
}