import PropTypes from 'prop-types';
import React from 'react';
import QuantidadeEencaminhamentosSituacao from './QuantidadeEencaminhamentosSituacao/quantidadeEncaminhamentosSituacao';

const GraficosEncaminhamentos = props => {
  const { anoLetivo, codigoDre, codigoUe } = props;
  return (
    <>
      <QuantidadeEencaminhamentosSituacao
        anoLetivo={anoLetivo}
        codigoDre={codigoDre}
        codigoUe={codigoUe}
      />
    </>
  );
};

GraficosEncaminhamentos.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  codigoDre: PropTypes.string,
  codigoUe: PropTypes.string,
};

GraficosEncaminhamentos.defaultProps = {
  anoLetivo: null,
  codigoDre: '',
  codigoUe: '',
};

export default GraficosEncaminhamentos;
