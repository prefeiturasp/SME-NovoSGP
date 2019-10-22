using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ServicoMenu
    {
        public IEnumerable<MenuRetornoDto> ObterMenu(IEnumerable<Permissao> permissoes)
        {
            //var agrupamentos = permissoes
            //                   .GroupBy(a => a.GetAttribute<PermissaoMenuAttribute>().Agrupamento)
            //                   .Distinct()
            //                   .ToList();

            var agrupamentos = permissoes.GroupBy(item => new
            {
                Descricao = item.GetAttribute<PermissaoMenuAttribute>().Agrupamento
            }).ToList();

            var listaRetorno = new List<MenuRetornoDto>();

            //foreach (var agrupamento in agrupamentos.OrderBy(a => a.GetAttribute<PermissaoMenuAttribute>().OrdemAgrupamento))
            //{
            //    var atributoEnumerado = agrupamento.GetAttribute<PermissaoMenuAttribute>();
            //    var menuRetornoDto = new MenuRetornoDto()
            //    {
            //        Codigo = (int)agrupamento,
            //        Descricao = atributoEnumerado.Agrupamento,
            //        Icone = atributoEnumerado.Icone
            //    };

            //    listaRetorno.Add(menuRetornoDto);
            //}
            return listaRetorno;
        }
    }
}