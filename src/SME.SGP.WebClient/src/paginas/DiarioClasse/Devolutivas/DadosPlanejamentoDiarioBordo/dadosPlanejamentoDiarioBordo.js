import React from 'react';
import { useSelector } from 'react-redux';
import CardPlanejamento from '~/paginas/DiarioClasse/Devolutivas/DadosPlanejamentoDiarioBordo/CardPlanejamento/cardPlanejamento';

const DadosPlanejamentoDiarioBordo = () => {
  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  return (
    <>
      {dadosPlanejamentos ? (
        // TODO Monar a barra de navegação!
        <CardPlanejamento dados={dadosPlanejamentos.itens} />
      ) : (
        ''
      )}
    </>
  );
};

export default DadosPlanejamentoDiarioBordo;
