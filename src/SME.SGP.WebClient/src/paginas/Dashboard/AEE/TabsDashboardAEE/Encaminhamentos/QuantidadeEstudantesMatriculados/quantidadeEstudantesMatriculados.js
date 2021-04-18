import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import MontarGraficoBarras from '~/paginas/Dashboard/ComponentesDashboard/montarGraficoBarras';
import ServicoDashboardAEE from '~/servicos/Paginas/Dashboard/ServicoDashboardAEE';

const QuantidadeEstudantesMatriculados = props => {
  const { anoLetivo, ueCodigo, dreCodigo } = props;

  const configCabecalho = {
    altura: '44px',
    corBorda: Base.AzulBordaCollapse,
  };

  const [exibir, setExibir] = useState(false);

  const key = 'quantidade-estudantes-matriculados';

  return (
    <div className="mt-3">
      <CardCollapse
        titulo="Quantidade de estudantes matriculados em SRM ou PAEE colaborativo"
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
              dreCodigo={dreCodigo}
              ueCodigo={ueCodigo}
              chavesGraficoAgrupado={[
                { nomeChave: 'quantidadeSRM', legenda: 'legendaSRM' },
                { nomeChave: 'quantidadePAEE', legenda: 'legendaPAEE' },
              ]}
              nomeIndiceDesc="descricao"
              ServicoObterValoresGrafico={
                ServicoDashboardAEE.obterQuantidadeEstudantesMatriculados
              }
            />
          </div>
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

QuantidadeEstudantesMatriculados.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  dreCodigo: PropTypes.string,
  ueCodigo: PropTypes.string,
};

QuantidadeEstudantesMatriculados.defaultProps = {
  anoLetivo: null,
  dreCodigo: '',
  ueCodigo: '',
};

export default QuantidadeEstudantesMatriculados;
