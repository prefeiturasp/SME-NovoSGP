import React, { useState } from 'react';
import { Menu, Layout } from 'antd';
import { Link } from 'react-router-dom';
import { Base } from '../componentes/colors';
import { MenuBody, DivFooter, MenuScope, Topo } from './sider.css'
import LogoMenuFooter from '../recursos/LogoMenuFooter.svg';

const Sider = () => {

  const { Sider, Footer } = Layout;
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
    <MenuBody>
      <Layout style={{ minHeight: '100vh' }}>
        <Sider style={{ background: Base.Roxo, flex: [0, 0, 220] }} collapsed={collapsed} onCollapse={collapsed}
          width="230px">
          <Topo>
            <div className="conteudo">
              <a className="arrow" onClick={toggleCollapsed}>
                <i style={{ color: Base.Branco }} className={collapsed ? 'fas fa-chevron-circle-right' : 'fas fa-chevron-circle-left'} />
              </a>
            </div>
            <div className="perfil">
              <div className="circulo-perfil">
                <img id="imagem-perfil" src="https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg"></img>
              </div>
              <div>
                <span id="nome" className="nome">Nome + Sobrenome</span>
              </div>
              <div className="perfil-edit" style={{ paddingTop: '12px' }}>
                <a id="perfil-edit">
                  <i className="fas fa-user-edit"></i>
                  <span>Perfil</span>
                </a>
              </div>
            </div>
          </Topo>

          <MenuScope>
            <div className="menu-scope">
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
                    <span>
                      <i className="fas fa-book-reader icons"></i>
                      <span>Diário de classe</span>
                    </span>
                  }>
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
                    <span>
                      <i className="fas fa-list-alt icons"></i>
                      <span>Planejamento</span>
                    </span>
                  }>
                  <Menu.Item key="30" id="plaPlanoCiclo" htmlFor="linkPlanoCiclo">
                    <span className="menuItem">  Plano de Ciclo</span>
                    <Link to="/planejamento/plano-ciclo/2019/1" className="nav-link text-white" id="linkPlanoCiclo" />
                  </Menu.Item>
                  <Menu.Item key="31" id="plaPlanoAnual" htmlFor="linkPlanoAnual">
                    <span className="menuItem"> Plano anual</span>
                    <Link to="/planejamento/plano-anual" className="nav-link text-white" id="linkPlanoAnual" />
                  </Menu.Item>
                </SubMenu>
                <SubMenu
                  id="fechamento"
                  key="subFechamento"
                  title={
                    <span>
                      <i className="fas fa-pencil-ruler icons"></i>
                      <span>Fechamento</span>
                    </span>
                  }>
                  <Menu.Item key="50" id="fecConselhoClasse">
                    <span className="menuItem">  Conselho de Classe</span>
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
                    <span>
                      <i className="fas fa-file-alt icons"></i>
                      <span>Relatórios</span>
                    </span>
                  }>
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
                    <span>
                      <i className="fas fa-user-cog icons"></i>
                      <span>Gestão</span>
                    </span>
                  }>
                  <Menu.Item key="90" id="gesCalendarioEscolar">
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
                  </Menu.Item>
                </SubMenu>
                <SubMenu style={{color: 'blue'}}
                  disabled
                  id="configuracoes"
                  key="subConfiguracoes"
                  title={
                    <span>
                      <i className="fas fa-cog icons"></i>
                      <span>Configurações</span>
                    </span>
                  }>
                </SubMenu>
              </Menu>
            </div>
          </MenuScope>
          <DivFooter>
            <Footer>
              <div className="logo-secretaria">
                <img src={LogoMenuFooter}></img>
              </div>
              <div className="descricao">
                <span>SME-SP-SGA - Distribuído sob</span><br />
                <span>a Licença AGPL V3</span>
              </div>
            </Footer>
          </DivFooter>
        </Sider>
      </Layout>
    </MenuBody>
  );
}

export default Sider;
