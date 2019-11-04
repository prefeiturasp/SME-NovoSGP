import React from 'react';

// Components
import CampoNumero from '~/componentes/campoNumero';

export default function DayNumber({ onChange, value, form }) {
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
