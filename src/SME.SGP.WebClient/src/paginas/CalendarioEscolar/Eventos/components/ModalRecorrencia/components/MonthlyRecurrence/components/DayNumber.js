import React from 'react';
import PropTypes from 'prop-types';

// Components
import CampoNumero from '~/componentes/campoNumero';

function DayNumber({ onChange, value, form }) {
  return (
    <CampoNumero
      form={form}
      name="dayNumber"
      max={31}
      type="number"
      onChange={onChange}
      value={value}
    />
  );
}

DayNumber.defaultProps = {
  value: 0,
  onChange: () => {},
  form: {},
};

DayNumber.propTypes = {
  form: PropTypes.oneOfType([PropTypes.object, PropTypes.symbol]),
  onChange: PropTypes.func,
  value: PropTypes.number,
};

export default DayNumber;
