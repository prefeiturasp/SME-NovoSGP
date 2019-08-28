import React, { useState } from 'react';
import { Menu, Icon, Layout } from 'antd';
import Button from '../componentes/button';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { Base } from '../componentes/colors';

const Sider = () => {
  const { Sider } = Layout;
  const MenuScope = styled.div`
  i{
    font-style: normal !important;
  }
  .ant-menu, ant-menu-sub, ant-menu-inline, ant-menu-item{
    background: ${Base.Roxo} !important;
    width: 200px !important;
  }

  .ant-menu-submenu-title:hover, .ant-menu-inline.ant-menu-sub, .ant-menu-submenu-open,
  .ant-menu-item-selected{
    background: ${Base.Branco} !important;
    color: ${Base.Roxo} !important;
  }

  .ant-menu-item {
    color: ${Base.CinzaMenuItem} !important;
  }

  .ant-menu-inline .ant-menu-item:not(:last-child){
    margin-bottom: 0px !important;
    border-bottom: 0.75px solid ${Base.Roxo} !important;
  }

  .ant-menu-dark, .ant-menu-inline.ant-menu-sub {
    box-shadow: none !important;
  }

  .ant-menu-submenu-title:hover, .ant-menu-inline.ant-menu-sub, .ant-menu-submenu-open{
    border-radius:4px;
  }

  .ant-menu-item-selected{
    background: ${Base.CinzaMenu} !important;
    border-bottom: none;
    border-left: solid ${Base.RoxoClaro} 8px;
    color: ${Base.CinzaMenuItem} !important;
  }

  `;
  const { SubMenu } = Menu;
  const [collapsed, setCollapsed] = useState(false);
  const [openKeys, setOpenKeys] = useState([]);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  const onOpenChange = openKeys => {
    if (openKeys.length > 0) {
      const latestOpenKey = openKeys[openKeys.length - 1];
      setOpenKeys([latestOpenKey]);
    } else {
      setOpenKeys([]);
    }
  };

  return (
    <div>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider style={{ background: Base.Roxo, flex:[0,0,220] }} collapsed={collapsed} onCollapse={collapsed}
          width="220px">
          <Button
            icon={collapsed ? 'fas fa-chevron-circle-right' : 'fas fa-chevron-circle-left'}
            onClick={toggleCollapsed}
          />

          <MenuScope>
            <Menu
              mode="inline"
              theme="dark"
              openKeys={openKeys}
              onOpenChange={onOpenChange}
              inlineCollapsed={collapsed}
            >
              <SubMenu
                if="diarioClasses"
                key="subDiarioClasses"
                title={
                  <span>
                    <Icon type="unordered-list" />
                    <span>Diário de classes</span>
                  </span>
                }>
                <Menu.Item key="1">
                  <span> Plano de aula/Frequência</span>
                </Menu.Item>
                <Menu.Item key="2">
                  <span>Notas</span>
                </Menu.Item>
                <Menu.Item key="3">
                  <span>Boletim</span>
                </Menu.Item>
                <Menu.Item key="4">
                  <span>Aula Prevista X Aula Dada</span>
                </Menu.Item>
                <Menu.Item key="5">
                  <span>Compensação de Ausência</span>
                </Menu.Item>
                <Menu.Item key="6">
                  <span>Cadastro AEE</span>
                </Menu.Item>
                <Menu.Item key="7">
                  <span>Justificativa de Faltas</span>
                </Menu.Item>
                <Menu.Item key="8">
                  <span>Registro HA/HI/CCH/CJ</span>
                </Menu.Item>
                <Menu.Item key="9">
                  <span>Sondagem</span>
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="planejamento"
                key="subPlanejamento"
                title={
                  <span>
                    <Icon type="unordered-list" />
                    <span>Planejamento</span>
                  </span>
                }>
                <Menu.Item key="30">
                  <span> Plano de Ciclo</span>
                  <Link to="/planejamento/plano-ciclo/2019/1" className="nav-link text-white" />
                </Menu.Item>
                <Menu.Item key="31">
                  <span>Plano anual</span>
                  <Link to="/planejamento/plano-anual" className="nav-link text-white" />
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="fechamento"
                key="subFechamento"
                title={
                  <span>
                    <Icon type="unordered-list" />
                    <span>Fechamento</span>
                  </span>
                }>
                <Menu.Item key="50">
                  <span> Conselho de Classe</span>
                </Menu.Item>
                <Menu.Item key="51">
                  <span>Nota do Bimestre</span>
                </Menu.Item>
                <Menu.Item key="52">
                  <span>Parecer Conclusivo</span>
                </Menu.Item>
              </SubMenu>
            </Menu>
          </MenuScope>
        </Sider>
      </Layout>
    </div>
  );
}

export default Sider;
