import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import SelectList from '~/componentes/selectList';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import Auditoria from '~/componentes/auditoria';

const AtribuicaoSupervisorCadastro = () => {
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

    carregarListas();
  }, []);

  function handleChange(targetKeys) {
    setListaUESSelecionadas(targetKeys);
  }

  function selecionaDre(e) {
    setDreSelecionada(e);
  }

  function cancelarAlteracoes() {}
  return (
    <>
      <h3>Atribuição de Supervisor</h3>
      <Card>
        <div className="col-xs-12 col-md-6 col-lg-12 d-flex justify-content-end mb-4">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-3"
          />
          <Button
            label="Cancelar"
            color={Colors.Roxo}
            border
            bold
            className="mr-3"
            onClick={cancelarAlteracoes}
          />
          <Button label="Salvar" color={Colors.Roxo} border bold disabled />
        </div>
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
        <Auditoria
          inserido="INSERIDO por ELISANGELA DOS SANTOS ARRUDA em 02/05/2019 às 20:28"
          alterado="ALTERADO por JOÃO DA SILVA em 02/05/2019 às 20:28"
        />
      </Card>
    </>
  );
};

export default AtribuicaoSupervisorCadastro;
