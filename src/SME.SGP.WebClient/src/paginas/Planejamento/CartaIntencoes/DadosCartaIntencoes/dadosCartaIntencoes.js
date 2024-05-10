import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import BimestresCartaIntencoes from './BimestresCartaIntencoes/bimestresCartaIntencoes';

const DadosCartaIntencoes = props => {
  const { permissoesTela, somenteConsulta } = props;

  const dadosCartaIntencoes = useSelector(
    store => store.cartaIntencoes.dadosCartaIntencoes
  );

  return (
    <>
      {dadosCartaIntencoes && dadosCartaIntencoes.length
        ? dadosCartaIntencoes.map(item => {
            return (
              <div key={shortid.generate()} className="mb-4">
                <BimestresCartaIntencoes
                  carta={item}
                  permissoesTela={permissoesTela}
                  somenteConsulta={somenteConsulta}
                />
              </div>
            );
          })
        : ''}
    </>
  );
};

DadosCartaIntencoes.propTypes = {
  permissoesTela: PropTypes.oneOfType([PropTypes.object]),
  somenteConsulta: PropTypes.bool,
};

DadosCartaIntencoes.defaultProps = {
  permissoesTela: {},
  somenteConsulta: false,
};

export default DadosCartaIntencoes;
