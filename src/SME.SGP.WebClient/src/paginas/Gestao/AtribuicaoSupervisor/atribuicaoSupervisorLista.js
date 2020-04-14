import React, { useEffect, useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import history from '../../../servicos/history';

import Button from '../../../componentes/button';
import Card from '../../../componentes/card';
import CheckboxComponent from '../../../componentes/checkbox';
import { Colors } from '../../../componentes/colors';
import SelectComponent from '../../../componentes/select';
import DataTable from '../../../componentes/table/dataTable';
import api from '../../../servicos/api';
import Cabecalho from '~/componentes-sgp/cabecalho';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';

export default function AtribuicaoSupervisorLista() {
  const [uesSemSupervisorCheck, setUesSemSupervisorCheck] = useState(false);
  const [assumirFiltroPrincCheck, setAssumirFiltroPrincCheck] = useState(false);
  const [dresSelecionadas, setDresSelecionadas] = useState('');
  const [supervisoresSelecionados, setSupervisoresSelecionados] = useState([]);
  const [listaDres, setListaDres] = useState([]);
  const [listaSupervisores, setListaSupervisores] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [ueSelecionada, setUeSelecionada] = useState([]);
  const [listaFiltroAtribuicao, setListaFiltroAtribuicao] = useState([]);
  const [desabilitarSupervisor, setDesabilitarSupervisor] = useState(false);
  const [desabilitarUe, setDesabilitarUe] = useState(false);
  const [desabilitarDre, setDesabilitarDre] = useState(false);
  const [
    desabilitarAssumirFiltroPrincipal,
    setDesabilitarAssumirFiltroPrincipal,
  ] = useState(true);

  const usuario = useSelector(store => store.usuario);
  const permissoesTela =
    usuario.permissoes[RotasDto.ATRIBUICAO_SUPERVISOR_LISTA];

  useEffect(() => {
    if (usuario && usuario.turmaSelecionada) {
      setDesabilitarAssumirFiltroPrincipal(false);
    } else {
      setDesabilitarAssumirFiltroPrincipal(true);
    }
  }, [usuario.turmaSelecionada]);

  useEffect(() => {
    async function carregarDres() {
      const dres = await api.get('v1/abrangencias/false/dres');
      setListaDres(dres.data.sort(FiltroHelper.ordenarLista('nome')));
    }
    carregarDres();
    verificaSomenteConsulta(permissoesTela);
  }, []);

  useEffect(() => {
    if (listaUes && listaUes.length && assumirFiltroPrincCheck) {
      setUeSelecionada(usuario.turmaSelecionada.unidadeEscolar);
      onChangeUes(usuario.turmaSelecionada.unidadeEscolar);
    }
  }, [listaUes]);

  useEffect(() => {
    if (uesSemSupervisorCheck) {
      montaListaUesSemSup(dresSelecionadas);
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
      title: 'DRE',
      dataIndex: 'dre',
      width: '15%',
    },
    {
      title: 'Unidade Escolar',
      dataIndex: 'escola',
      width: '55%',
    },
    {
      title: 'Supervisor',
      dataIndex: 'supervisor',
      width: '30%',
      render: text => {
        return text || <a className="texto-vermelho-negrito">NÃO ATRIBUIDO</a>;
      },
    },
  ];

  function onClickRow(row) {
    if (!permissoesTela.podeAlterar) return;

    onClickEditar(row.supervisorId);
  }

  function onClickVoltar() {
    history.push('/');
  }

  function onClickEditar(supervisorId) {
    if (!permissoesTela.podeAlterar) return;

    const path = `/gestao/atribuicao-supervisor/${dresSelecionadas}/${supervisorId ||
      ''}`;
    history.push(path);
  }

  function onClickNovaAtribuicao() {
    if (!permissoesTela.podeIncluir) return;

    if (dresSelecionadas) {
      history.push(`/gestao/atribuicao-supervisor/${dresSelecionadas}/`);
    } else {
      history.push('/gestao/atribuicao-supervisor');
    }
  }

  function onChangeUesSemSup(e) {
    if (e.target.checked) {
      setUesSemSupervisorCheck(true);
    } else {
      setUesSemSupervisorCheck(false);
    }
  }

  async function montaListaUesSemSup(dre) {
    setSupervisoresSelecionados([]);
    setUeSelecionada('');
    setDesabilitarSupervisor(true);
    setDesabilitarUe(true);
    const vinculoEscolasDreSemAtrib = await api.get(
      `/v1/dres/${dre}/ues/sem-atribuicao`
    );
    const novaLista = [
      {
        escolas: vinculoEscolasDreSemAtrib.data,
      },
    ];
    montarListaAtribuicao(novaLista, dre, true);
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

      carregarUes(usuario.turmaSelecionada.dre);
      setDresSelecionadas(usuario.turmaSelecionada.dre);
    } else {
      setAssumirFiltroPrincCheck(false);
      setDesabilitarSupervisor(false);
      setDesabilitarUe(false);
      setDesabilitarDre(false);
    }
  }

  const onChangeDre = useCallback(dre => {
    setListaSupervisores([]);
    setSupervisoresSelecionados([]);
    setListaUes([]);
    setUeSelecionada('');
    if (dre) {
      if (uesSemSupervisorCheck) {
        montaListaUesSemSup(dre);
      } else {
        const vinculoEscolasDre = api.get(
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
  }, []);

  function montarListaAtribuicao(lista, dre, isArray) {
    if (lista) {
      const dadosAtribuicao = [];
      if (isArray) {
        lista.forEach(item => {
          montarLista(item, dadosAtribuicao);
        });
        setListaFiltroAtribuicao(dadosAtribuicao);
      } else {
        setListaFiltroAtribuicao(montarLista(lista, dadosAtribuicao));
      }
    } else {
      setListaFiltroAtribuicao([]);
    }

    function montarLista(item, dadosAtribuicao) {
      const dreSelecionada = listaDres.find(d => d.codigo == dre);
      item.escolas.forEach(escola => {
        const contId = dadosAtribuicao.length + 1;
        dadosAtribuicao.push({
          id: contId,
          dre: dreSelecionada.abreviacao,
          escola: escola.nome,
          supervisor: item.supervisorId ? item.supervisorNome : '',
          supervisorId: item.supervisorId,
        });
      });
      return dadosAtribuicao;
    }
  }

  async function carregarSupervisores(dre) {
    const sups = await api.get(`/v1/supervisores/dre/${dre}`);
    if (sups.data) {
      setListaSupervisores(sups.data);
    } else {
      setListaSupervisores([]);
    }
  }

  async function carregarUes(dre) {
    const ues = await api.get(`/v1/abrangencias/false/dres/${dre}/ues`);
    if (ues.data) {
      ues.data.forEach(
        ue => (ue.nome = `${tipoEscolaDTO[ue.tipoEscola]} ${ue.nome}`)
      );
      setListaUes(ues.data.sort(FiltroHelper.ordenarLista('nome')));
    } else {
      setListaUes([]);
    }
  }

  async function onChangeSupervisores(sup) {
    if (sup && sup.length) {
      const vinculoSupervisores = await api.get(
        `/v1/supervisores/${sup.toString()}/dre/${dresSelecionadas}`
      );
      montarListaAtribuicao(vinculoSupervisores.data, dresSelecionadas, true);
      setDesabilitarUe(true);
      setUeSelecionada([]);
      setSupervisoresSelecionados(sup);
    } else {
      setSupervisoresSelecionados([]);
      setDesabilitarUe(false);
      onChangeDre(dresSelecionadas);
    }
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

  useEffect(() => {
    if (listaDres && listaDres.length === 1) {
      onChangeDre(listaDres[0].codigo);
    }
  }, [listaDres, onChangeDre]);

  return (
    <>
      <Cabecalho pagina="Atribuição de Supervisor - Listagem" />
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
            label="Nova Atribuição"
            color={Colors.Roxo}
            border
            bold
            disabled={!permissoesTela.podeIncluir}
            className="mr-2"
            onClick={onClickNovaAtribuicao}
          />
        </div>

        <div className="col-md-6 pb-2">
          <CheckboxComponent
            className="mb-2"
            label="Exibir apenas UE's sem supervisor"
            onChangeCheckbox={onChangeUesSemSup}
            disabled={
              !dresSelecionadas ||
              assumirFiltroPrincCheck ||
              !permissoesTela.podeConsultar
            }
            checked={uesSemSupervisorCheck}
          />
          <CheckboxComponent
            label="Assumir o filtro principal do sistema"
            onChangeCheckbox={onChangeAssumirFiltroPrinc}
            checked={assumirFiltroPrincCheck}
            disabled={
              desabilitarAssumirFiltroPrincipal || !permissoesTela.podeConsultar
            }
          />
        </div>
        <div className="col-md-6 pb-2">
          <SelectComponent
            className="col-md-12"
            name="dres-atribuicao-sup"
            id="dres-atribuicao-sup"
            lista={listaDres}
            valueOption="codigo"
            valueText="nome"
            onChange={onChangeDre}
            valueSelect={dresSelecionadas}
            placeholder="SELECIONE A DRE"
            disabled={desabilitarDre || !permissoesTela.podeConsultar}
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
            disabled={desabilitarSupervisor || !permissoesTela.podeConsultar}
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
            valueSelect={ueSelecionada || []}
            placeholder="SELECIONE A UE"
            disabled={desabilitarUe || !permissoesTela.podeConsultar}
          />
        </div>

        <div className="col-md-12 pt-4">
          <DataTable
            onClickRow={permissoesTela.podeAlterar && onClickRow}
            columns={columns}
            dataSource={listaFiltroAtribuicao}
          />
        </div>
      </Card>
    </>
  );
}
