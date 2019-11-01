import React from 'react';

// Components
import BootstrapRow from '../BootstrapRow';
import RadioStyleButton from './components/RadioStyledButton';
import { VerticalCentered } from '../../styles';

function MonthlyRecurrence() {
  return (
    <>
      <BootstrapRow>
        <VerticalCentered className="col-lg-12">
          <RadioStyleButton name="italo" value="D" />
        </VerticalCentered>
      </BootstrapRow>
      <BootstrapRow>
        <VerticalCentered className="col-lg-12">
          <RadioStyleButton name="italo" value="P" />
        </VerticalCentered>
      </BootstrapRow>
    </>
  );
}

export default MonthlyRecurrence;
