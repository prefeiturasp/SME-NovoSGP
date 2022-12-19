using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.TesteIntegracao.Documento
{
    public abstract class DocumentoTesteBase : TesteBaseComuns
    {
        public DocumentoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected IListarDocumentosUseCase ObterServicoListarDocumentosUseCase()
        {
            return ServiceProvider.GetService<IListarDocumentosUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroDocumentoDto filtroDocumentoDto)
        {
            await CriarTipoCalendario(filtroDocumentoDto.TipoCalendario);

            await CriarDreUePerfil();

            await CriarPeriodoEscolarTodosBimestres();

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroDocumentoDto.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroDocumentoDto.Modalidade);
        }

        protected async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

    }
}
