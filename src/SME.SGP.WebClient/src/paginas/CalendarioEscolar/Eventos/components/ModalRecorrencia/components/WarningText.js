import React from 'react';
import PropTypes from 'prop-types';

// Styles
import { SmallText, VerticalCentered } from '../styles';

// Componentes
import BootstrapRow from './BootstrapRow';

function WarningText({ dataTermino }) {
  return (
    !dataTermino && (
      <BootstrapRow>
        <VerticalCentered className="col-lg-12">
          <SmallText>
            Se não selecionar uma data fim, será considerado o fim do ano
            letivo.
          </SmallText>
        </VerticalCentered>
      </BootstrapRow>
    )
  );
}

WarningText.propTypes = {
  dataTermino: PropTypes.oneOfType([PropTypes.object, PropTypes.any]),
};

WarningText.defaultProps = {
  dataTermino: {},
};

export default WarningText;
