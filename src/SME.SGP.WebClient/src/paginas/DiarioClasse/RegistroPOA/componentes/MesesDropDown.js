import React from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

function MesesDropDown({ form, label, desabilitado }) {
  const listaMeses = [
    {
      valor: '1',
      desc: '1',
    },
    {
      valor: '2',
      desc: '2',
    },
  ];

  return (
    <SelectComponent
      label={label}
      valueOption="valor"
      valueText="desc"
      lista={listaMeses}
      form={form}
      name="mes"
      placeholder="Bimestre"
      className="select-mes"
      disabled={desabilitado}
    />
  );
}

MesesDropDown.propTypes = {
  form: t.oneOfType([t.any]),
  label: t.string,
  desabilitado: t.bool,
};

MesesDropDown.defaultProps = {
  form: null,
  label: null,
  desabilitado: false,
};

export default MesesDropDown;
