import React, { useState } from 'react';
import SelectComponent from '~/componentes/select';
import { Combo } from './CampoConceitoPosConselho.css';

const CampoConceitoPosConselho = props => {
  const {
    onChangeNotaConceito,
    nota,
    desabilitarCampo,
    listaTiposConceitos,
  } = props;

  const [valorSelecionado, setValorSelecionado] = useState();

  return (
    <Combo>
      <SelectComponent
        onChange={() => {}}
        valueOption="id"
        valueText="valor"
        lista={listaTiposConceitos}
        valueSelect={valorSelecionado}
        showSearch
        placeholder="Conceito"
        disabled={desabilitarCampo}
      />
    </Combo>
  );
};

export default CampoConceitoPosConselho;
