using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RegistroColetivoNAAPA.Base
{
    public class RegistroColetivoTesteBase : TesteBaseComuns
    {

        public RegistroColetivoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CarregarTipoDeReuniao()
        {
            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.ATENDIMENTO_NAO_PRESENCIAL_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.GRUPO_TRABALHO_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.GRUPO_FOCAL_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.ITINERANCIA_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.PROJETO_TECER_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.REUNIAO_COMPARTILHADA_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.REUNIAO_MACRO_TERRITORIO_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.REUNIAO_MACRO_NAAPA_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.REUNIAO_MACRO_UE_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new TipoReuniaoNAAPA()
            {
                Titulo = TipoReuniaoConstants.REUNIAO_HORARIOS_NOME,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
