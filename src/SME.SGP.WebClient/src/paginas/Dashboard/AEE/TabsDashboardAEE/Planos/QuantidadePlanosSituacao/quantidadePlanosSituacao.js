import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';

const QuantidadePlanosSituacao = props => {
  const { anoLetivo, dreId, ueId } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-planos-situacao';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de planos por situação"
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
          <MontarGraficoBarras
            anoLetivo={anoLetivo}
            dreId={dreId}
            ueId={ueId}
            nomeIndiceDesc="descricaoSituacao"
            nomeValor="quantidade"
            ServicoObterValoresGrafico={
              ServicoDashboardAEE.obterSituacoesPlanos
            }
            exibirLegenda
            showAxisBottom={false}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadePlanosSituacao.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

QuantidadePlanosSituacao.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default QuantidadePlanosSituacao;
