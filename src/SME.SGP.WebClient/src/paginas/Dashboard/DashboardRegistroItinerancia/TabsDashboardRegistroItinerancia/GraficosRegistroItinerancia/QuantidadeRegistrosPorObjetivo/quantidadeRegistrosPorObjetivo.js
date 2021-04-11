import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import ServicoDashboardRegistroItinerancia from '~/servicos/Paginas/Dashboard/ServicoDashboardRegistroItinerancia';

const QuantidadeRegistrosPorObjetivo = props => {
  const { anoLetivo, dreId, ueId } = props;

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
          <MontarGraficoBarras
            anoLetivo={anoLetivo}
            dreId={dreId}
            ueId={ueId}
            nomeIndiceDesc="descricao"
            nomeValor="quantidade"
            ServicoObterValoresGrafico={
              ServicoDashboardRegistroItinerancia.obterQuantidadeRegistrosPorObjetivo
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

QuantidadeRegistrosPorObjetivo.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreId: PropTypes.string,
  ueId: PropTypes.string,
};

QuantidadeRegistrosPorObjetivo.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default QuantidadeRegistrosPorObjetivo;
