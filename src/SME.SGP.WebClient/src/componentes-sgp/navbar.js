import React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { store } from '../redux';
import Filtro from './filtro';
import LogoDoSgp from '../recursos/LogoDoSgp.svg';
import { Base } from '../componentes/colors';
import NavbarNotificacoes from './navbar-notificacoes';
import Perfil from './perfil';
import { Deslogar } from '~/redux/modulos/usuario/actions';
import history from '~/servicos/history';
import { URL_LOGIN, URL_HOME } from '~/constantes/url';
import { limparDadosFiltro } from '~/redux/modulos/filtro/actions';
import { LimparSessao } from '~/redux/modulos/sessao/actions';

const Nav = styled.nav`
  z-index: 3000;
  height: 70px !important;
  padding-left: 15px !important;
  padding-right: 15px !important;
  @media (max-width: 767.98px) {
    height: 140px !important;
  }
`;

const Logo = styled.img`
  height: 65px !important;
  width: 75px !important;
  @media screen and (max-width: 993px) {
    width: 4rem !important;
  }
`;

const Botoes = styled.div`
  height: 45px !important;
  z-index: 101;
`;

const Botao = styled.a`
  display: block !important;
  text-align: center !important;
  cursor: pointer;
`;

const Icone = styled.i`
  align-items: center !important;
  background: ${Base.Roxo} !important;
  border-radius: 50% !important;
  color: ${Base.Branco} !important;
  display: flex !important;
  justify-content: center !important;
  font-size: 15px !important;
  height: 28px !important;
  width: 28px !important;
`;

const Texto = styled.span`
  font-size: 10px !important;
`;

const Div = styled.div`
  margin-left: ${props =>
    props.retraido ? '120px !important' : '260px !important'};
  @media (max-width: 767.98px) {
    left: 50%;
    margin-left: 0 !important;
    transform: translateX(-50%) translateY(-0.5rem);
  }
`;

const Navbar = () => {
  const retraido = useSelector(state => state.navegacao.retraido);

  const onClickSair = () => {
    store.dispatch(limparDadosFiltro());
    store.dispatch(Deslogar());
    store.dispatch(LimparSessao());
    history.push(URL_LOGIN);
  };

  return (
    <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top py-0">
      <div className="container-fluid h-100">
        <div className="d-flex w-100 h-100 position-relative">
          <div
            className={`${
              retraido
                ? 'col-xl-1 col-lg-1 col-md-1 col-sm-4'
                : 'col-xl-2 col-lg-2 col-md-2 col-sm-4'
            }`}
          >
            <Link
              style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
              }}
              to={URL_HOME}
            >
              <Logo
                src={LogoDoSgp}
                alt="SGP"
                className="mx-xl-auto mx-lg-auto mt-xl-0 mt-lg-0 mt-md-0 mt-sm-0 d-block"
              />
            </Link>
          </div>
          <div
            className={`d-flex justify-content-end ${
              retraido
                ? 'col-xl-11 col-lg-11 col-md-11'
                : 'col-xl-10 col-lg-10 col-md-10'
            } col-sm-8`}
          >
            <Botoes className="align-self-xl-center align-self-lg-center align-self-md-center align-self-sm-center mt-xl-0 mt-lg-0 mt-md-0 mt-sm-0">
              <ul className="list-inline p-0 m-0">
                <li className="list-inline-item mr-4">
                  <NavbarNotificacoes
                    Botao={Botao}
                    Icone={Icone}
                    Texto={Texto}
                  />
                </li>
                <li className="list-inline-item mr-4">
                  <Perfil Botao={Botao} Icone={Icone} Texto={Texto} />
                </li>
                <li className="list-inline-item">
                  <Botao className="text-center" onClick={onClickSair}>
                    <Icone className="fa fa-power-off fa-lg" />
                    <Texto className="d-block mt-1">Sair</Texto>
                  </Botao>
                </li>
              </ul>
            </Botoes>
          </div>
          <Div
            retraido={retraido}
            className="d-flex align-self-xl-center align-self-lg-center align-self-md-center align-self-sm-center w-100 position-absolute mb-sm-2 mb-md-2"
          >
            <Filtro />
          </Div>
        </div>
      </div>
    </Nav>
  );
};

export default Navbar;
