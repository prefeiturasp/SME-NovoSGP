import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { Base } from '../componentes/colors';

const Sidebar = () => {
  const Nav = styled.nav`
    background: ${Base.Roxo};
    bottom: 0;
    left: 0;
    top: 0;
    z-index: 100;
  `;

  return (
    <Nav className="col-md-2 d-none d-md-block sidebar pt-2 h-100">
      <div className="sidebar-sticky">
        <ul className="nav flex-column">
          <li className="nav-item">
            <Link
              to="/planejamento/plano-ciclo"
              className="nav-link text-white"
            >
              Plano de Ciclo
            </Link>
          </li>
          <li className="nav-item">
            <Link
              to="/planejamento/plano-anual"
              className="nav-link text-white"
            >
              Plano Anual
            </Link>
          </li>
        </ul>
      </div>
    </Nav>
  );
};

export default Sidebar;
