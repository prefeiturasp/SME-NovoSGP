import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import SelectList from '~/componentes/selectList';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import SelectAutocomplete from '~/componentes/select-autocomplete';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import Auditoria from '~/componentes/auditoria';
import api from '~/servicos/api';
import { erro, sucesso } from '~/servicos/alertas';
import Alert from '~/componentes/alert';

const AtribuicaoSupervisorCadastro = () => {
  const [listaDres, setListaDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState('');

  const [listaSupervisores, setListaSupervisores] = useState([]);
  const [supervisorSelecionado, setSupervisorSelecionado] = useState('');

  const [listaUES, setListaUES] = useState([]);
  const [uesSelecionadas, setUESSelecionadas] = useState([]);

  function exibeErro(erros) {
    if (erros && erros.response && erros.response.data)
      erros.response.data.mensagens.forEach(mensagem => erro(mensagem));
  }

  useEffect(() => {
    async function obterListaDres() {
      await api
        .get('v1/dres')
        .then(resposta => {
          setListaDres(resposta.data);
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }
    obterListaDres();
  }, []);

  useEffect(() => {
    async function obterListaUESAtribuidas() {
      await api
        .get(`v1/supervisores/${supervisorSelecionado}/dre/${dreSelecionada}`)
        .then(resposta => {
          if (resposta.data && resposta.data.length) {
            const ues = [
              ...listaUES,
              ...resposta.data[0].escolas.map(c => ({ ...c, key: c.codigo })),
            ];
            setListaUES(ues);

            setUESSelecionadas(resposta.data[0].escolas.map(e => e.codigo));
          } else setUESSelecionadas([]);
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }
    if (supervisorSelecionado) obterListaUESAtribuidas();
    else setUESSelecionadas([]);
  }, [supervisorSelecionado]);

  useEffect(() => {
    async function obterListaSupervisores() {
      const url = `v1/supervisores/dre/${dreSelecionada}`;
      await api
        .get(url)
        .then(resposta => {
          setListaSupervisores(resposta.data);
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }

    async function obterListaUES() {
      const url = `v1/dres/${dreSelecionada}/ues/sem-atribuicao`;
      await api
        .get(url)
        .then(resposta => {
          if (resposta.data)
            setListaUES(resposta.data.map(c => ({ ...c, key: c.codigo })));
          else erro('Nenhuma UE sem atribuição para a DRE selecionada.');
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }
    if (dreSelecionada) {
      obterListaSupervisores();
      obterListaUES();
    }
  }, [dreSelecionada]);

  function handleChange(targetKeys) {
    setUESSelecionadas(targetKeys);
  }

  function selecionaDre(idDre) {
    setDreSelecionada(idDre);
  }

  function selecionaSupervisor(idSupervisor) {
    setSupervisorSelecionado(idSupervisor);
  }

  function cancelarAlteracoes() {}

  async function salvarAtribuicao() {
    const atribuicao = {
      dreId: dreSelecionada,
      supervisorId: supervisorSelecionado,
      uesIds: uesSelecionadas,
    };
    await api
      .post('v1/supervisores/atribuir-ue', atribuicao)
      .then(resposta => sucesso('Atribuição realizada com sucesso.'))
      .catch(erros => {
        exibeErro(erros);
      });
  }
  const notificacoes = useSelector(state => state.notificacoes);

  return (
    <>
      <h3>Atribuição de Supervisor</h3>
      <div className="col-md-12">
        {notificacoes.alertas.map(alerta => (
          <Alert alerta={alerta} key={alerta.id} />
        ))}
      </div>
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
          <Button
            label="Salvar"
            color={Colors.Roxo}
            border
            bold
            onClick={salvarAtribuicao}
          />
        </div>
        <div className="col-xs-12 col-md-6 col-lg-6 mb-4">
          <SelectComponent
            label="SELECIONE A DRE:"
            className="col-md-12"
            name="dre"
            id="dre"
            lista={listaDres}
            valueOption="id"
            valueText="nome"
            onChange={selecionaDre}
            valueSelect={dreSelecionada}
          />
        </div>
        <div className="col-xs-12 col-md-6 col-lg-6 mb-4">
          <SelectAutocomplete
            label="SUPERVISOR:"
            className="col-md-12"
            name="dre"
            id="dre"
            lista={listaSupervisores}
            valueField="supervisorId"
            textField="supervisorNome"
            onChange={selecionaSupervisor}
            valueSelect={supervisorSelecionado}
          />
        </div>
        <SelectList
          id="select-list-supervisor"
          dados={listaUES}
          targetKeys={uesSelecionadas}
          handleChange={handleChange}
          titulos={["UE'S SEM ATRIBUIÇÃO", "UE'S ATRIBUIDAS AO SUPERVISOR"]}
          texto="nome"
          codigo="codigo"
          // selecionados={uesSelecionadas}
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
