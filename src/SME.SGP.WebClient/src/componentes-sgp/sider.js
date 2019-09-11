import React, { useState } from 'react';
import { Menu, Layout } from 'antd';
import { Link } from 'react-router-dom';
import { Base } from '../componentes/colors';
import { MenuBody, DivFooter, MenuScope, Topo } from './sider.css';
import LogoMenuFooter from '../recursos/LogoMenuFooter.svg';
import { store } from '../redux';
import { menuCollapsed } from '../redux/modulos/navegacao/actions'

const Sider = () => {
  const { Sider, Footer } = Layout;
  const { SubMenu } = Menu;
  const [collapsed, setCollapsed] = useState(false);
  const [openKeys, setOpenKeys] = useState([]);

  const toggleCollapsed = () => {
    setOpenKeys([]);
    setCollapsed(!collapsed);
    store.dispatch(menuCollapsed(!collapsed));
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
    <MenuBody id="main" style={{width: collapsed?'115px': '250px'}}>
      <Sider
        style={{ background: Base.Roxo, height: '100%' }} collapsed={collapsed} onCollapse={collapsed}
        width="250px" collapsedWidth="115px">
        <Topo>
          <div className="conteudo">
            <a className="arrow" onClick={toggleCollapsed}>
              <i
                style={{ color: Base.Branco }}
                className={
                  collapsed
                    ? 'fas fa-chevron-circle-right'
                    : 'fas fa-chevron-circle-left'
                }
              />
            </a>
          </div>
          <div className={collapsed ? 'perfil-retraido' : 'perfil'}>
            <div className="circulo-perfil">
              <img
                id="imagem-perfil"
                src="https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg"
              />
            </div>
            <div hidden={collapsed}>
              <span id="nome" className="nome">
                Nome + Sobrenome
              </span>
            </div>
            <div
              className="perfil-edit"
              style={{ paddingTop: collapsed ? '0' : '12px' }}
            >
              <a id="perfil-edit">
                <i className="fas fa-user-edit" />
                <span>Perfil</span>
              </a>
            </div>
          </div>
        </Topo>

        <MenuScope>
          <div
            className={`menu-scope${collapsed ? ' menu-scope-retraido' : ''}`}
          >
            <Menu
              id="menuPrincipal"
              mode="inline"
              theme="dark"
              openKeys={openKeys}
              onOpenChange={onOpenChange}
            >
              <SubMenu
                id="diarioClasse"
                key="subDiarioClasse"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-book-reader ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Diário de Classe</span>
                  </div>
                }
              >
                <Menu.Item key="1" id="diaPlanoAulaFreq">
                  <span className="menuItem"> Plano de aula/Frequência</span>
                </Menu.Item>
                <Menu.Item key="2" id="diaNotas">
                  <span className="menuItem"> Notas</span>
                </Menu.Item>
                <Menu.Item key="3" id="diaBoletim">
                  <span className="menuItem"> Boletim</span>
                </Menu.Item>
                <Menu.Item key="4" id="diaAulaPrevistaDada">
                  <span className="menuItem"> Aula Prevista X Aula Dada</span>
                </Menu.Item>
                <Menu.Item key="5" id="diaCompensacaoAusencia">
                  <span className="menuItem"> Compensação de Ausência</span>
                </Menu.Item>
                <Menu.Item key="6" id="diaCadastroAee">
                  <span className="menuItem"> Cadastro AEE</span>
                </Menu.Item>
                <Menu.Item key="7" id="diaJustificativaFaltas">
                  <span className="menuItem"> Justificativa de Faltas</span>
                </Menu.Item>
                <Menu.Item key="8" id="diaRegistro">
                  <span className="menuItem"> Registro HA/HI/CCH/CJ</span>
                </Menu.Item>
                <Menu.Item key="9" id="diaSondagem">
                  <span className="menuItem"> Sondagem</span>
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="planejamento"
                key="subPlanejamento"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-list-alt ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Planejamento</span>
                  </div>
                }
              >
                <Menu.Item key="30" id="plaPlanoCiclo" htmlFor="linkPlanoCiclo">
                  <span className="menuItem"> Plano de Ciclo</span>
                  <Link
                    to="/planejamento/plano-ciclo"
                    className="nav-link text-white"
                    id="linkPlanoCiclo"
                  />
                </Menu.Item>
                <Menu.Item key="31" id="plaPlanoAnual" htmlFor="linkPlanoAnual">
                  <span className="menuItem"> Plano Anual</span>
                  <Link
                    to="/planejamento/plano-anual"
                    className="nav-link text-white"
                    id="linkPlanoAnual"
                  />
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="fechamento"
                key="subFechamento"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-pencil-ruler ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Fechamento</span>
                  </div>
                }
              >
                <Menu.Item key="50" id="fecConselhoClasse">
                  <span className="menuItem"> Conselho de Classe</span>
                </Menu.Item>
                <Menu.Item key="51" id="fecNotaBimestre">
                  <span className="menuItem"> Nota do Bimestre</span>
                </Menu.Item>
                <Menu.Item key="52" id="fecParecerConclusivo">
                  <span className="menuItem"> Parecer Conclusivo</span>
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="relatorios"
                key="subRelatorios"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-file-alt ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Relatórios</span>
                  </div>
                }
              >
                <Menu.Item key="70" id="relFrequencia">
                  <span className="menuItem">Frequência</span>
                </Menu.Item>
                <Menu.Item key="71" id="relPendencias">
                  <span className="menuItem">Pendências</span>
                </Menu.Item>
                <Menu.Item key="72" id="relComponenteFinalizado">
                  <span className="menuItem">Componente Finalizado</span>
                </Menu.Item>
                <Menu.Item key="73" id="relContraturno">
                  <span className="menuItem">Contraturo</span>
                </Menu.Item>
                <Menu.Item key="74" id="relHistoricoEscolar">
                  <span className="menuItem">Histórico Escolar</span>
                </Menu.Item>
                <Menu.Item key="75" id="relAee">
                  <span className="menuItem">Relatório AEE</span>
                </Menu.Item>
                <Menu.Item key="76" id="relRecuperacaoParalela">
                  <span className="menuItem">Recuperação Paralela</span>
                </Menu.Item>
                <Menu.Item key="77" id="relSondagem">
                  <span className="menuItem">Relatório de Sondagem</span>
                </Menu.Item>
              </SubMenu>
              <SubMenu
                id="gestao"
                key="subGestao"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-user-cog ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Gestão</span>
                  </div>
                }>
                <Menu.Item key="90" id="gesCalendarioEscolar" className="popup-gestao">
                  <span className="menuItem">Calendário Escolar</span>
                </Menu.Item>
                <Menu.Item key="91" id="gesAtribuicaoCj">
                  <span className="menuItem">Atribuição CJ</span>
                </Menu.Item>
                <Menu.Item key="92" id="gesAtribuicaoDiretor">
                  <span className="menuItem">Atribuição Diretor</span>
                </Menu.Item>
                <Menu.Item key="93" id="gesControleGrade">
                  <span className="menuItem">Controle de Grade</span>
                </Menu.Item>
                <Menu.Item key="94" id="gesAtribuicaoSupervisor">
                  <span className="menuItem">Atribuição Supervisor</span>
                  <Link to="/gestao/atribuicao-supervisor-lista" className="nav-link text-white" id="linkAtribuicaoSupervisor"/>
                </Menu.Item>
              </SubMenu>
              <SubMenu
                disabled
                id="configuracoes"
                key="subConfiguracoes"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-cog ${
                        collapsed ? 'icons-retraido' : 'icons'
                      }`}
                    />
                    <span>Configurações</span>
                  </div>
                }
              />
            </Menu>
          </div>
        </MenuScope>
        <div className="footer-content">
          <DivFooter>
            <Footer>
              <div className="logo-secretaria" hidden>
                <img src={LogoMenuFooter} />
              </div>
              <div className="descricao">
                <span>SME-SP-SGA - Distribuído sob a Licença AGPL V3</span>
              </div>
            </Footer>
          </DivFooter>
        </div>
      </Sider>
    </MenuBody>
  );
};

export default Sider;
