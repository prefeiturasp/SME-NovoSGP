import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { Label } from '~/componentes';
import {
  ContainerObservacoesChat,
  CampoObservacao,
} from './observacoesChat.css';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import ObservacoesChatMontarDados from './observacoesChatMontarDados';

const ObservacoesChat = props => {
  const { onClickSalvarNovo, dados } = props;

  const [novaObservacao, setNovaObservacao] = useState('');

  const onChangeNovaObservacao = ({ target: { value } }) => {
    setNovaObservacao(value);
  };

  const onClickCancelarNovo = () => {
    setNovaObservacao('');
  };

  useEffect(() => {
    console.log(dados);
  }, [dados]);

  return (
    <div className="col-sm-12 mb-2 mt-2">
      <Label text="Observações" />
      <ContainerObservacoesChat>
        <div style={{ margin: '15px' }}>
          <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 pb-2">
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
              height="25px"
              disabled={!novaObservacao}
            />
            <Button
              id="btn-salvar-obs-novo"
              label="Salvar"
              color={Colors.Roxo}
              border
              bold
              onClick={() => {
                onClickSalvarNovo(novaObservacao);
              }}
              height="25px"
              disabled={!novaObservacao}
            />
          </div>
          <ObservacoesChatMontarDados dados={dados} />
        </div>
      </ContainerObservacoesChat>
    </div>
  );
};

ObservacoesChat.propTypes = {
  onClickSalvarNovo: PropTypes.func,
  dados: PropTypes.oneOfType([PropTypes.array]),
};

ObservacoesChat.defaultProps = {
  onClickSalvarNovo: () => {},
  dados: [],
};

export default ObservacoesChat;
