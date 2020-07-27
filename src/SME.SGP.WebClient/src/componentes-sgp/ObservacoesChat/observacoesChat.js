import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Label } from '~/componentes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import {
  CampoObservacao,
  ContainerObservacoesChat,
} from './observacoesChat.css';
import ObservacoesChatMontarDados from './observacoesChatMontarDados';

const ObservacoesChat = props => {
  const { onClickSalvarNovo, onClickSalvarEdicao, onClickExcluir } = props;

  const [novaObservacao, setNovaObservacao] = useState('');

  const onChangeNovaObservacao = ({ target: { value } }) => {
    setNovaObservacao(value);
  };

  const onClickCancelarNovo = () => {
    setNovaObservacao('');
  };

  const onClickSalvar = () => {
    onClickSalvarNovo(novaObservacao).then(() => {
      setNovaObservacao('');
    });
  };

  return (
    <div className="col-sm-12 mb-2 mt-2">
      <Label text="Observações" />
      <ContainerObservacoesChat>
        <div style={{ margin: '15px' }}>
          <div className="col-md-12 pb-2">
            <Label text="Escreva uma observação" />
            <CampoObservacao
              id="nova-observacao"
              autoSize={{ minRows: 4 }}
              value={novaObservacao}
              onChange={onChangeNovaObservacao}
            />
          </div>
          <div className="col-md-12 d-flex justify-content-end pb-4">
            <Button
              id="btn-cancelar-obs-novo"
              label="Cancelar"
              color={Colors.Roxo}
              border
              bold
              className="mr-3"
              onClick={onClickCancelarNovo}
              height="30px"
              disabled={!novaObservacao}
            />
            <Button
              id="btn-salvar-obs-novo"
              label="Salvar"
              color={Colors.Roxo}
              border
              bold
              onClick={onClickSalvar}
              height="30px"
              disabled={!novaObservacao}
            />
          </div>
          <ObservacoesChatMontarDados
            onClickSalvarEdicao={onClickSalvarEdicao}
            onClickExcluir={onClickExcluir}
          />
        </div>
      </ContainerObservacoesChat>
    </div>
  );
};

ObservacoesChat.propTypes = {
  onClickSalvarEdicao: PropTypes.func,
  onClickSalvarNovo: PropTypes.func,
  onClickExcluir: PropTypes.func,
};

ObservacoesChat.defaultProps = {
  onClickSalvarEdicao: () => {},
  onClickSalvarNovo: () => {},
  onClickExcluir: () => {},
};

export default ObservacoesChat;
