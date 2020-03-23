import React from 'react';
import PropTypes from 'prop-types';

// Components
import SelectComponent from '~/componentes/select';

function DiasDropDown({ onChange, selected, form }) {
  const items = [
    {
      desc: 'Domingo',
      valor: '0',
    },
    {
      desc: 'Segunda-feira',
      valor: '1',
    },
    {
      desc: 'Terça-feira',
      valor: '2',
    },
    {
      desc: 'Quarta-feira',
      valor: '3',
    },
    {
      desc: 'Quinta-feira',
      valor: '4',
    },
    {
      desc: 'Sexta-feira',
      valor: '5',
    },
    {
      desc: 'Sábado',
      valor: '6',
    },
  ];

  return (
    <SelectComponent
      className="fonte-14"
      onChange={onChange}
      lista={items}
      valueOption="valor"
      valueText="desc"
      valueSelect={selected}
      placeholder="Selecione o dia"
      form={form}
      name="diaSemana"
    />
  );
}

DiasDropDown.defaultProps = {
  onChange: () => {},
  form: {},
  selected: '0',
};

DiasDropDown.propTypes = {
  onChange: PropTypes.func,
  form: PropTypes.oneOfType([PropTypes.object, PropTypes.symbol]),
  selected: PropTypes.oneOfType([
    PropTypes.number,
    PropTypes.string,
    PropTypes.any,
  ]),
};

export default DiasDropDown;
