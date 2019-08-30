import { Base } from '../componentes/colors';
import styled from 'styled-components';

export const MenuBody = styled.div`
position: fixed;
`;

export const DivFooter = styled.div`
display: flex;
justify-content: center;
flex-direction: column;
align-items: center;
color: ${Base.Branco};
background: ${Base.Roxo};
text-align: center;
width: 214px;
vertical-align: bottom;
position: absolute;
bottom: 0;
margin-bottom: 40px;

.ant-layout-footer{
  color: ${Base.Branco};
  background: ${Base.Roxo};
}
.descricao{
  font-size:9px;
}
.logo-secretaria{
  width: 140px;
  padding-bottom: 10px;
}
`;

export const Topo = styled.div`
  width:100%;

  .conteudo{
    height: 20px;
  }
  .arrow{
    float: right;
  }

  .arrow i{
    margin: 8px 12px 0px 0px;
  }

  .perfil{
    display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    height: 150px;
    color: ${Base.Branco};
    width: 100%;
  }

  .circulo-perfil img {
    border: 2px solid ${Base.Branco};
    border-radius: 50%;
    bottom: 0;
    width: 100%;
    width: 60px;
    height: 60px;
    margin-bottom: 10px;
  }

  .nome{
    width: 100%;
    border-radius: 15px;
    border: 1px solid ${Base.Branco};
    color: white;
    padding: 5px 15px;
    font-size: 12px;
  }

  .perfil-edit{
    font-size: 10px !important;
  }

  .perfil-edit i{
    margin-right: 4px;
  }
`;

export const MenuScope = styled.div`
  .icons{
    font-style: normal !important;
    margin-right: 12px;
    height: 18px;
    width: 13.5px;
  }

  .ant-menu-submenu-title{
    padding-left: 15px !important;
  }

  .ant-menu{
    background: ${Base.Roxo} !important;
    width: 220px !important;
  }

  .ant-menu-inline, .ant-menu-submenu-title, .ant-menu-item{
    margin-bottom: 0px !important;
    margin-top: 0px !important;
  }

  .ant-menu-submenu-title, .ant-menu-item{
    height: 35px !important;
  }

  .ant-menu-item {
    color: ${Base.CinzaMenuItem} !important;
  }

  .ant-menu-sub{
    box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
    -webkit-box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
    -moz-box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
  }

  .ant-menu-submenu-title:hover, .ant-menu-inline.ant-menu-sub, .ant-menu-submenu-open{
    border-radius:5px;
  }

  .ant-menu-submenu-title{
    font-size: 14px !important;
    font-weight: bold;
  }

  .ant-menu-inline .ant-menu-item:not(:first-child){
    margin-bottom: 0px !important;
    border-top: 1px solid ${Base.RoxoClaro} ;
  }

  .ant-menu-item:last-child{
    border-bottom-right-radius:5px !important;
    border-bottom-left-radius:5px !important;
  }


  .ant-menu-item {
    padding-left: 34px !important;
    font-size: 12px !important;
    border-left: solid transparent 8px;
  }

  .ant-menu-dark, .ant-menu-submenu-arrow{
    color: ${Base.Branco} !important;
    opacity: initial !important;
  }

 .ant-menu-submenu-title:hover .ant-menu-submenu-arrow::before {
    background: ${Base.Roxo} !important;
  }

  .ant-menu-submenu-title:hover  .ant-menu-submenu-arrow::after {
    background: ${Base.Roxo} !important;
  }

  .ant-menu-submenu-open > div > i::before {
    background: ${Base.Roxo} !important;
  }
  .ant-menu-submenu-open > div > i::after {
    background: ${Base.Roxo} !important;
  }

  .menuItem{
    color: ${Base.CinzaMenuItem} !important;
  }

  .ant-menu-submenu-title{
    margin-top:3px !important;
  }

  .menu-scope{
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center
  }

  .ant-menu-submenu-disabled{
    color: blue !important;
    background: ${Base.Roxo} !important;
  }

  .ant-menu-submenu-title:hover, .ant-menu-inline.ant-menu-sub, .ant-menu-submenu-open{
    background: ${Base.Branco} !important;
    color: ${Base.Roxo} !important;
  }

  .ant-menu-item-selected{
    background: ${Base.CinzaMenu} !important;
    border-left: solid ${Base.RoxoClaro} 8px !important;
    border-bottom-width: 8px;
  }
  `;


