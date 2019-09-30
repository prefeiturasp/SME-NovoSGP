import React, { useLayoutEffect, useState, useRef } from 'react';
import styled from 'styled-components';
import LogoDoSgp from '~/recursos/LogoDoSgp.svg';
import { Base } from '~/componentes/colors';

const PrimeiroAcesso = props => {
  const Nav = styled.nav`
    height: 70px;
  `;

  const Logo = styled.img`
    max-height: 65px;
    max-width: 75px;
  `;

  const Div = styled.div`
    button {
      margin-right: 1rem;
    }
    button:last-child {
      margin-right: 0;
    }
  `;

  const Container = styled(Div)`
    background: ${Base.Branco};
    height: 100%;
  `;

  const Texto = styled(Div)`
    font-size: 14px;
    letter-spacing: normal;
    line-height: normal;
  `;

  return (
    <>
      <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top py-0">
        <Logo
          src={LogoDoSgp}
          className="mx-auto"
          alt="Novo Sistema de Gestão Pedagógica"
        />
      </Nav>
      <Container className="container text-center shadow-sm mx-auto mt-4 rounded">
        <Div className="row">
          <Div className="col-xl-12 col-lg-12 col-md-12 col-sm-12">
            <Texto
              className="mx-auto"
              style={{ marginBottom: '70px', maxWidth: '560px' }}
            >
              Olar
            </Texto>
          </Div>
        </Div>
      </Container>
    </>
  );
};

export default PrimeiroAcesso;
