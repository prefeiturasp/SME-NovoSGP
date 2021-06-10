import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import GraficoQuantidadeAusenciasPossuemJustificativa from './graficoQuantidadeAusenciasPossuemJustificativa';

const QuantidadeAusenciasPossuemJustificativa = props => {
  const { anoLetivo, dreId, ueId, modalidade, semestre } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-ausencias-possuem-justificativa';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de ausências que possuem justificativa"
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
          <GraficoQuantidadeAusenciasPossuemJustificativa
            anoLetivo={anoLetivo}
            dreId={dreId}
            ueId={ueId}
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

QuantidadeAusenciasPossuemJustificativa.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
};

QuantidadeAusenciasPossuemJustificativa.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
};

export default QuantidadeAusenciasPossuemJustificativa;
