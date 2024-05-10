import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarDadosQuantidadeRegistrosPorObjetivo from './montarDadosQuantidadeRegistrosPorObjetivo';

const QuantidadeRegistrosPorObjetivo = props => {
  const { anoLetivo, dreId, ueId, mesSelecionado } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-registros-por-objetivo';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de registros por objetivo"
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
          <MontarDadosQuantidadeRegistrosPorObjetivo
            anoLetivo={anoLetivo}
            dreId={dreId}
            ueId={ueId}
            mesSelecionado={mesSelecionado}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadeRegistrosPorObjetivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  mesSelecionado: PropTypes.string,
};

QuantidadeRegistrosPorObjetivo.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  mesSelecionado: '',
};

export default QuantidadeRegistrosPorObjetivo;
