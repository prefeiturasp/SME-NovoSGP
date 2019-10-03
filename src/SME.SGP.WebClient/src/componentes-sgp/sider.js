import React, { useState, useEffect } from 'react';
import { Menu, Layout } from 'antd';
import { Link } from 'react-router-dom';
import { Base } from '../componentes/colors';
import { MenuBody, DivFooter, MenuScope, Topo } from './sider.css';
import LogoMenuFooter from '../recursos/LogoMenuFooter.svg';
import { store } from '../redux';
import { menuRetraido, menuSelecionado } from '../redux/modulos/navegacao/actions';
import { useSelector } from 'react-redux';
import modalidade from '~/dtos/modalidade';

const Sider = () => {
  const { Sider, Footer } = Layout;
  const { SubMenu } = Menu;
  const NavegacaoStore = useSelector(store => store.navegacao);
  const [openKeys, setOpenKeys] = useState([]);
  const [modalidadeEja, setModalidadeEja] = useState(false);

  const usuario = useSelector(store => store.usuario);

  const subMenusPrincipais = ['subDiarioClasse', 'subPlanejamento', 'subFechamento', 'subRelatorios', 'subGestao', 'subConfiguracoes'];

  useEffect(() => {
    verificaSelecaoMenu(NavegacaoStore.rotaAtiva);
  }, [NavegacaoStore.rotaAtiva]);

  useEffect(() => {
    if (
      usuario &&
      usuario.turmaSelecionada &&
      usuario.turmaSelecionada.length &&
      usuario.turmaSelecionada[0].codModalidade == modalidade.EJA
    ) {
      setModalidadeEja(true);
    } else {
      setModalidadeEja(false);
    }
  }, [usuario.turmaSelecionada]);

  const verificaSelecaoMenu = rotaAtiva => {
    const rota = NavegacaoStore.rotas.get(rotaAtiva);
    setOpenKeys([]);
    if (rota && rota.limpaSelecaoMenu) {
      store.dispatch(menuSelecionado([]));
    }
  };

  const alterarPosicaoJanelaPopup = (idElementoHtml, quantidadeItens) => {
    const itemMenu = window.document.getElementById(idElementoHtml);
    if (itemMenu) {
      const alturaItens = (quantidadeItens * 40) + 6;
      const alturaTela = window.innerHeight
      const posicaoY = itemMenu.getBoundingClientRect().y;
      const posicaoRight = itemMenu.getBoundingClientRect().right;
      const alturaTotalItens = posicaoY + alturaItens;
      const posicaoTop = alturaTotalItens > alturaTela ? (posicaoY-(alturaTotalItens - alturaTela)) : posicaoY;
      document.documentElement.style.setProperty('--posicao-item-menu-top', `${posicaoTop}px`)
      document.documentElement.style.setProperty('--posicao-item-menu-right', `${posicaoRight}px`)
    }
  }

  const alternarRetraido = () => {
    setOpenKeys([]);
    store.dispatch(menuRetraido(!NavegacaoStore.retraido));
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
  }

  return (
    <MenuBody id="main" style={{ width: NavegacaoStore.retraido ? '115px' : '250px' }}>
      <Sider
        style={{ background: Base.Roxo, height: '100%' }}
        collapsed={NavegacaoStore.retraido}
        onCollapse={NavegacaoStore.retraido}
        width="250px"
        collapsedWidth="115px"
      >
        <Topo>
          <div className="conteudo">
            <a className="arrow" onClick={alternarRetraido}>
              <i
                style={{ color: Base.Branco }}
                className={
                  NavegacaoStore.retraido
                    ? 'fas fa-chevron-circle-right'
                    : 'fas fa-chevron-circle-left'
                }
              />
            </a>
          </div>
          <div className={NavegacaoStore.retraido ? 'perfil-retraido' : 'perfil'}>
            <div className="circulo-perfil">
              <img
                id="imagem-perfil"
                src="https://graziellanicolai.com.br/wp-content/uploads/2018/03/Graziella-perfil.jpg"
              />
            </div>
            <div hidden={NavegacaoStore.retraido}>
              <span id="nome" className="nome">
                Nome + Sobrenome
              </span>
            </div>
            <div
              className="perfil-edit"
              style={{ paddingTop: NavegacaoStore.retraido ? '0' : '12px' }}
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
            className={`menu-scope${NavegacaoStore.retraido ? ' menu-scope-retraido' : ''}`}
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
              <SubMenu
                id="diarioClasse"
                key="subDiarioClasse"
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('diarioClasse', 9)}
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-book-reader ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
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
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('planejamento', 2, 200)}
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-list-alt ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
                        }`}
                    />
                    <span>Planejamento</span>
                  </div>
                }
              >
                <Menu.Item key="30" id="plaPlanoCiclo" htmlFor="linkPlanoCiclo">
                  <span className="menuItem">{ modalidadeEja ? 'Plano de Etapa' : 'Plano de Ciclo'}</span>
                  <Link
                    to="/planejamento/plano-ciclo"
                    className="nav-link text-white"
                    id="linkPlanoCiclo"
                  />
                </Menu.Item>
                <Menu.Item key="31" id="plaPlanoAnual" htmlFor="linkPlanoAnual">
                  <span className="menuItem">{ modalidadeEja ? 'Plano Semestral' : 'Plano Anual'}</span>
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
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('fechamento', 3)}
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-pencil-ruler ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
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
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('relatorios', 8)}
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-file-alt ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
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
                  <span className="menuItem">Contraturno</span>
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
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('gestao', 5)}
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-user-cog ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
                        }`}
                    />
                    <span>Gestão</span>
                  </div>
                }
              >
                <Menu.Item
                  key="90"
                  id="gesCalendarioEscolar"
                  className="popup-gestao"
                >
                  <span className="menuItem">Calendário Escolar</span>
                  <Link
                    to="/calendario-escolar/periodos-escolares"
                    className="nav-link text-white"
                    id="linkTesteRemover"
                  />
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
                  <Link
                    to="/gestao/atribuicao-supervisor-lista"
                    className="nav-link text-white"
                    id="linkAtribuicaoSupervisor"
                  />
                </Menu.Item>
              </SubMenu>
              <SubMenu
                onMouseEnter={(e) => alterarPosicaoJanelaPopup('configuracoes', 1)}
                id="configuracoes"
                key="subConfiguracoes"
                title={
                  <div className="item-menu-retraido">
                    <i
                      className={`fas fa-cog ${
                        NavegacaoStore.retraido ? 'icons-retraido' : 'icons'
                        }`}
                    />
                    <span>Configurações</span>
                  </div>
                }
              >
                <SubMenu
                  id="usuarios"
                  key="subUsuarios"
                  onMouseEnter={(e) => alterarPosicaoJanelaPopup('usuarios', 1)}
                  title={
                    <div className="item-menu-retraido submenu-subnivel">
                      <span>Usuários</span>
                    </div>
                  }
                >
                  <Menu.Item key="110" id="usuTrocaSenha">
                    <span className="menuItem">Troca de Senha</span>
                  </Menu.Item>
                </SubMenu>
              </SubMenu>
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
