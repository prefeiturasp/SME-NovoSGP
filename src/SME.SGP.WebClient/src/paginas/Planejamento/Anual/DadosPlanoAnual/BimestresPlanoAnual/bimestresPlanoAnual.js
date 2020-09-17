import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import BimestreCardCollapse from './bimestreCardCollapse';

const BimestresPlanoAnual = () => {
  const dadosBimestresPlanoAnual = useSelector(
    store => store.planoAnual.dadosBimestresPlanoAnual
  );

  return (
    <>
      {dadosBimestresPlanoAnual && dadosBimestresPlanoAnual.length
        ? dadosBimestresPlanoAnual.map(item => {
            return (
              <div key={shortid.generate()} className="mb-4">
                <BimestreCardCollapse dados={item} />
              </div>
            );
          })
        : ''}
    </>
  );
};

export default BimestresPlanoAnual;
