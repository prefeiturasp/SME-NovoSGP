import React from 'react';
import PropTypes from 'prop-types';


// Components
import SelectComponent from '~/componentes/select';

function RecorrenciaDropDown({ onChange, selected, form }) {
  const items = [
    {
      desc: 'No dia',
      valor: '0',
    },
    {
      desc: 'Primeiro',
      valor: '1',
    },
    {
      desc: 'Segundo',
      valor: '2',
    },
    {
      desc: 'Terceiro',
      valor: '3',
    },
    {
      desc: 'Quarto',
      valor: '4',
    },
    {
      desc: 'Último',
      valor: '5',
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
      placeholder="Selecione a recorrência"
      form={form}
      name="padraoRecorrencia"
    />
  );
}

RecorrenciaDropDown.defaultProps = {
  onChange: () => {},
  selected: {},
  form: {},
};

RecorrenciaDropDown.propTypes = {
  onChange: PropTypes.func,
  selected: PropTypes.oneOfType([
    PropTypes.object,
    PropTypes.any,
    PropTypes.number,
    PropTypes.string,
  ]),
  form: PropTypes.oneOfType([
    PropTypes.object,
    PropTypes.symbol,
    PropTypes.any,
  ]),
};

export default RecorrenciaDropDown;
