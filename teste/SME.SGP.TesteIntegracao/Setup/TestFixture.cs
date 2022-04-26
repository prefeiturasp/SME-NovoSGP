using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dados;
using SME.SGP.Infra;
using System;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dados.Contexto;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class TestFixture : IDisposable
    {
    //RaphaelDias. Removi o que estava nessa classe para passar pro CollectionFixture. 
    //Se for necessário paralelizar os testes, o ideal é que passe o controle do banco de dados
    //pra cá para cada classe ter o seu e ser possível paralelizar. Neste momento, não é necessário.
        public void Dispose()
        {
        }
    }
}