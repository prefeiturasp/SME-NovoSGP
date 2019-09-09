import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Modal } from 'antd';
import styled from 'styled-components';
import { alertaFechar } from '../redux/modulos/alertas/actions';
import Rotas from '../rotas/rotas';
import Button from '../componentes/button';
import { Colors } from '../componentes/colors';
import Alert from '~/componentes/alert';

const ContainerModal = styled.div`
  .ant-modal-footer {
    border-top: 0px !important;
  }
`;

const Conteudo = () => {
  const MenuStore = useSelector(store => store.menu);
  const [collapsed, setCollapsed] = useState(false);
  const dispatch = useDispatch();
  useEffect(() => {
    setCollapsed(MenuStore.collapsed);
  }, [MenuStore.collapsed]);
  const notificacoes = useSelector(state => state.notificacoes);

  useEffect(() => {
    setCollapsed(MenuStore.collapsed);
  }, [MenuStore.collapsed]);
  const confirmacao = useSelector(state => state.notificacoes.confirmacao);

  const fecharConfirmacao = resultado => {
    confirmacao.resolve(resultado);
    dispatch(alertaFechar());
  };

  return (
    <main
      role="main"
      className={
        collapsed
          ? 'col-lg-10 col-md-10 col-sm-10 col-xs-12 col-xl-11'
          : 'col-sm-8 col-md-9 col-lg-9 col-xl-10'
      }
    >
      <ContainerModal>
        <Modal
          title={confirmacao.titulo}
          visible={confirmacao.visivel}
          onOk={() => fecharConfirmacao(true)}
          onCancel={() => fecharConfirmacao(false)}
          footer={[
            <Button
              key="btn-sim"
              onClick={() => fecharConfirmacao(true)}
              label="Sim"
              color={Colors.Azul}
              border
              className="mr-3"
            />,
            <Button
              label="Não"
              type="primary"
              onClick={() => fecharConfirmacao(false)}
              color={Colors.Azul}
            />,
          ]}
        >
          {confirmacao.texto}
          <br />
          <b>{confirmacao.textoNegrito}</b>
        </Modal>
      </ContainerModal>
      <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
        <div className="col-md-12">
          {notificacoes.alertas.map(alerta => (
            <Alert alerta={alerta} key={alerta.id} />
          ))}
        </div>
        <Rotas />
      </div>
    </main>
  );
};

export default Conteudo;
