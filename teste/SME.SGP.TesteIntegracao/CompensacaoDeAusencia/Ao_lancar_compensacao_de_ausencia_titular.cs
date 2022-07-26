using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_titular : Ao_lancar_compensacao_de_ausencia_base
    {
        public Ao_lancar_compensacao_de_ausencia_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_fundamental()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                        Modalidade.Fundamental,
                        ModalidadeTipoCalendario.FundamentalMedio,
                        ANO_7);

            await ExecuteTeste(dto);
        }

        [Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_medio()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                        Modalidade.Medio,
                        ModalidadeTipoCalendario.FundamentalMedio,
                        ANO_1);

            await ExecuteTeste(dto);
        }

        [Fact]
        public async Task Deve_lancamento_compensacao_ausencia_titular_eja()
        {
            var dto = ObtenhaDtoDadoBase(
                        COMPONENTE_GEOGRAFIA_ID_8,
                        Modalidade.EJA,
                        ModalidadeTipoCalendario.EJA,
                        ANO_3);

            await ExecuteTeste(dto);
        }

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(
                                        string componente,
                                        Modalidade modalidade,
                                        ModalidadeTipoCalendario modalidadeTipo,
                                        string ano)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipo,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ano,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}
