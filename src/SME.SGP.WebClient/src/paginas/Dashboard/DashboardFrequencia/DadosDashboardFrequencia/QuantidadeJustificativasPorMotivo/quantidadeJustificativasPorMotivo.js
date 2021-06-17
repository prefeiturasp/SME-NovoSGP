import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import GraficoQuantidadeJustificativasPorMotivo from './graficoQuantidadeJustificativasPorMotivo';

const QuantidadeJustificativasPorMotivo = props => {
  const { anoLetivo, dreId, ueId, modalidade, semestre, codigoUe } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-justificativas-por-motivo';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de justificativas por motivo"
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
          <GraficoQuantidadeJustificativasPorMotivo
            anoLetivo={anoLetivo}
            dreId={dreId}
            ueId={ueId}
            modalidade={modalidade}
            semestre={semestre}
            codigoUe={codigoUe}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadeJustificativasPorMotivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  modalidade: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  semestre: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  codigoUe: PropTypes.string,
};

QuantidadeJustificativasPorMotivo.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  modalidade: null,
  semestre: null,
  codigoUe: '',
};

export default QuantidadeJustificativasPorMotivo;
