import React from 'react';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent } from '~/componentes';

function AnoLetivoDropDown({ form, name }) {
  const anosLetivos = useSelector(store => store.filtro.anosLetivos);
  return (
    <SelectComponent
      valueOption="valor"
      valueText="desc"
      form={form}
      name={name}
      lista={anosLetivos}
      disabled={anosLetivos && anosLetivos.length === 1}
    />
  );
}

export default AnoLetivoDropDown;
