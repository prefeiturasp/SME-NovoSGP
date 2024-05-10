import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import {
  setErrosCartaIntencoes,
  setExibirModalErrosCartaIntencoes,
} from '~/redux/modulos/cartaIntencoes/actions';

function ModalErrosCartaIntencoes() {
  const dispatch = useDispatch();

  const exibirModalErrosCartaIntencoes = useSelector(
    store => store.cartaIntencoes.exibirModalErrosCartaIntencoes
  );
  const errosCartaIntencoes = useSelector(
    store => store.cartaIntencoes.errosCartaIntencoes
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosCartaIntencoes(false));
    dispatch(setErrosCartaIntencoes([]));
  };

  return (
    <ModalMultiLinhas
      key="erros-carta-intencoes"
      visivel={exibirModalErrosCartaIntencoes}
      onClose={onCloseErros}
      type="error"
      conteudo={errosCartaIntencoes}
      titulo="Erros Carta de intenções"
    />
  );
}

export default ModalErrosCartaIntencoes;
