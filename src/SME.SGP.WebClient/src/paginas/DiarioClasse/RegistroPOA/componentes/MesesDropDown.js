import React from 'react';
import t from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

function MesesDropDown({ form, label }) {
  const listaMeses = [
    {
      valor: '1',
      desc: 'Janeiro',
    },
    {
      valor: '2',
      desc: 'Fevereiro',
    },
    {
      valor: '3',
      desc: 'Março',
    },
    {
      valor: '4',
      desc: 'Abril',
    },
    {
      valor: '5',
      desc: 'Maio',
    },
    {
      valor: '6',
      desc: 'Junho',
    },
    {
      valor: '7',
      desc: 'Julho',
    },
    {
      valor: '8',
      desc: 'Agosto',
    },
    {
      valor: '9',
      desc: 'Setembro',
    },
    {
      valor: '10',
      desc: 'Outubro',
    },
    {
      valor: '11',
      desc: 'Novembro',
    },
    {
      valor: '12',
      desc: 'Dezembro',
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
      placeholder="Mês"
      className="select-mes"
    />
  );
}

MesesDropDown.propTypes = {
	form: t.oneOfType([t.any]),
	label: t.string,
};

MesesDropDown.defaultProps = {
	form: null,
	label: null
};

export default MesesDropDown;
