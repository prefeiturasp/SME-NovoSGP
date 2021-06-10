import React from 'react';
import { Modal, Row } from 'antd';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import Card from './cardBootstrap';
import Button from './button';
import { Base, Colors } from './colors';
import CardBody from './cardBody';
import Grid from './grid';

const Container = styled(Modal)`
  .ant-modal-footer {
    border-top: none;
    padding: 0;
    padding-right: ${({ paddingRight }) => paddingRight}px;
    padding-bottom: ${({ paddingBottom }) => paddingBottom}px;
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
  max-height: 75%;
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
    desabilitarBotaoPrincipal,
    esconderBotoes,
    width,
    fecharAoClicarFora,
    fecharAoClicarEsc,
    esconderBotaoPrincipal,
    esconderBotaoSecundario,
    paddingBottom,
    paddingRight,
    colorBotaoSecundario,
  } = props;
  return (
    <Container
      keyboard={fecharAoClicarEsc}
      maskClosable={fecharAoClicarFora}
      onCancel={onClose}
      title={titulo}
      visible={visivel}
      closable={!!closable}
      centered
      confirmLoading={loader}
      width={width}
      paddingBottom={paddingBottom}
      paddingRight={paddingRight}
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
                    <div
                      className="d-flex justify-content-end"
                      hidden={esconderBotoes}
                    >
                      <Button
                        id={shortid.generate()}
                        key="btn-sim-confirmacao"
                        label={labelBotaoSecundario}
                        color={colorBotaoSecundario}
                        bold
                        border
                        className="mr-2 padding-btn-confirmacao"
                        onClick={onConfirmacaoSecundaria}
                      />
                      <Button
                        id={shortid.generate()}
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
          <div className="d-flex justify-content-end" hidden={esconderBotoes}>
            <Button
              id={shortid.generate()}
              key="btn-sim-confirmacao"
              label={labelBotaoSecundario}
              color={colorBotaoSecundario}
              bold
              border
              className="mr-2 padding-btn-confirmacao"
              onClick={onConfirmacaoSecundaria}
              hidden={esconderBotaoSecundario}
            />
            <Button
              id={shortid.generate()}
              key="btn-nao-confirmacao"
              label={labelBotaoPrincipal}
              color={Colors.Roxo}
              bold
              className="padding-btn-confirmacao"
              onClick={onConfirmacaoPrincipal}
              disabled={desabilitarBotaoPrincipal}
              hidden={esconderBotaoPrincipal}
            />
          </div>
        )
      }
    >
      {children}
    </Container>
  );
};

ModalConteudoHtml.propTypes = {
  width: PropTypes.number,
  desabilitarBotaoPrincipal: PropTypes.bool,
  fecharAoClicarFora: PropTypes.bool,
  fecharAoClicarEsc: PropTypes.bool,
  esconderBotaoPrincipal: PropTypes.bool,
  esconderBotaoSecundario: PropTypes.bool,
  paddingBottom: PropTypes.string,
  paddingRight: PropTypes.string,
  colorBotaoSecundario: PropTypes.string,
};

ModalConteudoHtml.defaultProps = {
  desabilitarBotaoPrincipal: false,
  width: 520,
  fecharAoClicarFora: true,
  fecharAoClicarEsc: true,
  esconderBotaoPrincipal: false,
  esconderBotaoSecundario: false,

  paddingBottom: '15',
  paddingRight: '20',
  colorBotaoSecundario: 'Roxo',
};

export default ModalConteudoHtml;