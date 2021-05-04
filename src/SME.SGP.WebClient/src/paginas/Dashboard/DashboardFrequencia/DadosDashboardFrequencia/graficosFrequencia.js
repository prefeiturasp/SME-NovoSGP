import PropTypes from 'prop-types';
import React from 'react';
import FrequenciaGlobalPorAno from './FrequenciaGlobalPorAno/frequenciaGlobalPorAno';
import FrequenciaGlobalPorDRE from './FrequenciaGlobalPorDRE/frequenciaGlobalPorDRE';
import QuantidadeAusenciasPossuemJustificativa from './QuantidadeAusenciasPossuemJustificativa/quantidadeAusenciasPossuemJustificativa';
import QuantidadeJustificativasPorMotivo from './QuantidadeJustificativasPorMotivo/quantidadeJustificativasPorMotivo';

const GraficosFrequencia = props => {
  const {
    anoLetivo,
    dreId,
    ueId,
    modalidade,
    semestre,
    listaAnosEscolares,
    codigoUe,
    consideraHistorico,
  } = props;

  const OPCAO_TODOS = '-99';

  return (
    <>
      <FrequenciaGlobalPorAno
        anoLetivo={anoLetivo}
        dreId={dreId}
        ueId={ueId}
        modalidade={modalidade}
        semestre={semestre}
      />
      {dreId === OPCAO_TODOS && ueId === OPCAO_TODOS && (
        <FrequenciaGlobalPorDRE
          anoLetivo={anoLetivo}
          modalidade={modalidade}
          semestre={semestre}
          listaAnosEscolares={listaAnosEscolares}
        />
      )}
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
        listaAnosEscolares={listaAnosEscolares}
        codigoUe={codigoUe}
        consideraHistorico={consideraHistorico}
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
  listaAnosEscolares: PropTypes.oneOfType(PropTypes.array),
  codigoUe: PropTypes.string,
  consideraHistorico: PropTypes.bool,
};

GraficosFrequencia.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
  listaAnosEscolares: [],
  codigoUe: '',
  consideraHistorico: false,
};

export default GraficosFrequencia;
