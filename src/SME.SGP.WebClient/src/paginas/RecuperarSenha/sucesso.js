import React from 'react';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const Sucesso = () => {
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

  return (
    <Span className="d-block mx-auto">
      <Alerta className="d-block rounded-circle mx-auto position-relative">
        <Icon className="fa fa-check position-absolute" />
      </Alerta>
      <div className="mb-3">
        Seu link de recuperação de senha foi enviado para
        mar**********@gmail.com.
      </div>
      <div className="mb-3">Verifique sua caixa de entrada!</div>
    </Span>
  );
};

export default Sucesso;
