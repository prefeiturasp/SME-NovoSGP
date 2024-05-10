import { Switch } from 'antd';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Label } from '~/componentes';
import {
  setCheckedExibirEscolhaObjetivos,
  setModoEdicaoPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';

function SwitchInformarObjetivos() {
  const dispatch = useDispatch();

  const checkedExibirEscolhaObjetivos = useSelector(
    store => store.frequenciaPlanoAula.checkedExibirEscolhaObjetivos
  );

  const desabilitarCamposPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.desabilitarCamposPlanoAula
  );

  const exibirSwitchEscolhaObjetivos = useSelector(
    state => state.frequenciaPlanoAula.exibirSwitchEscolhaObjetivos
  );

  const possuiPlanoAnual = useSelector(
    state => state.frequenciaPlanoAula.dadosPlanoAula?.possuiPlanoAnual
  );

  const onChangeSwitch = () => {
    dispatch(setCheckedExibirEscolhaObjetivos(!checkedExibirEscolhaObjetivos));
    dispatch(setModoEdicaoPlanoAula(true));
  };

  return (
    <>
      {exibirSwitchEscolhaObjetivos ? (
        <>
          <Label text="Informar Objetivos de Aprendizagem e Desenvolvimento" />
          <Switch
            onChange={() => onChangeSwitch()}
            checked={checkedExibirEscolhaObjetivos}
            size="default"
            className="ml-2 mr-2"
            disabled={desabilitarCamposPlanoAula || !possuiPlanoAnual}
          />
        </>
      ) : (
        ''
      )}
    </>
  );
}

export default SwitchInformarObjetivos;
