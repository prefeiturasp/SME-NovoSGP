import tipoPermissao from '~/dtos/tipoPermissao';
import { store } from '~/redux';
import { setSomenteConsulta } from '~/redux/modulos/navegacao/actions';
import { setMenu, setPermissoes } from '~/redux/modulos/usuario/actions';
import api from '~/servicos/api';
import RotasDto from '~/dtos/rotasDto';
import modalidade from '~/dtos/modalidade';
import { obterModalidadeFiltroPrincipal } from './Validacoes/validacoesInfatil';
import { FiltroHelper } from '~/componentes-sgp';

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
        menus: [],
      };
      if (item.menus && item.menus.length > 0) {
        item.menus.forEach(itemMenu => {
          let menu = {
            codigo: itemMenu.codigo,
            descricao: itemMenu.descricao,
            url: itemMenu.url,
            subMenus: [],
          };
          if (subMenu.menus) {
            subMenu.menus.push(menu);
            if (itemMenu.url) {
              setPermissao(itemMenu, permissoes);
            }
          }
          if (itemMenu.subMenus && itemMenu.subMenus.length > 0) {
            const subMenusOrdenados = itemMenu.subMenus.sort(
              FiltroHelper.ordenarLista('ordem')
            );
            subMenusOrdenados.forEach(subItem => {
              menu.subMenus.push({
                codigo: subItem.codigo,
                descricao: subItem.descricao,
                url: subItem.url,
                subMenus: [],
              });
              setPermissao(subItem, permissoes);
            });
          }
        });
      }
      menus.push(subMenu);
    });
    store.dispatch(setMenu(menus));
    store.dispatch(setPermissoes(permissoes));
  });

  const setPermissao = (item, permissoes) => {
    permissoes[item.url] = {
      podeAlterar: item.podeAlterar,
      podeConsultar: item.podeConsultar,
      podeExcluir: item.podeExcluir,
      podeIncluir: item.podeIncluir,
    };
  };
};

const getObjetoStorageUsuario = objeto => {
  const persistSmeSgp = localStorage.getItem('persist:sme-sgp');
  const usuario =
    persistSmeSgp && persistSmeSgp.includes('usuario')
      ? JSON.parse(persistSmeSgp).usuario
      : null;
  const resultado = usuario ? JSON.parse(usuario)[objeto] : null;
  return resultado;
};

const verificaSomenteConsulta = (permissoes, naoSetarResultadoNoStore) => {
  if (
    permissoes &&
    permissoes[tipoPermissao.podeConsultar] &&
    !permissoes[tipoPermissao.podeAlterar] &&
    !permissoes[tipoPermissao.podeIncluir] &&
    !permissoes[tipoPermissao.podeExcluir]
  ) {
    if (naoSetarResultadoNoStore) {
      store.dispatch(setSomenteConsulta(false));
    } else {
      store.dispatch(setSomenteConsulta(true));
    }
    return true;
  }
  store.dispatch(setSomenteConsulta(false));
  return false;
};

const setSomenteConsultaManual = valor => {
  store.dispatch(setSomenteConsulta(valor));
}

const obterDescricaoNomeMenu = (
  url,
  modalidadesFiltroPrincipal,
  turmaSelecionada,
  descricao
) => {
  const urls = {
    [RotasDto.FREQUENCIA_PLANO_AULA]: {
      [String(modalidade.INFANTIL)]: 'Frequência',
      [String(modalidade.EJA)]: 'Frequência/Plano Aula',
      [String(modalidade.FUNDAMENTAL)]: 'Frequência/Plano Aula',
      [String(modalidade.ENSINO_MEDIO)]: 'Frequência/Plano Aula',
    },
    [RotasDto.PLANO_ANUAL]: {
      [String(modalidade.INFANTIL)]: 'Plano Anual',
      [String(modalidade.EJA)]: 'Plano Semestral',
      [String(modalidade.FUNDAMENTAL)]: 'Plano Anual',
      [String(modalidade.ENSINO_MEDIO)]: 'Plano Anual',
    },
    [RotasDto.PLANO_CICLO]: {
      [String(modalidade.INFANTIL)]: 'Plano de Ciclo',
      [String(modalidade.EJA)]: 'Plano de Etapa',
      [String(modalidade.FUNDAMENTAL)]: 'Plano de Ciclo',
      [String(modalidade.ENSINO_MEDIO)]: 'Plano de Ciclo',
    },
  };
  const rota = urls[url];
  if (rota) {
    const modalidadeAtual = obterModalidadeFiltroPrincipal(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );

    return rota[modalidadeAtual];
  }

  return descricao;
};

export {
  setMenusPermissoes,
  getObjetoStorageUsuario,
  verificaSomenteConsulta,
  obterDescricaoNomeMenu,
  setSomenteConsultaManual,
};
