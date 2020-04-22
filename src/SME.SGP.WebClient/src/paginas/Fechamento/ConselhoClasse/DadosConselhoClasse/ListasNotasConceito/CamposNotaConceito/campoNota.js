import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';

const CampoNota = props => {
  const { notaPosConselho } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  return (
    <CampoNumero
      onChange={setNotaValorAtual}
      value={notaValorAtual}
      min={0}
      max={10}
      step={0.5}
    />
  );
};

CampoNota.defaultProps = {
  notaPosConselho: PropTypes.string,
};

CampoNota.propTypes = {
  notaPosConselho: '',
};

export default CampoNota;
