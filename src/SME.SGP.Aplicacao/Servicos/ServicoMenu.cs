using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ServicoMenu
    {
        public void ObterMenu(IEnumerable<Permissao> permissoes)
        {
            var agrupamentos = permissoes
                               .GroupBy(a => a.GetAttribute<DisplayAttribute>().GroupName)
                               .Distinct()
                               .ToList();

            //var listaRetorno = ;

            foreach (var agrupamento in agrupamentos)
            {
                var permissaoAgrupamento = agrupamento.First();
            }
        }
    }
}