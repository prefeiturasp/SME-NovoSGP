using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ServicoMenu : IServicoMenu
    {
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ServicoMenu(IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<MenuRetornoDto>> ObterMenu()
        {
            var ajudas = (await mediator.Send(ObterAjudasDoSistemaQuery.Instance)).ToList();

            var teste = ajudas.Where(x => x.IdModulo == 114);
            var permissoes = servicoUsuario.ObterPermissoes();

            var agrupamentos = permissoes.Where(c => Enum.IsDefined(typeof(Permissao), c)).GroupBy(item => new
            {
                Descricao = item.GetAttribute<PermissaoMenuAttribute>().Agrupamento,
                Ordem = item.GetAttribute<PermissaoMenuAttribute>().OrdemAgrupamento
            }).OrderBy(a => a.Key.Ordem).ToList();

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

                var permissoesMenu = agrupamento.GroupBy(item => new MenuOrdem(item.GetAttribute<PermissaoMenuAttribute>().Menu,
                                                                               item.GetAttribute<PermissaoMenuAttribute>().OrdemMenu)).OrderBy(a => a.Key.Ordem).ToList();

                AdicionarPermissoesMenu(menuRetornoDto, permissoesMenu, ajudas);
                
                menuRetornoDto.Menus = menuRetornoDto.Menus.OrderBy(a => a.Ordem).ToList();
                listaRetorno.Add(menuRetornoDto);
            }
            return listaRetorno;
        }

        private record MenuOrdem(string Menu, int Ordem);

        private void AdicionarPermissoesMenu(MenuRetornoDto menuRetornoDto, List<IGrouping<MenuOrdem, Permissao>> permissoesMenu, IEnumerable<AjudaDoSistemaDto> ajudas)
        {
            foreach (var permissaoMenu in permissoesMenu)
            {
                var menu = permissaoMenu.First();
                var menuEnumerado = menu.GetAttribute<PermissaoMenuAttribute>();

                if (menuEnumerado.EhSubMenu)
                {
                    var menuPai = new MenuPermissaoDto
                    {
                        Codigo = (int)menu,
                        Descricao = menuEnumerado.Menu,
                    };

                    AdicionarPermissoesSubMenu(menuPai, permissaoMenu, ajudas, menuEnumerado.EhSubMenu);
                    menuRetornoDto.Menus.Add(menuPai);
                }
                else
                {
                    var url = ObterUrlComRedirect(menuEnumerado);
                    menuRetornoDto.Menus.Add(new MenuPermissaoDto()
                    {
                        Codigo = (int)menu,
                        Url = url,
                        Descricao = menuEnumerado.Menu,
                        PodeAlterar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhAlteracao),
                        PodeIncluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhInclusao),
                        PodeExcluir = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhExclusao),
                        PodeConsultar = permissaoMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhConsulta),
                        AjudaDoSistema = ajudas.FirstOrDefault(c => c.IdModulo == (int)menu)?.AjudaDoSistema
                    });
                }
            }
        }

        private void AdicionarPermissoesSubMenu(MenuPermissaoDto menuPai, IGrouping<MenuOrdem, Permissao> permissaoMenu, IEnumerable<AjudaDoSistemaDto> ajudas, bool ehSubMenu)
        {
            foreach (var subMenu in permissaoMenu.OrderBy(o => o.GetAttribute<PermissaoMenuAttribute>().OrdemSubMenu).GroupBy(a => a.GetAttribute<PermissaoMenuAttribute>().Url))
            {
                if (!ehSubMenu)
                    continue;

                var menuSubEnumerado = subMenu.FirstOrDefault();
                var menuSubEnumeradoComAtributo = menuSubEnumerado.GetAttribute<PermissaoMenuAttribute>();

                var url = ObterUrlComRedirect(menuSubEnumeradoComAtributo);

                var permissoesSubMenu = permissaoMenu.Where(c =>
                    c.GetAttribute<PermissaoMenuAttribute>().Url == subMenu.Key).ToList();

                menuPai.SubMenus.Add(new MenuPermissaoDto()
                {
                    Codigo = (int)menuSubEnumerado,
                    Url = url,
                    Descricao = menuSubEnumeradoComAtributo.SubMenu,
                    Ordem = menuSubEnumeradoComAtributo.OrdemSubMenu,
                    PodeConsultar = permissoesSubMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhConsulta),
                    PodeAlterar = permissoesSubMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhAlteracao),
                    PodeIncluir = permissoesSubMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhInclusao),
                    PodeExcluir = permissoesSubMenu.Any(a => a.GetAttribute<PermissaoMenuAttribute>().EhExclusao),
                    AjudaDoSistema = ajudas.FirstOrDefault(c => c.IdModulo == (int)menuSubEnumerado)?.AjudaDoSistema
                });
            }
        }

        private string ObterUrlComRedirect(PermissaoMenuAttribute permissaoMenuAttribute)
        {
            var url = permissaoMenuAttribute.Url;

            return url;
        }
    }
}