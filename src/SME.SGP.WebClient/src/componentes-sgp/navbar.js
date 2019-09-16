import React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import Filtro from './filtro';
import LogoDoSgp from '../recursos/LogoDoSgp.svg';
import { Base } from '../componentes/colors';
import NavbarNotificacoes from './navbar-notificacoes';

const Navbar = () => {
  const Nav = styled.nav`
    height: 70px !important;
    padding-left: 15px !important;
    padding-right: 15px !important;
  `;

  const Logo = styled.img`
    max-height: 65px !important;
    max-width: 75px !important;
  `;

  const Botoes = styled.div`
    height: 45px !important;
  `;

  const Botao = styled.a`
    display: block !important;
    text-align: center !important;
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

  const collapsed = useSelector(state => state.navegacao.collapsed);
  const usuario = useSelector(state => state.usuario.rf);

  return (
    <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top py-0">
      <Link
        className={`${
          collapsed
            ? 'col-xl-1 col-lg-2 col-md-2 col-sm-2'
            : 'col-xl-2 col-lg-3 col-md-3 col-sm-4'
        }`}
        to={`/${usuario}`}
      >
        <Logo src={LogoDoSgp} alt="SGP" className="mx-auto d-block" />
      </Link>
      <div
        className={`d-flex justify-content-between ${
          collapsed
            ? 'col-xl-11 col-lg-10 col-md-10 col-sm-10'
            : 'col-xl-10 col-lg-9 col-md-9 col-sm-8'
        }`}
      >
        <div className="col-xl-8 col-lg-8 col-md-8 col-sm-12 col-xl-6 pl-5 ml-n2">
          <Filtro />
        </div>
        <div className="col-xl-4 col-lg-4 col-md-4 col-sm-12 d-flex justify-content-end pr-xl-2 pr-lg-2 pr-md-2">
          <Botoes className="d-flex align-items-center">
            <ul className="list-inline p-0 m-0">
              <li className="list-inline-item mr-4">
                <NavbarNotificacoes Botao={Botao} Icone={Icone} Texto={Texto} />
              </li>
              <li className="list-inline-item">
                <Botao className="text-center">
                  <Icone className="fa fa-power-off fa-lg" />
                  <Texto className="d-block mt-1">Sair</Texto>
                </Botao>
              </li>
            </ul>
          </Botoes>
        </div>
      </div>
    </Nav>
  );
};

export default Navbar;
