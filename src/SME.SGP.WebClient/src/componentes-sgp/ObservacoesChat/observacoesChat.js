import PropTypes from 'prop-types';
import React from 'react';
import { Label } from '~/componentes';
import CampoObservacao from './campoObservacao';
import { ContainerObservacoesChat } from './observacoesChat.css';
import ObservacoesChatMontarDados from './observacoesChatMontarDados';

const ObservacoesChat = props => {
  const { salvarObservacao, editarObservacao, excluirObservacao } = props;

  return (
    <div className="col-sm-12 mb-2 mt-4">
      <Label text="Observações" />
      <ContainerObservacoesChat>
        <div style={{ margin: '15px' }}>
          <CampoObservacao salvarObservacao={salvarObservacao} />
          <ObservacoesChatMontarDados
            onClickSalvarEdicao={editarObservacao}
            onClickExcluir={excluirObservacao}
          />
        </div>
      </ContainerObservacoesChat>
    </div>
  );
};

ObservacoesChat.propTypes = {
  editarObservacao: PropTypes.func,
  salvarObservacao: PropTypes.func,
  excluirObservacao: PropTypes.func,
};

ObservacoesChat.defaultProps = {
  editarObservacao: () => {},
  salvarObservacao: () => {},
  excluirObservacao: () => {},
};

export default ObservacoesChat;
