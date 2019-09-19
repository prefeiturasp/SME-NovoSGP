import React from 'react';
import { Link } from 'react-router-dom';
import Filtro from './filtro';
import styled from 'styled-components';
import LogoDoSgp from '../recursos/LogoDoSgp.svg';
import { Base } from '../componentes/colors';
import { useSelector } from 'react-redux';

const Navbar = () => {
  const Nav = styled.nav`
    height: 70px !important;
    padding-left: 0 !important;
    padding-right: 0 !important;

    .logo{
      display: flex;
      justify-content: center;
      flex-direction: column;
      align-items: center;
    }
  `;

  const Logo = styled.img`
    max-height: 65px !important;
    max-width: 75px !important;
  `;

  const Botoes = styled.div`
    font-size: 10px !important;
    height: 45px !important;
  `;

  const Icone = styled.i`
    align-items: center !important;
    background: ${Base.Roxo} !important;
    border-radius: 50% !important;
    color: ${Base.Branco} !important;
    display: flex !important;
    justify-content: center !important;
    font-size: 18px !important;
    height: 28px !important;
    width: 28px !important;
  `;

  const BtnSair = styled.a``;

  const retraido = useSelector(state => state.navegacao.retraido);

  return (
    <Nav className="bg-white shadow-sm">
      <div style={{ width: retraido ? '115px' : '250px' }}>
      <Link
        style={{ width: retraido ? '115px' : '250px' }}
        className={'logo'}
        to="/"
      >
        <Logo src={LogoDoSgp} alt="SGP" className="mx-auto d-block" />
      </Link>
      </div>
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
