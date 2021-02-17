import React, { useState } from 'react';
import { CardCollapse } from '~/componentes';
import ReestruturacaoTabela from '../ReestruturacaoTabela/reestruturacaoTabela';

const SecaoReesturutracaoPlano = () => {
  const [listaPrimeiroSemestre, setListaPrimeiroSemestre] = useState([]);
  const [listaSegundoSemestre, setListaSegundoSemestre] = useState([]);
  const keyPrimeiroSemestre = 'secao-1-semestre-plano-collapse';
  const keySegundoSemestre = 'secao-2-semestre-plano-collapse';

  return (
    <>
      <CardCollapse
        key={`${keyPrimeiroSemestre}-key`}
        titulo="Reestruturações do 1º Semestre"
        show
        indice={`${keyPrimeiroSemestre}-indice`}
        alt="secao-informacoes-plano-alt"
      >
        <ReestruturacaoTabela
          key={keyPrimeiroSemestre}
          listaDados={listaPrimeiroSemestre}
          setListaDados={setListaPrimeiroSemestre}
        />
      </CardCollapse>
      <CardCollapse
        key={`${keySegundoSemestre}-key`}
        titulo="Reestruturações do 2º Semestre"
        show
        indice={`${keySegundoSemestre}-indice`}
        alt="secao-informacoes-plano-alt"
      >
        <ReestruturacaoTabela
          key={keySegundoSemestre}
          listaDados={listaSegundoSemestre}
          setListaDados={setListaSegundoSemestre}
        />
      </CardCollapse>
    </>
  );
};

export default SecaoReesturutracaoPlano;
