import React from 'react';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const Erro = () => {
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

  const Icon = styled.i`
    font-size: 36px;
    left: 50%;
    top: 50%;
    transform: translateX(-50%) translateY(-50%);
  `;

  return (
    <Span className="d-block mx-auto">
      <Alerta className="d-block rounded-circle mx-auto position-relative">
        <Icon className="fa fa-times position-absolute" />
      </Alerta>
      <div className="mb-3">
        Você não tem um e-mail cadastrado para recuperar sua senha.
      </div>
      <div className="mb-3">
        Para restabelecer o seu acesso, procure o Diretor da sua UE ou
        Administrador do SGP da sua unidade.
      </div>
    </Span>
  );
};

export default Erro;
