import PropTypes from 'prop-types';
import React from 'react';
import QuantidadeCriancasEstudantesPlanoVigente from './QuantidadeCriancasEstudantesPlanoVigente/quantidadeCriancasEstudantesPlanoVigente';
import QuantidadePlanosSituacao from './QuantidadePlanosSituacao/quantidadePlanosSituacao';
import QuantidadePlanosUtilizamRecursosAcessibilidadeSalaRegularSRM from './QuantidadePlanosUtilizamRecursosAcessibilidadeSalaRegularSRM/quantidadePlanosUtilizamRecursosAcessibilidadeSalaRegularSRM';

const GraficosPlanos = props => {
  const { anoLetivo, dreId, ueId } = props;
  return (
    <>
      <QuantidadePlanosSituacao
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
      />
      <QuantidadeCriancasEstudantesPlanoVigente
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
      />
      <QuantidadePlanosUtilizamRecursosAcessibilidadeSalaRegularSRM
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
      />
    </>
  );
};

GraficosPlanos.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

GraficosPlanos.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default GraficosPlanos;
