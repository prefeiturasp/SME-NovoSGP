import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent } from '~/componentes';

function AnoLetivoDropDown({ form, name, onChange }) {
  const [valorPadrao, setValorPadrao] = useState({});
  const anosLetivos = useSelector(store => store.filtro.anosLetivos);

  useEffect(() => {
    if (anosLetivos.length === 1) {
      form.setFieldValue(name, String(anosLetivos[0].valor), false);
      onChange(String(anosLetivos[0].valor));
      setValorPadrao(anosLetivos[0]);
    }
  }, [anosLetivos]);

  return (
    <SelectComponent
      valueOption="valor"
      valueText="desc"
      form={form}
      name={name}
      lista={anosLetivos}
      placeholder="Ano letivo"
      onChange={onChange}
      disabled={anosLetivos && anosLetivos.length === 1}
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
