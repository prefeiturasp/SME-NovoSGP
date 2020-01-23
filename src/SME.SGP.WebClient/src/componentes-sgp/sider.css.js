import styled from 'styled-components';
import { Base } from '../componentes/colors';

export const MenuBody = styled.div`
  background: ${Base.Roxo};
  padding: 0 !important;
  position: fixed;
  left: 0%;
  height: 100%;
  width: ${props => (props.retraido ? `115px` : `250px`)};

  @media screen and (max-width: 993px) {
    width: 115px !important;
  }

  .footer-content {
    display: flex;
    justify-content: center;
  }
`;

export const DivFooter = styled.div`
  bottom: 0;
  position: fixed;
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;

  .ant-layout-footer {
    align-items: center;
    color: ${Base.Branco};
    background: ${Base.Roxo};
    text-align: center;
    padding: 0;
    bottom: 0;
    margin-bottom: 2px;
  }
  .descricao {
    width: 110px;
    font-size: 9px;
    word-wrap: break-word;
  }

  @media (max-width: 790px) {
    .descricao {
      width: 93px;
      font-size: 8px;
    }
  }
  .logo-secretaria {
    width: 100%;
    padding-bottom: 10px;
  }
`;

export const Topo = styled.div`
  width: 100%;

  .conteudo {
    height: 20px;
  }
  .arrow {
    float: right;
  }

  .arrow i {
    margin: 8px 12px 0px 0px;
  }

  .perfil {
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    height: 150px;
    color: ${Base.Branco};
    width: 100%;
    a,
    a:hover {
      color: ${Base.Branco} !important;
    }
  }

  .perfil-retraido {
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    height: 100px;
    color: ${Base.Branco};
    width: 100%;
    margin-bottom: 50px;
  }

  @media (max-height: 650px) {
    .perfil-retraido {
      margin-bottom: 0;
    }
  }

  .circulo-perfil img {
    border: 2px solid ${Base.Branco};
    border-radius: 50%;
    bottom: 0;
    width: 100%;
    width: 60px;
    height: 60px;
    margin-bottom: 10px;
    background: ${Base.Branco};
  }

  .nome {
    width: 200px;
    border-radius: 15px;
    border: 1px solid ${Base.Branco};
    color: white;
    padding: 5px 15px;
    font-size: 12px;
    display: inline-block;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    direction: ltr;
    text-align: center;
  }

  .perfil-edit {
    font-size: 10px !important;
  }

  .perfil-edit #perfil-edit {
    color: ${Base.Branco};
  }

  .perfil-edit i {
    margin-right: 4px;
  }

  .icone-perfil {
    color: ${Base.CinzaBotao};
    background: ${Base.Branco};
    font-size: 60px;
    border-radius: 50%;
    padding: 1px 1px 0 1px;
    margin-bottom: 10px;
  }
`;

export const MenuScope = styled.div`
  /*----MENU EXPANDIDO----*/
  position: absolute;
  width: 100%;
  overflow-y: auto;
  height: calc(100vh - 270px);

  ::-webkit-scrollbar {
    width: 10px;
    background: ${Base.RoxoFundo};
    border-radius: 4px;
  }

  ::-webkit-scrollbar-thumb {
    background: #dad7d7;
    border-radius: 4px !important;
    border: solid 2px ${Base.RoxoFundo};
    background: ${Base.RoxoClaro};
  }

  .menu-scope {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    margin: 0 10px 0 10px;
  }

  .menu-scope-retraido {
    margin: 0 !important;
  }

  .icons {
    font-style: normal !important;
    margin-right: 12px;
    height: 18px;
    width: 13.5px;
  }

  .ant-menu {
    background: ${Base.Roxo};
    width: 100%;
  }

  .ant-menu-submenu-title,
  .ant-menu-item {
    height: 35px !important;
  }

  .ant-menu-item,
  .ant-menu-submenu-open {
    background: ${Base.Branco} !important;
  }

  .ant-menu-submenu-title:first-child,
  .ant-menu-submenu-open {
    border-radius: 4px;
  }

  .ant-menu-submenu-title {
    margin-top: 2px !important;
    padding-left: 15px !important;
    font-size: 14px !important;
    font-weight: bold;
  }

  .ant-menu-item:last-child,
  .ant-menu-inline.ant-menu-sub:last-child {
    border-bottom-right-radius: 4px !important;
    border-bottom-left-radius: 4px !important;
  }

  .ant-menu-item {
    padding-left: 40px !important;
    font-size: 12px !important;
    height: auto !important;
    line-height: normal !important;
    padding-top: 10px !important;
    padding-bottom: 10px !important;
  }

  .ant-menu-item-selected {
    background: ${Base.CinzaMenu} !important;
    border-bottom-width: 8px;
    padding-left: 32px !important;
    border-left: solid ${Base.RoxoClaro} 8px !important;
  }

  .ant-menu-dark:not(:disabled),
  .ant-menu-submenu-arrow {
    color: ${Base.Branco};
    opacity: initial !important;
  }

  .ant-menu-submenu-title:hover .ant-menu-submenu-arrow::before {
    background: ${Base.Roxo} !important;
  }

  .ant-menu-submenu-title:hover .ant-menu-submenu-arrow::after {
    color: ${Base.Branco} !important;
    background: ${Base.Roxo} !important;
  }

  .ant-menu-submenu-open > div > i::before {
    background: ${Base.Roxo} !important;
  }
  .ant-menu-submenu-open > div > i::after {
    background: ${Base.Roxo} !important;
  }

  .menuItem {
    color: ${Base.CinzaMenuItem} !important;
    white-space: normal;
  }

  .ant-menu-submenu-title:hover:not(:disabled),
  .ant-menu-inline.ant-menu-sub,
  .ant-menu-submenu-open {
    background: ${Base.Branco};
    color: ${Base.Roxo};
  }

  .ant-menu-dark > .ant-menu-submenu-disabled > .ant-menu-submenu-title {
    opacity: initial;
    background: ${Base.Roxo};
    color: ${Base.CinzaDesabilitado};
  }

  .ant-menu-dark > .ant-menu-submenu-open {
    color: ${Base.Roxo};
  }

  .ant-menu-inline.ant-menu-sub {
    background: ${Base.Branco} !important;
  }

  .menu-scope {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }

  .submenu-subnivel {
    padding-left: 25px !important;
    color: ${Base.CinzaMenuItem} !important;
    font-size: 14px !important;

    &:hover {
      color: ${Base.Roxo} !important;
    }
  }

  /*----MENU RETRAÍDO----*/
  .ant-menu-vertical {
    width: 100%;
  }

  .ant-menu:not(.ant-menu-inline) .ant-menu-submenu-open {
    color: ${Base.Roxo};
  }

  .ant-menu-vertical > .ant-menu-submenu {
    border-radius: 0 !important;
    .ant-menu-submenu-title {
      border-radius: 0 !important;
      height: 60px !important;
      .item-menu-retraido {
        margin-top: 5px;
        display: flex;
        justify-content: center;
        flex-direction: column;
        align-items: center;
        font-weight: normal;
        font-size: 12px;
      }
    }
  }

  .icons-retraido {
    margin-top: 0px !important;
    font-size: 25px;
  }
`;

export const IconeRetrair = styled.a`
  @media screen and (max-width: 993px) {
    visibility: hidden;
  }
`;
