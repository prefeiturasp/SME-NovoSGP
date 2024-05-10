import React from 'react';
import PropTypes from 'prop-types';

// Styles
import { TextoPequeno, VerticalCentralizado } from '../styles';

// Componentes
import LinhaBootstrap from './LinhaBootstrap';

function TextoInformativo({ dataTermino }) {
  return (
    !dataTermino && (
      <LinhaBootstrap>
        <VerticalCentralizado className="col-lg-12">
          <TextoPequeno>
            Se não selecionar uma data fim, será considerado o fim do ano
            letivo.
          </TextoPequeno>
        </VerticalCentralizado>
      </LinhaBootstrap>
    )
  );
}

TextoInformativo.propTypes = {
  dataTermino: PropTypes.oneOfType([PropTypes.object, PropTypes.any]),
};

TextoInformativo.defaultProps = {
  dataTermino: {},
};

export default TextoInformativo;
