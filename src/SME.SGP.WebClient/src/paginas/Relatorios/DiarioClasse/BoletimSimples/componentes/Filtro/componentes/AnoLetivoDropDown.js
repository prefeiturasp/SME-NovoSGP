import React, { useEffect } from 'react';
import PropTypes from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent } from '~/componentes';

function AnoLetivoDropDown({ form, onChange }) {
  const anosLetivos = useSelector(store => store.filtro.anosLetivos);

  useEffect(() => {
    if (anosLetivos.length === 1) {
      form.setFieldValue('anoLetivo', String(anosLetivos[0].valor), false);
      onChange(String(anosLetivos[0].valor));
    }
  }, [anosLetivos]);

  return (
    <SelectComponent
      label="Ano Letivo"
      valueOption="valor"
      valueText="desc"
      form={form}
      name="anoLetivo"
      lista={anosLetivos}
      placeholder="Ano Letivo"
      onChange={onChange}
      disabled={anosLetivos && anosLetivos.length === 1}
    />
  );
}

AnoLetivoDropDown.propTypes = {
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
};

AnoLetivoDropDown.defaultProps = {
  form: {},
  onChange: () => null,
};

export default AnoLetivoDropDown;
