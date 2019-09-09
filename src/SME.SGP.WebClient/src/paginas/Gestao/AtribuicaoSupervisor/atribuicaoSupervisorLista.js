import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import history from '../../../servicos/history';

import Button from '../../../componentes/button';
import Card from '../../../componentes/card';
import CheckboxComponent from '../../../componentes/checkbox';
import { Colors } from '../../../componentes/colors';
import SelectComponent from '../../../componentes/select';
import DataTable from '../../../componentes/table/dataTable';
import api from '../../../servicos/api';
import { NovoSGP, Titulo } from './atribuicaoSupervisor.css';

export default function AtribuicaoSupervisorLista() {
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [uesSemSupervisorCheck, setUesSemSupervisorCheck] = useState(false);
  const [assumirFiltroPrincCheck, setAssumirFiltroPrincCheck] = useState(false);
  const [dresSelecionadas, setDresSelecionadas] = useState('');
  const [supervisoresSelecionados, setSupervisoresSelecionados] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaSupervisores, setListaSupervisores] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [ueSelecionada, setUeSelecionada] = useState('');
  const [listaFiltroAtribuicao, setListaFiltroAtribuicao] = useState([]);
  const [desabilitarSupervisor, setDesabilitarSupervisor] = useState(false);
  const [desabilitarUe, setDesabilitarUe] = useState(false);
  const [desabilitarDre, setDesabilitarDre] = useState(false);
  const [
    desabilitarAssumirFiltroPrincipal,
    setDesabilitarAssumirFiltroPrincipal,
  ] = useState(true);

  const usuario = useSelector(store => store.usuario);

  useEffect(() => {
    if (
      usuario &&
      usuario.turmaSelecionada &&
      usuario.turmaSelecionada.length
    ) {
      setDesabilitarAssumirFiltroPrincipal(false);
    } else {
      setDesabilitarAssumirFiltroPrincipal(true);
    }
  }, [usuario.turmaSelecionada]);

  useEffect(() => {
    async function carregarDres() {
      const dres = await api.get('v1/dres');
      setListaDres(dres.data);
    }
    carregarDres();
  }, []);

  useEffect(() => {
    if (listaUes && listaUes.length && assumirFiltroPrincCheck) {
      setUeSelecionada(usuario.turmaSelecionada[0].codEscola);
      onChangeUes(usuario.turmaSelecionada[0].codEscola);
    }
  }, [listaUes]);

  useEffect(() => {
    if (uesSemSupervisorCheck) {
      montaListaUesSemSup();
    } else {
      setSupervisoresSelecionados([]);
      setUeSelecionada('');
      setDesabilitarSupervisor(false);
      setDesabilitarUe(false);
      onChangeDre(dresSelecionadas);
    }
  }, [uesSemSupervisorCheck]);

  const columns = [
    {
      title: 'dre',
      dataIndex: 'dre',
    },
    {
      title: 'escola',
      dataIndex: 'escola',
    },
    {
      title: 'supervisor',
      dataIndex: 'supervisor',
    },
  ];
  function onSelectRow(row) {
    console.log(row);
    setSelectedRowKeys(row);
  }

  function onClickVoltar() {
    history.push('/');
  }

  function onClickEditar() {
    const ueSelecionada = listaFiltroAtribuicao.find(
      ue => ue.key == selectedRowKeys[0]
    );
    history.push(`/gestao/atribuicao-supervisor/${dresSelecionadas}/${ueSelecionada.supervisorId || 0}`);
  }

  function onClickNovaAtribuicao() {
    history.push('/gestao/atribuicao-supervisor');
  }

  function onChangeUesSemSup(e) {
    if (e.target.checked) {
      setUesSemSupervisorCheck(true);
    } else {
      setUesSemSupervisorCheck(false);
    }
  }

  async function montaListaUesSemSup() {
    setSupervisoresSelecionados([]);
    setUeSelecionada('');
    setDesabilitarSupervisor(true);
    setDesabilitarUe(true);
    const vinculoEscolasDreSemAtrib = await api.get(
      `/v1/dres/${dresSelecionadas}/ues/sem-atribuicao`
    );
    const novaLista = [
      {
        escolas: vinculoEscolasDreSemAtrib.data,
      },
    ];
    montarListaAtribuicao(novaLista, dresSelecionadas, true);
  }

  function onChangeAssumirFiltroPrinc(e) {
    if (e.target.checked) {
      setAssumirFiltroPrincCheck(true);
      setUeSelecionada('');
      setListaFiltroAtribuicao([]);
      setListaSupervisores([]);
      setSupervisoresSelecionados([]);
      setListaUes([]);
      setDesabilitarSupervisor(true);
      setDesabilitarUe(true);
      setDesabilitarDre(true);
      setUesSemSupervisorCheck(false);

      carregarUes(usuario.turmaSelecionada[0].codDre);
      setDresSelecionadas(usuario.turmaSelecionada[0].codDre);
    } else {
      setAssumirFiltroPrincCheck(false);
      setDesabilitarSupervisor(false);
      setDesabilitarUe(false);
      setDesabilitarDre(false);
    }
  }

  async function onChangeDre(dre) {
    setListaSupervisores([]);
    setSupervisoresSelecionados([]);
    setListaUes([]);
    setUeSelecionada('');
    if (dre) {
      if (uesSemSupervisorCheck) {
        montaListaUesSemSup();
      } else {
        const vinculoEscolasDre = await api.get(
          `v1/supervisores/dre/${dre}/vinculo-escolas`
        );
        montarListaAtribuicao(vinculoEscolasDre.data, dre, true);
      }
    } else {
      setListaFiltroAtribuicao([]);
      setUesSemSupervisorCheck(false);
    }

    setDresSelecionadas(dre);
    if (dre) {
      carregarSupervisores(dre);
      carregarUes(dre);
    }
  }

  function montarListaAtribuicao(lista, dre, isArray) {
    if (lista) {
      if (isArray) {
        lista.forEach(item => {
          setListaFiltroAtribuicao(montarLista(item));
        });
      } else {
        setListaFiltroAtribuicao(montarLista(lista));
      }
    } else {
      setListaFiltroAtribuicao([]);
    }

    function montarLista(item) {
      const dreSelecionada = listaDres.find(d => d.id == dre);
      const dadosAtribuicao = [];
      let contId = 1;
      item.escolas.forEach(escola => {
        dadosAtribuicao.push({
          key: contId++,
          dre: dreSelecionada.sigla,
          escola: escola.nome,
          supervisor: item.supervisorNome || 'NÃO ATRIBUÍDO',
          supervisorId: item.supervisorId,
          className: item.supervisorId ? '' : 'supervisor-nao-atribuido',
        });
      });
      return dadosAtribuicao;
    }
  }

  async function carregarSupervisores(dre) {
    const sups = await api.get(`/v1/supervisores/dre/${dre}`);
    setListaSupervisores(sups.data || []);
  }

  async function carregarUes(dre) {
    const ues = await api.get(`/v1/dres/${dre}/ues`);
    setListaUes(ues.data || []);
  }

  function onChangeSupervisores(sup) {
    setSupervisoresSelecionados(sup);
    console.log(`onChangeSupervisores${sup}`);
  }

  async function onChangeUes(ue) {
    if (ue) {
      const vinculoUes = await api.get(`/v1/supervisores/ues/${ue}/vinculo`);
      montarListaAtribuicao(vinculoUes.data, dresSelecionadas, false);
      setDesabilitarSupervisor(true);
      setSupervisoresSelecionados([]);
      setUeSelecionada(ue);
    } else {
      setUeSelecionada('');
      setDesabilitarSupervisor(false);
      onChangeDre(dresSelecionadas);
    }
  }

  return (
    <>
      <div className="col-md-12">
        <NovoSGP>NOVO SGP</NovoSGP>
        <Titulo>Atribuição de Supervisor - Listagem</Titulo>
      </div>
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
          <Button
            label="Editar"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickEditar}
            hidden={selectedRowKeys && selectedRowKeys.length < 1}
          />
          <Button
            label="Nova Atribuição"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickNovaAtribuicao}
          />
        </div>

        <div className="col-md-6">
          <CheckboxComponent
            className="mb-2"
            label="Exibir apenas UE's sem supervisor"
            onChangeCheckbox={onChangeUesSemSup}
            disabled={!dresSelecionadas || assumirFiltroPrincCheck}
            checked={uesSemSupervisorCheck}
          />
          <CheckboxComponent
            label="Assumir o filtro principal do sistema"
            onChangeCheckbox={onChangeAssumirFiltroPrinc}
            checked={assumirFiltroPrincCheck}
            disabled={desabilitarAssumirFiltroPrincipal}
          />
        </div>
        <div className="col-md-6">
          <SelectComponent
            className="col-md-12"
            name="dres-atribuicao-sup"
            id="dres-atribuicao-sup"
            lista={listaDres}
            valueOption="id"
            valueText="nome"
            onChange={onChangeDre}
            valueSelect={dresSelecionadas}
            placeholder="SELECIONE A DRE"
            disabled={desabilitarDre}
          />
        </div>

        <div className="col-md-6">
          <SelectComponent
            className="col-md-12"
            name="supervisores-list"
            id="supervisores-list"
            lista={listaSupervisores}
            valueOption="supervisorId"
            valueText="supervisorNome"
            onChange={onChangeSupervisores}
            valueSelect={supervisoresSelecionados}
            multiple
            placeholder="SELECIONE O SUPERVISOR"
            disabled={desabilitarSupervisor}
          />
        </div>

        <div className="col-md-6">
          <SelectComponent
            className="col-md-12"
            name="ues-list"
            id="ues-list"
            lista={listaUes}
            valueOption="codigo"
            valueText="nome"
            onChange={onChangeUes}
            valueSelect={ueSelecionada}
            placeholder="SELECIONE A UE"
            disabled={desabilitarUe}
          />
        </div>

        <div className="col-md-12 pt-4">
          <DataTable
            selectedRowKeys={selectedRowKeys}
            onSelectRow={onSelectRow}
            columns={columns}
            dataSource={listaFiltroAtribuicao}
          />
        </div>
      </Card>
    </>
  );
}
