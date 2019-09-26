import React from 'react';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import Filtro from './filtro';
import LogoDoSgp from '../recursos/LogoDoSgp.svg';
import { Base } from '../componentes/colors';
import { useSelector } from 'react-redux';
import Perfil from './perfil';

const Navbar = () => {
  const retraido = useSelector(state => state.navegacao.retraido);
  const usuario = useSelector(state => state.usuario.rf);

  const Nav = styled.nav`
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

  const Div = styled.div`
    margin-left: ${retraido ? '120px' : '260px'} !important;
    @media (max-width: 767.98px) {
      left: 50%;
      margin-left: 0 !important;
      transform: translateX(-50%) translateY(-0.5rem);
    }
  `;

  return (
    <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top">
      <Link
        className={`navbar-brand ${
          retraido
            ? 'col-lg-2 col-md-2 col-sm-2 col-xl-1 pl-0'
            : 'col-sm-4 col-md-3 col-lg-3 col-xl-2'
        }`}
        to="/"
      >
        <Logo src={LogoDoSgp} alt="SGP" className="mx-auto d-block" />
      </Link>
      <div
        className={`${
          retraido
            ? 'col-lg-10 col-md-10 col-sm-10 col-xl-11'
            : 'col-sm-8 col-md-9 col-lg-9 col-xl-10'
        }`}
      >
        <Filtro />
        <Botoes className="float-right d-flex align-items-center mr-4">
          <ul className="list-inline p-0 m-0">
            <li className="list-inline-item text-center">
              <Perfil/>
            </li>
            <li className="list-inline-item text-center">
              <BtnSair>
                <Icone className="fa fa-power-off fa-lg mb-1" />
                Sair
              </BtnSair>
            </li>
          </ul>
        </Botoes>
      </div>
    </Nav>
  );
};

export default Navbar;