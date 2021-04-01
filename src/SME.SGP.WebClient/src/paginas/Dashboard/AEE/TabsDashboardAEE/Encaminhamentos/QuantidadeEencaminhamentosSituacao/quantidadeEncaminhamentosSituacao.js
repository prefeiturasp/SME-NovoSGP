import PropTypes from 'prop-types';
import React from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import GraficoQuantidadeEncaminhamentosSituacao from './graficoQuantidadeEncaminhamentosSituacao';

const QuantidadeEncaminhamentosSituacao = props => {
  const { anoLetivo, codigoDre, codigoUe } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  return (
    <div className="mt-3">
      <CardCollapse
        key="quantidade-encaminhamentos-situacao-collapse-key"
        titulo="Quantidade de encaminhamentos por situação"
        indice="quantidade-encaminhamentos-situacao-collapse-indice"
        alt="quantidade-encaminhamentos-situacaoe-alt"
        configCabecalho={configCabecalho}
      >
        <div className="col-md-12">
          <GraficoQuantidadeEncaminhamentosSituacao
            anoLetivo={anoLetivo}
            codigoDre={codigoDre}
            codigoUe={codigoUe}
          />
        </div>
      </CardCollapse>
    </div>
  );
};

QuantidadeEncaminhamentosSituacao.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  codigoDre: PropTypes.string,
  codigoUe: PropTypes.string,
};

QuantidadeEncaminhamentosSituacao.defaultProps = {
  anoLetivo: null,
  codigoDre: '',
  codigoUe: '',
};

export default QuantidadeEncaminhamentosSituacao;
