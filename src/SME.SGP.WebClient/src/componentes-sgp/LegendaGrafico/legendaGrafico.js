import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerLegendaGrafico } from './legendaGrafico.css';

const LegendaGrafico = props => {
  const { orizontal, dados } = props;

  const TAMANHO_PADRAO_COLUNA = '3';

  const [tamanhoColuna, setTamanhoColuna] = useState('');

  useEffect(() => {
    let valorAtual = TAMANHO_PADRAO_COLUNA;

    if (orizontal) {
      switch (dados?.length) {
        case 1:
          valorAtual = '12';
          break;
        case 2:
          valorAtual = '6';
          break;
        case 3:
          valorAtual = '4';
          break;

        default:
          break;
      }
    } else {
      valorAtual = '12';
    }

    setTamanhoColuna(valorAtual);
  }, [dados, orizontal]);

  return dados?.length ? (
    <ContainerLegendaGrafico
      className="row"
      style={{ display: !orizontal ? 'inline-table' : '' }}
    >
      {dados.map(item => {
        return (
          <div
            className={`col-md-${tamanhoColuna} ${
              orizontal && dados.length <= 3 ? 'legenda-centralizada' : ''
            }`}
          >
            <div className="legenda-container-conteudo">
              <div className="cor-legenda">
                <span style={{ backgroundColor: item.color }} />
              </div>
              <div className="label-valor">
                <div className="label-valor-porcentagem">
                  {item.radialLabel}
                </div>
                {item.label}
              </div>
            </div>
          </div>
        );
      })}
    </ContainerLegendaGrafico>
  ) : (
    ''
  );
};

LegendaGrafico.propTypes = {
  orizontal: PropTypes.bool,
  dados: PropTypes.oneOfType([PropTypes.array]),
};

LegendaGrafico.defaultProps = {
  orizontal: false,
  dados: null,
};

export default LegendaGrafico;
