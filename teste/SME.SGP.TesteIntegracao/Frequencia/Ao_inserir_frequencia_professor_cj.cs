using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;


namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_frequencia_professor_cj : FrequenciaBase
    {
        private DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_inserir_frequencia_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_registrar_frenquecia_professor_cj_ensino_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, false);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarPeriodoEscolarEAberturaPadrao();

            await InserirFrequenciaUseCaseBasica();
        }
    }
}
