import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import BimestreCardCollapse from './bimestreCardCollapse';

const BimestresPlanoAnual = () => {
  const bimestresPlanoAnual = useSelector(
    store => store.planoAnual.bimestresPlanoAnual
  );

  return (
    <>
      {bimestresPlanoAnual && bimestresPlanoAnual.length
        ? bimestresPlanoAnual.map(item => {
            return (
              <div key={shortid.generate()} className="mb-4">
                <BimestreCardCollapse dadosBimestre={item} />
              </div>
            );
          })
        : ''}
    </>
  );
};

export default BimestresPlanoAnual;
