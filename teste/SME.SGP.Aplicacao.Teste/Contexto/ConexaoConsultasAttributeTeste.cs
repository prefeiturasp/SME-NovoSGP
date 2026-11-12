using Microsoft.Extensions.Configuration;
using SME.SGP.Infra.Contexto;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Contexto
{
    public class ConexaoConsultasAttributeTeste
    {
        private static IConfiguration Configuracao(params string[] connectionStrings)
        {
            var valores = new Dictionary<string, string>();
            foreach (var nome in connectionStrings)
                valores[$"ConnectionStrings:{nome}"] = "Host=x;Database=y";

            return new ConfigurationBuilder().AddInMemoryCollection(valores).Build();
        }

        [Fact]
        public void Deve_Usar_Conexao_Padrao_Quando_Endpoint_Nao_Tem_Atributo()
        {
            //Arrange
            var configuracao = Configuracao(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS, ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS_TURVO2);

            //Act
            var nome = ConexaoConsultasAttribute.ObterNomeConexao(null, configuracao);

            //Assert
            Assert.Equal(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS, nome);
        }

        [Fact]
        public void Deve_Usar_Conexao_Do_Atributo_Quando_Ela_Existe_Na_Configuracao()
        {
            //Arrange
            var configuracao = Configuracao(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS, ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS_TURVO2);
            var atributo = new ConexaoConsultasAttribute(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS_TURVO2);

            //Act
            var nome = ConexaoConsultasAttribute.ObterNomeConexao(atributo, configuracao);

            //Assert
            Assert.Equal(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS_TURVO2, nome);
        }

        [Fact]
        public void Deve_Voltar_Para_Conexao_Padrao_Quando_Conexao_Do_Atributo_Nao_Esta_Configurada()
        {
            //Arrange
            var configuracao = Configuracao(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS);
            var atributo = new ConexaoConsultasAttribute(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS_TURVO2);

            //Act
            var nome = ConexaoConsultasAttribute.ObterNomeConexao(atributo, configuracao);

            //Assert
            Assert.Equal(ConexaoConsultasAttribute.SGP_POSTGRES_CONSULTAS, nome);
        }
    }
}
