import React from 'react';
import shortid from 'shortid';

import RegistroIndividualCollapse from './registroIndividualCollapse';

const DadosRegistroIndividual = () => {
  const items = [
    {
      titulo: 'Novo registro individual',
    },
    {
      titulo: 'Registros anteriores',
    },
  ];

  return (
    <>
      {items.map(({ titulo }) => (
        <div key={shortid.generate()} className="px-4 pt-4">
          <RegistroIndividualCollapse titulo={titulo} />
        </div>
      ))}
    </>
  );
};

export default DadosRegistroIndividual;
