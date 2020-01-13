import React from 'react';
import PropTypes from 'prop-types';

// Styles
import { VerticalCentralizado } from '../styles';

// Componentes
import LinhaBootstrap from './LinhaBootstrap';

function TextoDiasDaSemana({ diasSemana }) {
  /**
   * @description Render helper text above WeekDays component
   */
  const renderHelperText = () => {
    let text = `Ocorre a cada `;
    if (diasSemana.length === 1) {
      text += diasSemana[0].descricao;
    } else if (diasSemana.length === 2) {
      text += `${diasSemana[0].descricao} e ${diasSemana[1].descricao}`;
    } else if (diasSemana.length > 2) {
      text += `${diasSemana
        .map((item, index) =>
          index !== diasSemana.length - 1 ? item.descricao : ''
        )
        .toString()} e ${diasSemana[diasSemana.length - 1].descricao}.`;
    }
    return text;
  };

  return (
    diasSemana.length > 0 && (
      <LinhaBootstrap>
        <VerticalCentralizado className="col-lg-12">
          {renderHelperText()}
        </VerticalCentralizado>
      </LinhaBootstrap>
    )
  );
}

TextoDiasDaSemana.defaultProps = {
  diasSemana: [],
};

TextoDiasDaSemana.propTypes = {
  diasSemana: PropTypes.oneOfType([PropTypes.array]),
};

export default TextoDiasDaSemana;
