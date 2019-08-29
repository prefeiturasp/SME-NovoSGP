import { Modal } from 'antd';
import React from 'react';
import styled from 'styled-components';

import Button from './button';
import { Colors } from './colors';

const Container = styled(Modal)`
  .ant-modal-footer {
    border-top: none;
  }

  .padding-btn-confirmacao {
    padding: 22px;
  }

  .ant-modal-header {
    border-bottom: none;
    padding-top: 27px;
    padding-bottom: 0px;
  }

  .ant-modal-title {
    border-bottom: solid 1.5px rgba(0, 0, 0, 0.12);
    color: #42474a;
    font-size: 25px;
    padding-bottom: 7px;
  }

  .ant-modal-close-x {
    font-style: normal;
    line-height: 40px;
    color: #42474a;
  }

  p {
    margin-bottom: 0;
  }
`;

const ModalConfirmacao = props => {
  const {
    visivel,
    onConfirmacaoSim,
    onConfirmacaoNao,
    conteudo,
    perguntaDoConteudo,
    titulo,
  } = props;
  return (
    <Container
      onCancel={onConfirmacaoNao}
      title={titulo}
      visible={visivel}
      centered
      footer={[
        <Button
          key="btn-sim-confirmacao"
          label="Sim"
          color={Colors.Azul}
          border
          className="mr-2 padding-btn-confirmacao"
          onClick={onConfirmacaoSim}
        />,
        <Button
          key="btn-nao-confirmacao"
          label="Não"
          color={Colors.Azul}
          border
          className="padding-btn-confirmacao"
          onClick={onConfirmacaoNao}
        />,
      ]}
    >
      {conteudo ? <p> {conteudo}</p> : ''}

      {perguntaDoConteudo ? (
        <p>
          <b>{perguntaDoConteudo}</b>
        </p>
      ) : (
        ''
      )}
    </Container>
  );
};

export default ModalConfirmacao;
