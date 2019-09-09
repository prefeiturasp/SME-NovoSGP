import { Modal } from 'antd';
import React, { Children } from 'react';
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
    onConfirmacaoPrincipal,
    onConfirmacaoSecundaria,
    onClose,
    labelBotaoPrincipal,
    labelBotaoSecundario,
    titulo,
  } = props;
  return (
    <Container
      onCancel={onClose}
      title={titulo}
      visible={visivel}
      centered
      footer={[
        <Button
          key="btn-sim-confirmacao"
          label={labelBotaoSecundario}
          color={Colors.Azul}
          border
          className="mr-2 padding-btn-confirmacao"
          onClick={onConfirmacaoSecundaria}
        />,
        <Button
          key="btn-nao-confirmacao"
          label={labelBotaoPrincipal}
          color={Colors.Azul}
          className="padding-btn-confirmacao"
          onClick={onConfirmacaoPrincipal}
        />,
      ]}
    >
      {Children}
    </Container>
  );
};

export default ModalConfirmacao;
