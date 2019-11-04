import React from 'react';
import PropTypes from 'prop-types';

// Components
import { VerticalCentered } from '../../styles';
import BootstrapRow from '../BootstrapRow';
import DaysDropDown from './components/DaysDropDown';
import RecurrenceDropDown from './components/RecurrenceDropDown';
import DayNumber from './components/DayNumber';

function MonthlyRecurrence({
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
      <BootstrapRow>
        <VerticalCentered className="col-lg-12">
          <span style={{ paddingBottom: '7px', fontWeight: 'bold' }}>
            Padrão de recorrência
          </span>
        </VerticalCentered>
        <VerticalCentered className="col-lg-4">
          <RecurrenceDropDown
            selected={currentRecurrence}
            onChange={onChangeRecurrence}
            form={form}
          />
        </VerticalCentered>
        {currentRecurrence === '0' ? (
          <VerticalCentered className="col-lg-4">
            <DayNumber
              form={form}
              value={currentDayNumber}
              onChange={onChangeDayNumber}
            />
          </VerticalCentered>
        ) : (
          <VerticalCentered className="col-lg-4">
            <DaysDropDown
              onChange={onChangeWeekDay}
              selected={currentWeekDay}
              form={form}
            />
          </VerticalCentered>
        )}
      </BootstrapRow>
    </>
  );
}

MonthlyRecurrence.defaultProps = {
  form: {},
  onChangeRecurrence: () => {},
  onChangeDayNumber: () => {},
  onChangeWeekDay: () => {},
  currentRecurrence: {},
  currentDayNumber: 0,
  currentWeekDay: {},
};

MonthlyRecurrence.propTypes = {
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

export default MonthlyRecurrence;
