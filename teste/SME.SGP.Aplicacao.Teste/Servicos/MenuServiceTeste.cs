using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    public class MenuServiceTeste
    {
        [Fact]
        public void MontaMenu()
        {
            var servicoMenu = new ServicoMenu();
            var permissoes = new List<Permissao>() { Permissao.SR_C, Permissao.S_I, Permissao.S_C, Permissao.F_A, Permissao.USSA_A };

            var menu = servicoMenu.ObterMenu(permissoes);

            Assert.True(menu.Count() > 0);
        }
    }
}