import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import CardPlanejamento from '~/paginas/DiarioClasse/Devolutivas/DadosPlanejamentoDiarioBordo/CardPlanejamento/cardPlanejamento';
import BarraNavegacaoPlanejamento from '~/paginas/DiarioClasse/Devolutivas/DadosPlanejamentoDiarioBordo/BarraNavegacaoPlanejamento/barraNavegacaoPlanejamento';

const DadosPlanejamentoDiarioBordo = React.memo(props => {
  const { onChangePage } = props;

  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  return (
    <>
      {dadosPlanejamentos ? (
        <>
          <BarraNavegacaoPlanejamento onChangePage={onChangePage} />
          <CardPlanejamento />
        </>
      ) : (
        ''
      )}
    </>
  );
});

DadosPlanejamentoDiarioBordo.propTypes = {
  onChangePage: PropTypes.func,
};

DadosPlanejamentoDiarioBordo.defaultProps = {
  onChangePage: () => {},
};

export default DadosPlanejamentoDiarioBordo;
