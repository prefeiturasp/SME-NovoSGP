import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import BimestresCartaIntencoes from './BimestresCartaIntencoes/bimestresCartaIntencoes';

const DadosCartaIntencoes = () => {
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
                  descricao={item.descricao}
                  bimestre={item.bimestre}
                  auditoria={item.auditoria}
                />
              </div>
            );
          })
        : ''}
    </>
  );
};

export default DadosCartaIntencoes;
