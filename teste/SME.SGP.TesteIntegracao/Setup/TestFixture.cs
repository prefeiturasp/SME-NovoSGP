using System;

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