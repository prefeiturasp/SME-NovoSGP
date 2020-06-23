using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ServicoMenu : IServicoMenu
    {
        private readonly IServicoUsuario servicoUsuario;

        public ServicoMenu(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public IEnumerable<MenuRetornoDto> ObterMenu()
        {
            var permissoes = servicoUsuario.ObterPermissoes();

            var agrupamentos = permissoes.Where(c => Enum.IsDefined(typeof(Permissao), c)).GroupBy(item => new
            {
                Descricao = item.GetAttribute<PermissaoMenuAttribute>().Agrupamento,
                Ordem = item.GetAttribute<PermissaoMenuAttribute>().OrdemAgrupamento
            }).OrderBy(a => a.Key.Ordem)
            .ToList();

            var listaRetorno = new List<MenuRetornoDto>();

            foreach (var agrupamento in agrupamentos)
            {
                var permissao = agrupamento.First();
                var atributoEnumerado = permissao.GetAttribute<PermissaoMenuAttribute>();
                var menuRetornoDto = new MenuRetornoDto()
                {
                    Codigo = (int)permissao,
                    Descricao = atributoEnumerado.Agrupamento,
                    Icone = atributoEnumerado.Icone,
                    EhMenu = atributoEnumerado.EhMenu
                };

                var permissoesMenu = agrupamento.GroupBy(item => new
                {
                    item.GetAttribute<PermissaoMenuAttribute>().Menu,
                    Ordem = item.GetAttribute<PermissaoMenuAttribute>().OrdemMenu
                }).OrderBy(a => a.Key.Ordem)
                    .ToList();

                foreach (var permissaoMenu in permissoesMenu)
                {
                    var menu = permissaoMenu.First();
                    var menuEnumerado = menu.GetAttribute<PermissaoMenuAttribute>();

                    if (menuEnumerado.EhSubMenu)
                    {
                        var menuPai = new MenuPermissaoDto()
                        {
                            Codigo = (int)menu,
                            Descricao = menuEnumerado.Menu,

                        };

                        foreach (var subMenu in permissaoMenu.GroupBy( a => a.GetAttribute<PermissaoMenuAttribute>().Url))
                        {
                            if (menuEnumerado.EhSubMenu)
                            {
                                var menuSubEnumerado = subMenu.FirstOrDefault();
                                var menuSubEnumeradoComAtributo = menuSubEnumerado.GetAttribute<PermissaoMenuAttribute>();

                                menuPai.SubMenus.Add(new MenuPermissaoDto()
                                {
                                    Codigo = (int)menuSubEnumerado,
                                    Url = menuSubEnumeradoComAtributo.Url,
                                    Descricao = menuSubEnumeradoComAtributo.SubMenu,
                                    Ordem = menuSubEnumeradoComAtributo.OrdemSubMenu,                                    
                                    PodeConsultar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhConsulta),
                                    PodeAlterar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhAlteracao),
                                    PodeIncluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhInclusao),
                                    PodeExcluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhExclusao)
                                }) ;
                            }
                        }

                        menuRetornoDto.Menus.Add(menuPai);
                    }
                    else
                    {
                        menuRetornoDto.Menus.Add(new MenuPermissaoDto()
                        {
                            Codigo = (int)menu,
                            Url = menuEnumerado.Url,
                            Descricao = menuEnumerado.Menu,
                            PodeAlterar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhAlteracao),
                            PodeIncluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhInclusao),
                            PodeExcluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhExclusao),
                            PodeConsultar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhConsulta),
                        });
                    }
                }
                menuRetornoDto.Menus = menuRetornoDto.Menus.OrderBy(a => a.Ordem).ToList();
                listaRetorno.Add(menuRetornoDto);
            }
            return listaRetorno;
        }
    }
}