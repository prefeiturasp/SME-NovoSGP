import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import ServicoDashboardRegistroItinerancia from '~/servicos/Paginas/Dashboard/ServicoDashboardRegistroItinerancia';

const QuantidadeRegistrosPAAI = props => {
  const { anoLetivo, dreId, ueId } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-registros-paai';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de registros/visitas por PAAI"
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
            nomeIndiceDesc="descricao"
            nomeValor="quantidade"
            ServicoObterValoresGrafico={
              ServicoDashboardRegistroItinerancia.obterQuantidadeRegistrosPAAI
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

QuantidadeRegistrosPAAI.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

QuantidadeRegistrosPAAI.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default QuantidadeRegistrosPAAI;
