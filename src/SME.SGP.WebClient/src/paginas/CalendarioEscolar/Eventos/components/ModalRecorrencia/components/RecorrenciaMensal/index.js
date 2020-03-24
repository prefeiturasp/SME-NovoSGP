import React from 'react';
import PropTypes from 'prop-types';

// Components
import LinhaBootstrap from '../LinhaBootstrap';
import DiasDropDown from './components/DiasDropDown';
import RecorrenciaDropDown from './components/RecorrenciaDropDown';
import NumeroDia from './components/NumeroDia';

function RecorrenciaMensal({
  currentRecurrence,
  onChangeRecurrence,
  currentDayNumber,
  onChangeDayNumber,
  currentWeekDay,
  onChangeWeekDay,
  form,
}) {
  return (
    <>
      <LinhaBootstrap>
        <div className="col-lg-12">
          <span style={{ paddingBottom: '7px', fontWeight: 'bold' }}>
            Padrão de recorrência
          </span>
        </div>
        <div className="col-lg-6">
          <RecorrenciaDropDown
            selected={currentRecurrence}
            onChange={onChangeRecurrence}
            form={form}
          />
        </div>
        {currentRecurrence === '0' ? (
          <div className="col-lg-4">
            <NumeroDia
              form={form}
              value={currentDayNumber}
              onChange={onChangeDayNumber}
            />
          </div>
        ) : (
          <div className="col-lg-6">
            <DiasDropDown
              onChange={onChangeWeekDay}
              selected={currentWeekDay}
              form={form}
            />
          </div>
        )}
      </LinhaBootstrap>
    </>
  );
}

RecorrenciaMensal.defaultProps = {
  form: {},
  onChangeRecurrence: () => {},
  onChangeDayNumber: () => {},
  onChangeWeekDay: () => {},
  currentRecurrence: {},
  currentDayNumber: 0,
  currentWeekDay: {},
};

RecorrenciaMensal.propTypes = {
  form: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onChangeRecurrence: PropTypes.func,
  onChangeDayNumber: PropTypes.func,
  onChangeWeekDay: PropTypes.func,
  currentDayNumber: PropTypes.number,
  currentRecurrence: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
    PropTypes.symbol,
    PropTypes.any,
  ]),
  currentWeekDay: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
    PropTypes.symbol,
    PropTypes.any,
  ]),
};

export default RecorrenciaMensal;
