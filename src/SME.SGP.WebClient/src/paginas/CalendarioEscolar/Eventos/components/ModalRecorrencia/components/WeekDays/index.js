import React from 'react';
import shortid from 'shortid';
import PropTypes from 'prop-types';

// Components
import DayCircle from './components/DayCircle';

function WeekDays({ onChange, currentState }) {
  const days = [
    { label: 'D', value: '0', description: 'Domingo' },
    { label: 'S', value: '1', description: 'Segunda-feira' },
    { label: 'T', value: '2', description: 'Terça-feira' },
    { label: 'Q', value: '3', description: 'Quarta-feira' },
    { label: 'Q', value: '4', description: 'Quinta-feira' },
    { label: 'S', value: '5', description: 'Sexta-feira' },
    { label: 'S', value: '6', description: 'Sábado' },
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
