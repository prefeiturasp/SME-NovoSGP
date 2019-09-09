import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Modal } from 'antd';
import styled from 'styled-components';
import { alertaFechar } from '../redux/modulos/alertas/actions';
import Rotas from '../rotas/rotas';
import Button from '../componentes/button';
import { Colors } from '../componentes/colors';
import BreadcrumbSgp from '../componentes-sgp/breadcrumb-sgp'

const ContainerModal = styled.div`
  .ant-modal-footer {
    border-top: 0px !important;
  }
`;

const Conteudo = () => {
  const NavegacaoStore = useSelector(store => store.navegacao);
  const [collapsed, setCollapsed] = useState(false);
  const dispatch = useDispatch();
  useEffect(() => {
    setCollapsed(NavegacaoStore.collapsed);
  }, [NavegacaoStore.collapsed]);

  const confirmacao = useSelector(state => state.notificacoes.confirmacao);

  useEffect(() => { setCollapsed(NavegacaoStore.collapsed); }, [NavegacaoStore.collapsed]);

  const fecharConfirmacao = resultado => {
    confirmacao.resolve(resultado);
    dispatch(alertaFechar());
  };

  return (
    <div style={{ marginLeft: collapsed ? '115px' : '250px' }}>
      <BreadcrumbSgp />
      <div className="row h-100">
        <main role="main" className="col-md-12 col-lg-12 col-sm-12 col-xl-12">
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
            <Rotas />
          </div>
        </main>
      </div>
    </div>
  );
};

export default Conteudo;
