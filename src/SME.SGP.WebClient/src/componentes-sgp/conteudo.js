import { Modal } from 'antd';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Switch } from 'react-router-dom';
import shortid from 'shortid';
import styled from 'styled-components';
import rotasArray from '~/rotas/rotas';

import Button from '../componentes/button';
import { Colors } from '../componentes/colors';
import { alertaFechar } from '../redux/modulos/alertas/actions';
import RotaAutenticadaEstruturada from '../rotas/rotaAutenticadaEstruturada';
import BreadcrumbSgp from './breadcrumb-sgp';
import Mensagens from './mensagens/mensagens';
import TempoExpiracaoSessao from './tempoExpiracaoSessao/tempoExpiracaoSessao';

const ContainerModal = styled.div`
  .ant-modal-footer {
    border-top: 0px !important;
  }
`;

const ContainerBotoes = styled.div`
  display: flex;
  justify-content: flex-end;
`;

const Conteudo = () => {
  const menuRetraido = useSelector(store => store.navegacao.retraido);
  const dispatch = useDispatch();
  const confirmacao = useSelector(state => state.alertas.confirmacao);

  const fecharConfirmacao = resultado => {
    if (confirmacao) confirmacao.resolve(resultado);
    dispatch(alertaFechar());
  };

  return (
    <div style={{ marginLeft: menuRetraido ? '115px' : '250px' }}>
      <BreadcrumbSgp />
      <TempoExpiracaoSessao />
      <div className="row h-100">
        <main role="main" className="col-md-12 col-lg-12 col-sm-12 col-xl-12">
          <ContainerModal>
            <Modal
              title={confirmacao.titulo}
              visible={confirmacao.visivel}
              onOk={() => fecharConfirmacao(true)}
              onCancel={() => fecharConfirmacao(false)}
              footer={[
                <ContainerBotoes key={shortid.generate()}>
                  <Button
                    key={shortid.generate()}
                    onClick={() => fecharConfirmacao(true)}
                    S
                    label={confirmacao.textoOk}
                    color={Colors.Azul}
                    border
                  />
                  <Button
                    key={shortid.generate()}
                    onClick={() => fecharConfirmacao(false)}
                    label={confirmacao.textoCancelar}
                    type="primary"
                    color={Colors.Azul}
                  />
                </ContainerBotoes>,
              ]}
            >
              {confirmacao.texto && Array.isArray(confirmacao.texto)
                ? confirmacao.texto.map(item => (
                    <div key={shortid.generate()}>{item}</div>
                  ))
                : confirmacao.texto}
              {confirmacao.texto ? <br /> : ''}
              <b>{confirmacao.textoNegrito}</b>
            </Modal>
          </ContainerModal>
          <Mensagens somenteConsulta={menuRetraido.somenteConsulta} />
        </main>
      </div>
      <Switch>
        {rotasArray.map(rota => (
          <RotaAutenticadaEstruturada
            key={shortid.generate()}
            path={rota.path}
            component={rota.component}
            exact={rota.exact}
          />
        ))}
      </Switch>
    </div>
  );
};

export default Conteudo;
