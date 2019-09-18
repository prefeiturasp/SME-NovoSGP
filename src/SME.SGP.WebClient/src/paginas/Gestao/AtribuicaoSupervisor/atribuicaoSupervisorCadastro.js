import React, { useEffect, useState } from 'react';
import SelectList from '~/componentes/selectList';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import SelectAutocomplete from '~/componentes/select-autocomplete';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import Auditoria from '~/componentes/auditoria';
import api from '~/servicos/api';
import { erro, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import Cabecalho from '~/componentes-sgp/cabecalho';
import {setBreadcrumbManual} from '~/servicos/breadcrumb-services';

const AtribuicaoSupervisorCadastro = ({ match }) => {
  const [auditoria, setAuditoria] = useState([]);

  const [listaDres, setListaDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState('');

  const [listaSupervisores, setListaSupervisores] = useState([]);
  const [supervisorSelecionado, setSupervisorSelecionado] = useState('');
  const [filtroSupervisor, setFiltroSupervisor] = useState('');

  const [listaUESDreAtual, setListaUESDreAtual] = useState([]);
  const [listaUES, setListaUES] = useState([]);
  const [uesAtribuidas, setUESAtribuidas] = useState([]);

  function exibeErro(erros) {
    if (erros && erros.response && erros.response.data)
      erros.response.data.mensagens.forEach(mensagem => erro(mensagem));
  }

  useEffect(() =>{
    setBreadcrumbManual(match.url,'Editar Atribuição', '/gestao/atribuicao-supervisor-lista');
  },[])
  // 1 - carrega dres
  useEffect(() => {
    async function obterListaDres() {
      await api
        .get('v1/dres')
        .then(resposta => {
          setListaDres(resposta.data);
          if (match.params.dreId) {
            setDreSelecionada(match.params.dreId);
          }
          if (match.params.supervisorId) {
            setTimeout(() => {
              setSupervisorSelecionado(match.params.supervisorId);
              setFiltroSupervisor(match.params.supervisorId);
            }, 500);
          }
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }
    obterListaDres();
  }, []);

  // 2 - carrega supervisores e ues
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

    if (dreSelecionada) {
      setListaSupervisores([]);
      obterListaSupervisores();
      setSupervisorSelecionado('');
      setFiltroSupervisor('');
    } else {
      setListaSupervisores([]);
    }
  }, [dreSelecionada]);

  // 3 - carrega ues atribuídas
  useEffect(() => {
    async function obterListaUESAtribuidas(uesNaoAtribuidas) {
      await api
        .get(`v1/supervisores/${supervisorSelecionado}/dre/${dreSelecionada}`)
        .then(resposta => {
          if (resposta.data && resposta.data.length) {
            const listaUesNaoAtribuidas = listaUESDreAtual.length
              ? listaUESDreAtual
              : uesNaoAtribuidas;
            const ues = [
              ...listaUesNaoAtribuidas,
              ...resposta.data[0].escolas.map(c => ({ ...c, key: c.codigo })),
            ];
            setUESAtribuidas(resposta.data[0].escolas.map(e => e.codigo));
            setListaUES(ues);
            const registro = resposta.data[0];
            setAuditoria({
              criadoPor: registro.criadoPor,
              criadoEm: registro.criadoEm,
              alteradoPor: registro.alteradoPor,
              alteradoEm: registro.alteradoEm,
            });
          } else {
            setUESAtribuidas([]);
          }
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
          if (resposta.data.length) {
            const ues = resposta.data.map(c => ({ ...c, key: c.codigo }));
            setListaUES([...ues]);
            setListaUESDreAtual([...ues]);
            if (supervisorSelecionado) obterListaUESAtribuidas(ues);
          } // else erro('Nenhuma UE sem atribuição para a DRE selecionada.');
        })
        .catch(erros => {
          exibeErro(erros);
        });
    }

    if (supervisorSelecionado) {
      obterListaUES();
    } else {
      setUESAtribuidas([]);
      setListaUES([]);
    }
  }, [supervisorSelecionado]);

  function handleChange(targetKeys) {
    setUESAtribuidas(targetKeys);
  }

  function selecionaDre(idDre) {
    setDreSelecionada(idDre);
  }

  function selecionaSupervisor(idSupervisor) {
    if (Number(idSupervisor) || !idSupervisor) {
      setSupervisorSelecionado(idSupervisor);
    }
    setFiltroSupervisor(idSupervisor);
  }

  function cancelarAlteracoes() {
    setDreSelecionada('');
    setSupervisorSelecionado('');
    setFiltroSupervisor('');
  }

  async function salvarAtribuicao() {
    const atribuicao = {
      dreId: dreSelecionada,
      supervisorId: supervisorSelecionado,
      uesIds: uesAtribuidas,
    };
    await api
      .post('v1/supervisores/atribuir-ue', atribuicao)
      .then(() => {
        sucesso('Atribuição realizada com sucesso.');
        history.push('/gestao/atribuicao-supervisor-lista');
      })
      .catch(erros => {
        exibeErro(erros);
      });
  }

  const filtraSupervisor = (item, texto) => {
    return (
      item.supervisorNome.toLowerCase().indexOf(texto) > -1 ||
      item.supervisorId === texto
    );
  };

  return (
    <>
      <Cabecalho pagina="Atribuição de Supervisor" />
      <Card>
        <div className="col-xs-12 col-md-12 col-lg-12 d-flex justify-content-end mb-4">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-3"
            onClick={() => history.push('/gestao/atribuicao-supervisor-lista')}
          />
          {dreSelecionada && supervisorSelecionado && (
            <Button
              label="Cancelar"
              color={Colors.Roxo}
              border
              bold
              className="mr-3"
              onClick={cancelarAlteracoes}
            />
          )}
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
            onSelect={selecionaSupervisor}
            onChange={selecionaSupervisor}
            value={filtroSupervisor}
            filtro={filtraSupervisor}
          />
        </div>
        <SelectList
          id="select-list-supervisor"
          dados={listaUES}
          targetKeys={uesAtribuidas}
          handleChange={handleChange}
          titulos={["UE'S SEM ATRIBUIÇÃO", "UE'S ATRIBUIDAS AO SUPERVISOR"]}
          texto="nome"
          codigo="codigo"
        />
        <Auditoria
          criadoEm={auditoria.criadoEm}
          criadoPor={auditoria.criadoPor}
          alteradoPor={auditoria.alteradoPor}
          alteradoEm={auditoria.alteradoEm}
        />
      </Card>
    </>
  );
};

export default AtribuicaoSupervisorCadastro;
