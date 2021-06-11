import PropTypes from 'prop-types';
import React from 'react';
import QuantidadeEncaminhamentosSituacao from './QuantidadeEncaminhamentosSituacao/quantidadeEncaminhamentosSituacao';
import QuantidadeEstudantesEncaminhamentosDeferidos from './QuantidadeEstudantesEncaminhamentosDeferidos/quantidadeEstudantesEncaminhamentosDeferidos';
import QuantidadeEstudantesMatriculados from './QuantidadeEstudantesMatriculados/quantidadeEstudantesMatriculados';

const GraficosEncaminhamentos = props => {
  const { anoLetivo, dreId, ueId, dreCodigo, ueCodigo } = props;
  return (
    <>
      <QuantidadeEncaminhamentosSituacao
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
      />
      <QuantidadeEstudantesEncaminhamentosDeferidos
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
      />
      <QuantidadeEstudantesMatriculados
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        dreCodigo={dreCodigo}
        ueCodigo={ueCodigo}
      />
    </>
  );
};

GraficosEncaminhamentos.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
  dreCodigo: PropTypes.string,
  ueCodigo: PropTypes.string,
};

GraficosEncaminhamentos.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
  dreCodigo: '',
  ueCodigo: '',
};

export default GraficosEncaminhamentos;
