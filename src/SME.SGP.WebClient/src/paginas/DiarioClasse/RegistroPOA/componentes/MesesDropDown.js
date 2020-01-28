import React from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

function MesesDropDown({ form, label, desabilitado, name }) {
  const listaMeses = [
    {
      valor: '1',
      desc: '1º Bimestre',
    },
    {
      valor: '2',
      desc: '2º Bimestre',
    },
    {
      valor: '3',
      desc: '3º Bimestre',
    },
    {
      valor: '4',
      desc: '4º Bimestre',
    },
  ];

  return (
    <SelectComponent
      label={label}
      valueOption="valor"
      valueText="desc"
      lista={listaMeses}
      form={form}
      name={name}
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
  name: t.string,
};

MesesDropDown.defaultProps = {
  form: null,
  label: null,
  desabilitado: false,
  name: 'mes',
};

export default MesesDropDown;
