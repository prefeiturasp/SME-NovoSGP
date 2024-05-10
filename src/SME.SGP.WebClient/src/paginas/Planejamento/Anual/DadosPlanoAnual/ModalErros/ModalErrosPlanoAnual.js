import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import {
  setErrosPlanoAnual,
  setExibirModalErrosPlanoAnual,
} from '~/redux/modulos/anual/actions';

function ModalErrosPlanoAnual() {
  const dispatch = useDispatch();

  const exibirModalErrosPlanoAnual = useSelector(
    store => store.planoAnual.exibirModalErrosPlanoAnual
  );
  const errosPlanoAnual = useSelector(
    store => store.planoAnual.errosPlanoAnual
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosPlanoAnual(false));
    dispatch(setErrosPlanoAnual([]));
  };

  return (
    <ModalMultiLinhas
      key="erros-plano-anual"
      visivel={exibirModalErrosPlanoAnual}
      onClose={onCloseErros}
      type="error"
      conteudo={errosPlanoAnual}
      titulo="Erros Plano anual"
    />
  );
}

export default ModalErrosPlanoAnual;
