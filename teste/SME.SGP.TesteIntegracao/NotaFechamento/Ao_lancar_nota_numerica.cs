using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using SME.SGP.TesteIntegracao.NotaFechamento.Base;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamento
{
    public class Ao_lancar_nota_numerica : NotaFechamentoTesteBase
    {
        public Ao_lancar_nota_numerica(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact]
        public async Task Deve_permitir_lancamento_nota_numerica_titular_fundamental()
        {
            var filtroNotaFechamento = ObterFiltroNotasFechamento(
                ObterPerfilProfessor(), 
                TipoNota.Nota, ANO_7,
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio, 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
                
            await CriarDadosBase(filtroNotaFechamento);

            await ExecutarComandosFechamentoFinal(filtroNotaFechamento);
        }

        public async Task Deve_permitir_lancamento_nota_numerica_titular_medio()
        {
        }
        
        public async Task Deve_permitir_lancamento_nota_numerica_titular_eja()
        {
        }
        
        public async Task Deve_permitir_lancamento_nota_numerica_titular_regencia_classe_fundamental()
        {
        }

        private FiltroNotaFechamentoDto ObterFiltroNotasFechamento(string perfil, TipoNota tipoNota, string anoTurma,Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular , bool considerarAnoAnterior = false)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            };
        }
    }
}