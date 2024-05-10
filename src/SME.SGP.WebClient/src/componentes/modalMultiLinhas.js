import { Modal } from 'antd';
import shortid from 'shortid';
import React from 'react';
import styled from 'styled-components';

import Button from './button';
import { Colors } from './colors';

const Container = styled(Modal)`
  .ant-modal-footer {
    border-top: none;
    padding-left: 24px !important;
    padding-right: 24px !important;
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

const ModalMultiLinhas = props => {
  const { visivel, onClose, conteudo, titulo, type } = props;

  return (
    <Container
      onCancel={onClose}
      title={titulo}
      type={type}
      visible={visivel}
      centered
      footer={[
        <Button
          id={shortid.generate()}
          key="btn-sim-confirmacao"
          label="Ok"
          color={Colors.Azul}
          border
          className="mr-2 padding-btn-confirmacao"
          onClick={onClose}
        />,
      ]}
    >
      {conteudo
        ? conteudo.map(content => <p key={shortid.generate()}>{content}</p>)
        : ''}
    </Container>
  );
};

export default ModalMultiLinhas;
