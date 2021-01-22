import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import {
  setErrosModalEncaminhamento,
  setExibirModalErrosEncaminhamento,
} from '~/redux/modulos/encaminhamentoAEE/actions';

function ModalErrosEncaminhamento() {
  const dispatch = useDispatch();

  const exibirModalErrosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.exibirModalErrosEncaminhamento
  );
  const errosModalEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.errosModalEncaminhamento
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosEncaminhamento(false));
    dispatch(setErrosModalEncaminhamento([]));
  };

  return (
    <ModalMultiLinhas
      key="erros-encaminhamento"
      visivel={exibirModalErrosEncaminhamento}
      onClose={onCloseErros}
      type="error"
      conteudo={errosModalEncaminhamento}
      titulo="Campos obrigatórios"
    />
  );
}

export default ModalErrosEncaminhamento;
