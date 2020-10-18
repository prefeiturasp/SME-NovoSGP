import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import {
  setErrosPlanoAula,
  setExibirModalErrosPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';

function ModalErrosPlanoAula() {
  const dispatch = useDispatch();

  const exibirModalErrosPlanoAula = useSelector(
    store => store.frequenciaPlanoAula.exibirModalErrosPlanoAula
  );
  const errosPlanoAula = useSelector(
    store => store.frequenciaPlanoAula.errosPlanoAula
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosPlanoAula(false));
    dispatch(setErrosPlanoAula([]));
  };

  return (
    <ModalMultiLinhas
      key="erros-plano-aula"
      visivel={exibirModalErrosPlanoAula}
      onClose={onCloseErros}
      type="error"
      conteudo={errosPlanoAula}
      titulo="Erros Plano aula"
    />
  );
}

export default ModalErrosPlanoAula;
