using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Abrangencia.Base
{
    public abstract class AbrangenciaBase : TesteBaseComuns
    {
        protected AbrangenciaBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroTesteDto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtro.Perfil);

            await CriarUsuarios();

            await CriarTipoCalendario(filtro.TipoCalendario, filtro.ConsiderarAnoAnterior);

            await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior, tipoTurno: 2);
        }

        protected class FiltroTesteDto
        {
            public FiltroTesteDto()
            {
                ConsiderarAnoAnterior = false;
            }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
        }
    }
}
