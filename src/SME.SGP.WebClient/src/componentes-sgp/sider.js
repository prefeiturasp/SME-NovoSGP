import React, { useState, useEffect } from 'react';
import { Menu, Layout, Tooltip } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import {
  MenuBody,
  DivFooter,
  MenuScope,
  Topo,
  IconeRetrair,
} from './sider.css';
import LogoMenuFooter from '../recursos/LogoMenuFooter.svg';
import { store } from '../redux';
import {
  menuRetraido,
  menuSelecionado,
} from '../redux/modulos/navegacao/actions';
import { obterDescricaoNomeMenu } from '~/servicos/servico-navegacao';

const Sider = () => {
  const { Sider, Footer } = Layout;
  const { SubMenu } = Menu;
  const NavegacaoStore = useSelector(state => state.navegacao);
  const [openKeys, setOpenKeys] = useState([]);

  const usuario = useSelector(state => state.usuario);

  const modalidadesFiltroPrincipal = useSelector(
    state => state.filtro.modalidades
  );

  const [subMenusPrincipais, setSubMenusPrincipais] = useState([]);

  useEffect(() => {
    const verificaSelecaoMenu = rotaAtiva => {
      const rota = NavegacaoStore.rotas.get(rotaAtiva);
      setOpenKeys([]);
      if (rota && rota.limpaSelecaoMenu) {
        store.dispatch(menuSelecionado([]));
      }
    };
    verificaSelecaoMenu(NavegacaoStore.rotaAtiva);
  }, [NavegacaoStore.rotaAtiva, NavegacaoStore.rotas]);

  useEffect(() => {
    if (usuario.menu)
      setSubMenusPrincipais(
        usuario.menu
          .filter(menu => {
            if (menu.ehMenu) return menu;
          })
          .map(x => 'menu-' + x.codigo)
      );
  }, [usuario.menu]);

  const alterarPosicaoJanelaPopup = (idElementoHtml, quantidadeItens) => {
    const itemMenu = window.document.getElementById(idElementoHtml);
    if (itemMenu) {
      const alturaItens = quantidadeItens * 40 + 6;
      const alturaTela = window.innerHeight;
      const posicaoY = itemMenu.getBoundingClientRect().y;
      const posicaoRight = itemMenu.getBoundingClientRect().right;
      const alturaTotalItens = posicaoY + alturaItens;
      const posicaoTop =
        alturaTotalItens > alturaTela
          ? posicaoY - (alturaTotalItens - alturaTela)
          : posicaoY;
      document.documentElement.style.setProperty(
        '--posicao-item-menu-top',
        `${posicaoTop}px`
      );
      document.documentElement.style.setProperty(
        '--posicao-item-menu-right',
        `${posicaoRight}px`
      );
    }
  };

  const alternarRetraido = () => {
    setOpenKeys([]);
    store.dispatch(menuRetraido(!NavegacaoStore.retraido));
    document.documentElement.style.setProperty(
      '--espacamento-conteudo',
      `${NavegacaoStore.retraido ? 250 : 115}px`
    );
  };

  const onOpenChange = openKeys => {
    const latestOpenKey = openKeys[openKeys.length - 1];
    if (subMenusPrincipais.indexOf(latestOpenKey) === -1) {
      setOpenKeys(openKeys);
    } else {
      setOpenKeys(latestOpenKey ? [latestOpenKey] : []);
    }
  };

  const selecionarItem = item => {
    store.dispatch(menuSelecionado([item.key]));
  };

  const criarItensMenu = menus => {
    const itens = menus.map(item => {
      return item.subMenus && item.subMenus.length > 0 ? (
        criarMenus([item])
      ) : (
        <Menu.Item key={item.codigo} id={item.codigo}>
          <span className="menuItem">
            {obterDescricaoNomeMenu(
              item.url,
              modalidadesFiltroPrincipal,
              usuario.turmaSelecionada,
              item.descricao
            )}
          </span>
          {item.url ? <Link to={item.url} id={'link-' + item.codigo} /> : ''}
        </Menu.Item>
      );
    });
    return itens;
  };

  const criarMenus = menu => {
    if (menu && menu.length > 0) {
      return menu.map(subMenu => {
        const temSubmenu = subMenu.subMenus && subMenu.subMenus.length > 0;
        if (subMenu.ehMenu || temSubmenu) {
          const menuKey = (temSubmenu ? 'sub-' : 'menu-') + subMenu.codigo;
          return (
            <SubMenu
              id={subMenu.codigo}
              key={menuKey}
              onMouseEnter={e =>
                alterarPosicaoJanelaPopup(
                  subMenu.codigo,
                  subMenu.quantidadeMenus
                )
              }
              title={
                subMenu.icone ? (
                  <div className={'item-menu-retraido'}>
                    <i
                      className={
                        subMenu.icone +
                        (NavegacaoStore.retraido ? ' icons-retraido' : ' icons')
                      }
                    />
                    <span>{subMenu.descricao}</span>
                  </div>
                ) : (
                  <div
                    className={
                      'item-menu-retraido' + temSubmenu
                        ? ' submenu-subnivel'
                        : ''
                    }
                  >
                    <span>{subMenu.descricao}</span>
                  </div>
                )
              }
            >
              {criarItensMenu(subMenu.menus ? subMenu.menus : subMenu.subMenus)}
            </SubMenu>
          );
        }
      });
    }
  };

  return (
    <MenuBody id="main" retraido={NavegacaoStore.retraido}>
      <Sider
        style={{ background: Base.Roxo, height: '100%' }}
        collapsed={NavegacaoStore.retraido}
        onCollapse={NavegacaoStore.retraido}
        width="250px"
        collapsedWidth="115px"
        breakpoint="lg"
        onBreakpoint={breakpoint => store.dispatch(menuRetraido(breakpoint))}
      >
        <Topo>
          <div className="conteudo">
            <IconeRetrair className="arrow" onClick={alternarRetraido}>
              <i
                style={{ color: Base.Branco }}
                className={
                  NavegacaoStore.retraido
                    ? 'fas fa-chevron-circle-right'
                    : 'fas fa-chevron-circle-left'
                }
              />
            </IconeRetrair>
          </div>
          <div
            className={NavegacaoStore.retraido ? 'perfil-retraido' : 'perfil'}
          >
            <div className="circulo-perfil">
              <i className="fas fa-user-circle icone-perfil" />
              {/* <img
                id="imagem-perfil"
                src={usuario.meusDados.foto}
              /> */}
            </div>
            <div hidden={NavegacaoStore.retraido}>
              <Tooltip
                title={usuario.meusDados.nome}
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <span id="nome" className="nome">
                  {usuario.meusDados.nome}
                </span>
              </Tooltip>
            </div>
            <div
              className="perfil-edit"
              style={{ paddingTop: NavegacaoStore.retraido ? '0' : '12px' }}
            >
              <Link id="perfil-edit" to="/meus-dados">
                <i className="fas fa-user-edit" />
                <span>Meus Dados</span>
              </Link>
            </div>
          </div>
        </Topo>
        <MenuScope>
          <div
            className={`menu-scope${
              NavegacaoStore.retraido ? ' menu-scope-retraido' : ''
            }`}
          >
            <Menu
              id="menuPrincipal"
              mode="inline"
              theme="dark"
              openKeys={openKeys}
              onOpenChange={onOpenChange}
              onSelect={selecionarItem.bind(NavegacaoStore.menuSelecionado)}
              selectedKeys={NavegacaoStore.menuSelecionado}
            >
              {criarMenus(usuario.menu)}
            </Menu>
          </div>
        </MenuScope>
        <div className="footer-content">
          <DivFooter>
            <Footer>
              <div className="logo-secretaria" hidden>
                <img alt="Logotipo SME" src={LogoMenuFooter} />
              </div>
              <div className="descricao">
                <span>SME-SP-SGP - Distribuído sob a Licença AGPL V3</span>
              </div>
            </Footer>
          </DivFooter>
        </div>
      </Sider>
    </MenuBody>
  );
};

export default Sider;
