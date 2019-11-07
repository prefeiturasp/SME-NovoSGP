import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from '~/componentes/colors';

const Erro = ({ mensagem }) => {
  const Span = styled.span`
    color: ${Base.Vermelho};
    margin-top: 70px;
    max-width: 295px;
  `;

  const Alerta = styled.div`
    border: 3px solid ${Base.Vermelho};
    height: 100px;
    margin-bottom: 25px;
    width: 100px;
  `;

  const Icone = styled.i`
    font-size: 36px;
    left: 50%;
    top: 50%;
    transform: translateX(-50%) translateY(-50%);
  `;

  return (
    <Span className="d-block mx-auto">
      <Alerta className="d-block rounded-circle mx-auto position-relative">
        <Icone className="fa fa-times position-absolute" />
      </Alerta>
      <div className="mb-3">{mensagem}</div>
    </Span>
  );
};

Erro.propTypes = {
  mensagem: PropTypes.string,
};

Erro.defaultProps = {
  mensagem: '',
};

export default Erro;
