import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import GraficoFrequenciaGlobalPorDRE from './graficoFrequenciaGlobalPorDRE';

const FrequenciaGlobalPorDRE = props => {
  const { anoLetivo, modalidade, semestre } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'Frequencia-global-por-dre';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Frequência global por DRE"
        key={`${key}-collapse-key`}
        indice={`${key}-collapse-indice`}
        alt={`${key}-alt`}
        configCabecalho={configCabecalho}
        show={exibir}
        onClick={() => {
          setExibir(!exibir);
        }}
      >
        {exibir ? (
          <GraficoFrequenciaGlobalPorDRE
            anoLetivo={anoLetivo}
            modalidade={modalidade}
            semestre={semestre}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

FrequenciaGlobalPorDRE.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

FrequenciaGlobalPorDRE.defaultProps = {
  anoLetivo: null,
  modalidade: null,
  semestre: null,
};

export default FrequenciaGlobalPorDRE;
