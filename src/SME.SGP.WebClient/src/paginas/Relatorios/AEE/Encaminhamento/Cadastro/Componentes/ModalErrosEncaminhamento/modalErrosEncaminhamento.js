import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import { setExibirModalErrosEncaminhamento } from '~/redux/modulos/encaminhamentoAEE/actions';

function ModalErrosEncaminhamento() {
  const dispatch = useDispatch();

  const exibirModalErrosEncaminhamento = useSelector(
    store => store.encaminhamentoAEE.exibirModalErrosEncaminhamento
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosEncaminhamento(false));
  };

  return (
    <ModalMultiLinhas
      key="erros-encaminhamento"
      visivel={exibirModalErrosEncaminhamento}
      onClose={onCloseErros}
      type="error"
      conteudo={['Existem campos obrigatórios não preenchidos']}
      titulo="Atenção"
    />
  );
}

export default ModalErrosEncaminhamento;
