import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { ModalMultiLinhas } from '~/componentes';
import {
  setExibirModalErrosRalSemestralPAP,
  setErrosRalSemestralPAP,
} from '~/redux/modulos/relatorioSemestralPAP/actions';

function ModalErrosRalSemestralPAP() {
  const dispatch = useDispatch();

  const exibirModalErrosRalSemestralPAP = useSelector(
    store => store.relatorioSemestralPAP.exibirModalErrosRalSemestralPAP
  );
  const errosRalSemestralPAP = useSelector(
    store => store.relatorioSemestralPAP.errosRalSemestralPAP
  );

  const onCloseErros = () => {
    dispatch(setExibirModalErrosRalSemestralPAP(false));
    dispatch(setErrosRalSemestralPAP([]));
  };

  return (
    <ModalMultiLinhas
      key="erros-ral-semestral-pap"
      visivel={exibirModalErrosRalSemestralPAP}
      onClose={onCloseErros}
      type="error"
      conteudo={errosRalSemestralPAP}
      titulo="Campos obrigatórios"
    />
  );
}

export default ModalErrosRalSemestralPAP;
