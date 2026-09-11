using System;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class ComponenteCurricularEolVigenciaTeste
    {
        private static readonly DateTime INICIO = new DateTime(2026, 2, 19);
        private static readonly DateTime FIM = new DateTime(2026, 2, 23);

        [Fact]
        public void Atribuicao_ativa_alcanca_qualquer_data_do_ano()
        {
            var componente = new ComponenteCurricularEol
            {
                AtribuicaoAtiva = true,
                InicioAtribuicao = INICIO,
                FimAtribuicao = null
            };

            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 1, 5)));
            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 6, 15)));
            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 12, 20)));
        }

        [Fact]
        public void Atribuicao_encerrada_alcanca_data_dentro_do_periodo()
        {
            var componente = ComponenteEncerrado();

            Assert.True(componente.AtribuicaoAlcancaData(INICIO));
            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 2, 20)));
            Assert.True(componente.AtribuicaoAlcancaData(FIM));
        }

        [Fact]
        public void Atribuicao_encerrada_nao_alcanca_data_posterior_ao_fim()
        {
            var componente = ComponenteEncerrado();

            Assert.False(componente.AtribuicaoAlcancaData(FIM.AddDays(1)));
            Assert.False(componente.AtribuicaoAlcancaData(new DateTime(2026, 4, 9)));
        }

        [Fact]
        public void Atribuicao_encerrada_nao_alcanca_data_anterior_ao_inicio()
        {
            var componente = ComponenteEncerrado();

            Assert.False(componente.AtribuicaoAlcancaData(INICIO.AddDays(-1)));
            Assert.False(componente.AtribuicaoAlcancaData(new DateTime(2026, 2, 6)));
        }

        [Fact]
        public void Componente_sem_vigencia_conhecida_nao_recorta()
        {
            var componente = new ComponenteCurricularEol
            {
                AtribuicaoAtiva = false,
                InicioAtribuicao = null,
                FimAtribuicao = null
            };

            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 1, 5)));
            Assert.True(componente.AtribuicaoAlcancaData(new DateTime(2026, 12, 20)));
        }

        [Fact]
        public void Hora_da_aula_nao_interfere_na_comparacao()
        {
            var componente = ComponenteEncerrado();

            Assert.True(componente.AtribuicaoAlcancaData(FIM.AddHours(23).AddMinutes(59)));
            Assert.False(componente.AtribuicaoAlcancaData(INICIO.AddDays(-1).AddHours(23)));
        }

        private static ComponenteCurricularEol ComponenteEncerrado()
            => new ComponenteCurricularEol
            {
                AtribuicaoAtiva = false,
                InicioAtribuicao = INICIO,
                FimAtribuicao = FIM
            };
    }
}
