import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import GraficoQuantidadeEncaminhamentosSituacao from './graficoQuantidadeEncaminhamentosSituacao';

const QuantidadeEncaminhamentosSituacao = props => {
  const { anoLetivo, dreId, ueId } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  return (
    <div className="mt-3">
      <CardCollapse
        key="quantidade-encaminhamentos-situacao-collapse-key"
        titulo="Quantidade de encaminhamentos por situação"
        indice="quantidade-encaminhamentos-situacao-collapse-indice"
        alt="quantidade-encaminhamentos-situacaoe-alt"
        configCabecalho={configCabecalho}
        show={exibir}
        onClick={() => {
          setExibir(!exibir);
        }}
      >
        {exibir ? (
          <div className="col-md-12">
            <GraficoQuantidadeEncaminhamentosSituacao
              anoLetivo={anoLetivo}
              dreId={dreId}
              ueId={ueId}
            />
          </div>
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadeEncaminhamentosSituacao.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

QuantidadeEncaminhamentosSituacao.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default QuantidadeEncaminhamentosSituacao;
