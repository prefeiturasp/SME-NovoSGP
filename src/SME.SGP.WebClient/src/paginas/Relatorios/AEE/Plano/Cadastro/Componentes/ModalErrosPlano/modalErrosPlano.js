import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import { setExibirModalErrosPlano } from '~/redux/modulos/planoAEE/actions';

function ModalErrosPlano() {
  const dispatch = useDispatch();

  const exibirModalErrosPlano = useSelector(
    store => store.planoAEE.exibirModalErrosPlano
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosPlano(false));
  };

  return (
    <ModalMultiLinhas
      key="erros-plano"
      visivel={exibirModalErrosPlano}
      onClose={onCloseErros}
      type="error"
      conteudo={['Existem campos obrigatórios não preenchidos']}
      titulo="Atenção"
    />
  );
}

export default ModalErrosPlano;
