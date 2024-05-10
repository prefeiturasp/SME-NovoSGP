import React from 'react';
import { useSelector } from 'react-redux';
import { RegistroMigrado } from '~/componentes-sgp';

const MarcadorMigrado = () => {
  const ehRegistroMigrado = useSelector(
    store => store.planoAnual.ehRegistroMigrado
  );

  return (
    <>
      {ehRegistroMigrado ? (
        <RegistroMigrado>Registro Migrado</RegistroMigrado>
      ) : (
        ''
      )}
    </>
  );
};

export default MarcadorMigrado;
