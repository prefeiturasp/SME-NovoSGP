import React from 'react';
import PropTypes from 'prop-types';

// Styles
import { VerticalCentered } from '../styles';

// Componentes
import BootstrapRow from './BootstrapRow';

function HelperText({ diasSemana }) {
  /**
   * @description Render helper text above WeekDays component
   */
  const renderHelperText = () => {
    let text = `Ocorre a cada `;
    if (diasSemana.length === 1) {
      text += diasSemana[0].description;
    } else if (diasSemana.length === 2) {
      text += `${diasSemana[0].description} e ${diasSemana[1].description}`;
    } else if (diasSemana.length > 2) {
      text += `${diasSemana
        .map((item, index) =>
          index !== diasSemana.length - 1 ? item.description : ''
        )
        .toString()} e ${diasSemana[diasSemana.length - 1].description}.`;
    }
    return text;
  };

  return (
    diasSemana.length > 0 && (
      <BootstrapRow>
        <VerticalCentered className="col-lg-12">
          {renderHelperText()}
        </VerticalCentered>
      </BootstrapRow>
    )
  );
}

HelperText.defaultProps = {
  diasSemana: [],
};

HelperText.propTypes = {
  diasSemana: PropTypes.array,
};

export default HelperText;
