import React, { useEffect, useState } from 'react';
import SelectList from '~/componentes/selectList';
import Card from '~/componentes/card';
import SelectComponent from 'componentes/select';

export default function AtribuicaoSupervisorCadastro() {
  const [listaUES, setListaUES] = useState([]);
  const [listaDres, setListaDres] = useState([{ id: 1, descricao: 'teste' }]);
  const [listaUESSelecionadas, setListaUESSelecionadas] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState([]);

  useEffect(() => {
    function carregarListas() {
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
    console.log(dreSelecionada);

    carregarListas();
  }, []);

  function handleChange(targetKeys) {
    setListaUESSelecionadas(targetKeys);
  }

  function selecionaDre(e) {
    setDreSelecionada(e);
  }
  return (
    <>
      <h3>Atribuição de Supervisor</h3>
      <Card>
        <div className="col-xs-12 col-md-6 col-lg-6">
          <SelectComponent
            label="SELECIONE A DRE:"
            className="col-md-12"
            name="dre"
            id="dre"
            lista={listaDres}
            valueOption="id"
            valueText="descricao"
            onChange={selecionaDre}
            valueSelect={dreSelecionada}
          />
        </div>
        <div className="col-xs-12 col-md-6 col-lg-6">
          <SelectComponent
            label="SUPERVISOR:"
            className="col-md-12"
            name="dre"
            id="dre"
            lista={listaDres}
            valueOption="id"
            valueText="descricao"
            onChange={selecionaDre}
            valueSelect={dreSelecionada}
          />
        </div>
        <SelectList
          id="select-list-supervisor"
          mockData={listaUES}
          targetKeys={listaUESSelecionadas}
          handleChange={handleChange}
          titulos={["UE'S SEM ATRIBUIÇÃO", "UE'S ATRIBUIDAS AO SUPERVISOR"]}
        />
      </Card>
    </>
  );
}
