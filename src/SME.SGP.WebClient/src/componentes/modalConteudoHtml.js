import { Modal, Row } from 'antd';
import React from 'react';
import styled from 'styled-components';
import Card from './cardBootstrap';
import Button from './button';
import { Base, Colors } from './colors';
import CardBody from './cardBody';
import Grid from './grid';

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

const TituloAlerta = styled.h2`
  color: ${Base.VermelhoAlerta};
  font-size: 24px;
  text-align: left;
`;

const TextoAlerta = styled.p`
  font-family: Roboto;
  padding-right: 9px;
  font-size: 14px;
  color: ${Base.VermelhoAlerta};
  text-align: left;
`;

const ModalConteudoHtml = props => {
  const {
    visivel,
    onConfirmacaoPrincipal,
    onConfirmacaoSecundaria,
    onClose,
    labelBotaoPrincipal,
    labelBotaoSecundario,
    titulo,
    tituloAtencao,
    perguntaAtencao,
    children,
    closable,
    loader,
    desabilitarBotaoPrincipal
  } = props;
  return (
    <Container
      onCancel={onClose}
      title={titulo}
      visible={visivel}
      closable={closable ? true : false}
      centered
      confirmLoading={loader}
      footer={
        tituloAtencao || perguntaAtencao ? (
          <>
            <Row className="m-b-10">
              <Grid cols={12} className="p-l-8 p-r-8">
                <Card className="border-2 border-vermelhoAlerta border-radius-4">
                  <CardBody>
                    <TituloAlerta>{tituloAtencao}</TituloAlerta>
                    <TextoAlerta className="m-b-20">
                      {perguntaAtencao}
                    </TextoAlerta>
                    <div className="d-flex justify-content-end">
                      <Button
                        key="btn-sim-confirmacao"
                        label={labelBotaoSecundario}
                        color={Colors.Roxo}
                        bold
                        border
                        className="mr-2 padding-btn-confirmacao"
                        onClick={onConfirmacaoSecundaria}
                      />
                      <Button
                        key="btn-nao-confirmacao"
                        label={labelBotaoPrincipal}
                        color={Colors.Roxo}
                        bold
                        className="padding-btn-confirmacao"
                        onClick={onConfirmacaoPrincipal}
                        disabled={desabilitarBotaoPrincipal}
                      />
                    </div>
                  </CardBody>
                </Card>
              </Grid>
            </Row>
          </>
        ) : (
          <>
            <Button
              key="btn-sim-confirmacao"
              label={labelBotaoSecundario}
              color={Colors.Roxo}
              bold
              border
              className="mr-2 padding-btn-confirmacao"
              onClick={onConfirmacaoSecundaria}
            />
            <Button
              key="btn-nao-confirmacao"
              label={labelBotaoPrincipal}
              color={Colors.Roxo}
              bold
              className="padding-btn-confirmacao"
              onClick={onConfirmacaoPrincipal}
              disabled={desabilitarBotaoPrincipal}
            />
          </>
        )
      }
    >
      {children}
    </Container>
  );
};

ModalConteudoHtml.defaultProps = {
  desabilitarBotaoPrincipal: false
};

export default ModalConteudoHtml;
