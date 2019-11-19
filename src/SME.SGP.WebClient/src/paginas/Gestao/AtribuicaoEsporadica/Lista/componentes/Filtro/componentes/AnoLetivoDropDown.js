import React from 'react';
import PropTypes from 'prop-types';

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
      valueSelect={anosLetivos && anosLetivos[0]}
      placeholder="Ano letivo"
    />
  );
}

AnoLetivoDropDown.propTypes = {
  form: PropTypes.objectOf(PropTypes.object),
  name: PropTypes.string,
};

AnoLetivoDropDown.defaultProps = {
  form: {},
  name: '',
};

export default AnoLetivoDropDown;
