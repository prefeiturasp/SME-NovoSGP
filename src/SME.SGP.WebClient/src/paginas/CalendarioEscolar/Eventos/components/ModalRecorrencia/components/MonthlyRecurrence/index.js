import React from 'react';

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
          <span style={{ paddingBottom: '7px' }}>Padrão de recorrência</span>
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
            <DayNumber value={currentDayNumber} onChange={onChangeDayNumber} />
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

export default MonthlyRecurrence;
