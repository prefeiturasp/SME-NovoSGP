import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';

const QuantidadeEncaminhamentosSituacao = props => {
  const { anoLetivo, dreId, ueId } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-encaminhamentos-situacao';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de encaminhamentos por situação"
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
          <div className="col-md-12">
            <MontarGraficoBarras
              anoLetivo={anoLetivo}
              dreId={dreId}
              ueId={ueId}
              nomeIndiceDesc="descricaoSituacao"
              nomeValor="quantidade"
              ServicoObterValoresGrafico={
                ServicoDashboardAEE.obterQuantidadeEncaminhamentosPorSituacao
              }
              exibirLegenda
              showAxisBottom={false}
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
