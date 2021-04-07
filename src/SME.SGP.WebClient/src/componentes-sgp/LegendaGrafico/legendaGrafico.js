import PropTypes from 'prop-types';
import React from 'react';
import shortid from 'shortid';
import { ContainerLegendaGrafico } from './legendaGrafico.css';

const LegendaGrafico = props => {
  const { orizontal, dados, styleLegendaLabel } = props;

  return dados?.length ? (
    <ContainerLegendaGrafico orizontal={orizontal}>
      <div className="legenda-container">
        <ul className="legenda-labels" style={styleLegendaLabel}>
          {dados.map(item => {
            return (
              <li key={shortid.generate()}>
                <div className="legenda-container-conteudo">
                  <div>
                    <span style={{ backgroundColor: item.color }} />
                  </div>
                  <div>
                    <div className="label-valor-porcentagem">
                      {item.radialLabel}
                    </div>
                    {item.label}
                  </div>
                </div>
              </li>
            );
          })}
        </ul>
      </div>
    </ContainerLegendaGrafico>
  ) : (
    ''
  );
};

LegendaGrafico.propTypes = {
  orizontal: PropTypes.bool,
  dados: PropTypes.oneOfType([PropTypes.array]),
  styleLegendaLabel: PropTypes.oneOfType(PropTypes.any),
};

LegendaGrafico.defaultProps = {
  orizontal: false,
  dados: null,
  styleLegendaLabel: null,
};

export default LegendaGrafico;
