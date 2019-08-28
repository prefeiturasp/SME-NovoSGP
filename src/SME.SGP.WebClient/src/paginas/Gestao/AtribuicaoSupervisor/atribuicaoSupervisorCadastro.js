import React, { useEffect, useState } from 'react';
import SelectList from '../../../componentes/selectList';

export default function AtribuicaoSupervisorCadastro() {
  const [listaUES, setListaUES] = useState([]);
  const [listaUESSelecionadas, setListaUESSelecionadas] = useState([]);

  useEffect(() => {
    function carregarListas() {
      // TODO - Mock
      setListaUES([
        {
          key: 1,
          titulo: '000',
          descricao: 'teste 0',
        },
        {
          key: 2,
          titulo: '001',
          descricao: 'teste 1',
        },
      ]);
    }

    carregarListas();
  }, []);

  function handleChange(targetKeys) {
    console.log(targetKeys);
    setListaUESSelecionadas(targetKeys);
  }

  return (
    <>
      <SelectList
        id="select-list-supervisor"
        mockData={listaUES}
        targetKeys={listaUESSelecionadas}
        handleChange={handleChange}
        titulos={["UE'S SEM ATRIBUIÇÃO", "UE'S ATRIBUIDAS AO SUPERVISOR"]}
      />
    </>
  );
}
