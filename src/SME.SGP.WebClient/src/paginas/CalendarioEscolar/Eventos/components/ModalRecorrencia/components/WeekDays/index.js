import React from 'react';
import shortid from 'shortid';
import PropTypes from 'prop-types';

// Components
import DayCircle from './components/DayCircle';

function WeekDays({ onChange, currentState }) {
  const days = [
    { label: 'D', value: 'Domingo' },
    { label: 'S', value: 'Segunda-feira' },
    { label: 'T', value: 'Terça-feira' },
    { label: 'Q', value: 'Quarta-feira' },
    { label: 'Q', value: 'Quinta-feira' },
    { label: 'S', value: 'Sexta-feira' },
    { label: 'S', value: 'Sábado' },
  ];

  return (
    <>
      {days.map(value => (
        <DayCircle
          isActive={
            currentState && currentState.some(x => x.value === value.value)
          }
          onChange={onChange}
          key={shortid.generate()}
          data={value}
        />
      ))}
    </>
  );
}

WeekDays.defaultProps = {
  currentState: [],
  onChange: () => {},
};

WeekDays.propTypes = {
  currentState: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onChange: PropTypes.func,
};

export default WeekDays;
