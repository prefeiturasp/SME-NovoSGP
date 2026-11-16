using Microsoft.Extensions.Configuration;
using System;

namespace SME.SGP.Infra.Contexto
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ConexaoConsultasAttribute : Attribute
    {
        public const string SGP_POSTGRES_CONSULTAS = "SGP_PostgresConsultas";
        public const string SGP_POSTGRES_CONSULTAS_TURVO2 = "SGP_PostgresConsultas_turvo2";

        public ConexaoConsultasAttribute(string nomeConnectionString)
        {
            NomeConnectionString = nomeConnectionString;
        }

        public string NomeConnectionString { get; }

        public static string ObterNomeConexao(ConexaoConsultasAttribute atributo, IConfiguration configuration)
        {
            var nome = atributo?.NomeConnectionString;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(configuration?.GetConnectionString(nome)))
                return SGP_POSTGRES_CONSULTAS;

            return nome;
        }
    }
}
