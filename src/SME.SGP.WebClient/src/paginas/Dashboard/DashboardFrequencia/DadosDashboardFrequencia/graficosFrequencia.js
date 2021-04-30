import PropTypes from 'prop-types';
import React from 'react';
import FrequenciaGlobalPorAno from './FrequenciaGlobalPorAno/frequenciaGlobalPorAno';
import FrequenciaGlobalPorDRE from './FrequenciaGlobalPorDRE/frequenciaGlobalPorDRE';
import QuantidadeAusenciasPossuemJustificativa from './QuantidadeAusenciasPossuemJustificativa/quantidadeAusenciasPossuemJustificativa';
import QuantidadeJustificativasPorMotivo from './QuantidadeJustificativasPorMotivo/quantidadeJustificativasPorMotivo';

const GraficosFrequencia = props => {
  const { anoLetivo, dreId, ueId, modalidade, semestre } = props;
  return (
    <>
      <FrequenciaGlobalPorAno
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      <FrequenciaGlobalPorDRE
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      <QuantidadeAusenciasPossuemJustificativa
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      <QuantidadeJustificativasPorMotivo
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
    </>
  );
};

GraficosFrequencia.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

GraficosFrequencia.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
};

export default GraficosFrequencia;
