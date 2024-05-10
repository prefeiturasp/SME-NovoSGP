import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from '~/componentes/colors';

const Sucesso = ({ email }) => {
  const Span = styled.span`
    color: ${Base.Verde};
    margin-top: 70px;
    max-width: 295px;
  `;

  const Alerta = styled.div`
    border: 3px solid ${Base.Verde};
    height: 100px;
    margin-bottom: 25px;
    width: 100px;
  `;

  const Icon = styled.i`
    font-size: 36px;
    left: 50%;
    top: 50%;
    transform: translateX(-50%) translateY(-50%);
  `;

  const emailFormatado = () => {
    const inicioServidor = email.lastIndexOf('@');
    const totalASubstituir = email.substring(3, inicioServidor).length;
    return `   ${email.replace(
      email.substring(3, inicioServidor),
      '*'.repeat(totalASubstituir)
    )}`;
  };

  return (
    <Span className="d-block mx-auto">
      <Alerta className="d-block rounded-circle mx-auto position-relative">
        <Icon className="fa fa-check position-absolute" />
      </Alerta>
      <div className="mb-3">
        Seu link de recuperação de senha foi enviado para
        {emailFormatado()}
      </div>
      <div className="mb-3">Verifique sua caixa de entrada!</div>
    </Span>
  );
};

Sucesso.propTypes = {
  email: PropTypes.string,
};

Sucesso.defaultProps = {
  email: '',
};

export default Sucesso;
