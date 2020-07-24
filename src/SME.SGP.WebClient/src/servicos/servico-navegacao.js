import tipoPermissao from '~/dtos/tipoPermissao';
import { store } from '~/redux';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { setMenu, setPermissoes } from '~/redux/modulos/usuario/actions';
import api from '~/servicos/api';

const setMenusPermissoes = () => {
  let permissoes = {};
  let menus = [];
  api.get('v1/menus').then(resp => {
    resp.data.forEach(item => {
      let subMenu = {
        codigo: item.codigo,
        descricao: item.descricao,
        ehMenu: item.ehMenu,
        icone: item.icone,
        quantidadeMenus: item.quantidadeMenus,
        url: item.url,
        menus: []
      }
      if (item.menus && item.menus.length > 0) {
        item.menus.forEach(itemMenu => {
          let menu = {
            codigo: itemMenu.codigo,
            descricao: itemMenu.descricao,
            url: itemMenu.url,
            subMenus: []
          }
          if (subMenu.menus) {
            subMenu.menus.push(menu);
            if (itemMenu.url) {
              setPermissao(itemMenu, permissoes);
            }
          }
          if (itemMenu.subMenus && itemMenu.subMenus.length > 0) {
            itemMenu.subMenus.forEach(subItem => {
              menu.subMenus.push({
                codigo: subItem.codigo,
                descricao: subItem.descricao,
                url: subItem.url,
                subMenus: []
              });
              setPermissao(subItem, permissoes);
            })
          }
        })
      }
      menus.push(subMenu)
    })
    store.dispatch(setMenu(menus));
    store.dispatch(setPermissoes(permissoes));
  });

  const setPermissao = (item, permissoes) => {
    permissoes[item.url] =
      {
        podeAlterar: item.podeAlterar,
        podeConsultar: item.podeConsultar,
        podeExcluir: item.podeExcluir,
        podeIncluir: item.podeIncluir
      }
  }
}

const getObjetoStorageUsuario = objeto => {
  const persistSmeSgp = localStorage.getItem('persist:sme-sgp')
  const usuario = persistSmeSgp && (persistSmeSgp.includes('usuario')) ? JSON.parse(persistSmeSgp).usuario : null;
  const resultado = usuario ? JSON.parse(usuario)[objeto] : null;
  return resultado;
}

const verificaSomenteConsulta = (permissoes, naoSetarResultadoNoStore) => {
  if (permissoes && permissoes[tipoPermissao.podeConsultar] && !permissoes[tipoPermissao.podeAlterar]
    && !permissoes[tipoPermissao.podeIncluir] && !permissoes[tipoPermissao.podeExcluir]) {
    if (naoSetarResultadoNoStore) {
      store.dispatch(setSomenteConsulta(false));
    } else {
      store.dispatch(setSomenteConsulta(true));
    }
    return true;
  }
  store.dispatch(setSomenteConsulta(false));
  return false;
}

export { setMenusPermissoes, getObjetoStorageUsuario, verificaSomenteConsulta };

