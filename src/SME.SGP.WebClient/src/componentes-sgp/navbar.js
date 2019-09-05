import React from 'react';
import { Link } from 'react-router-dom';
import Filtro from './filtro';
import styled from 'styled-components';
import LogoSGP from '../recursos/LogoSGP.svg';

const Navbar = () => {
  const Nav = styled.nav`
    height: 70px;
  `;

  const collapsed = true;

  return (
    <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top">
      <Link
        className={`navbar-brand ${} mr-0`}
        to="/"
      >
        SGP
      </Link>
      <Filtro />
    </Nav>
  );
};

export default Navbar;
