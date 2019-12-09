import { Tabs } from 'antd';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import { Container, ContainerAuditoria } from './notas.css';

const { TabPane } = Tabs;

const Notas = () => {

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [bimestres, setBimestres] = useState();
  const [notaTipo, setNotaTipo] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoriaInfo, setAuditoriaInfo] = useState({
    auditoriaAlterado: '',
    auditoriaInserido: ''
  });

  useEffect(() => {
    const obterDisciplinas = async () => {
      const url = `v1/professores/123/turmas/${turmaId}/disciplinas`;
      const disciplinas = await api.get(url);

      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
        setDesabilitarDisciplina(true);
        obterDadosBimestres(disciplina.codigoComponenteCurricular);
      }
    };

    if (turmaId) {
      setDisciplinaSelecionada(undefined);
      obterDisciplinas();
    } else {
      // TODO - Resetar tela
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      setDisciplinaSelecionada(undefined);
    }
  }, [turmaSelecionada.turma]);


  const onClickVoltar = async () => {
    if (!desabilitarCampos && modoEdicao) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        await onSalvarNotas();
        irParaHome();
      } else {
        irParaHome();
      }
    } else {
      irParaHome();
    }
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const irParaHome = () => {
    history.push(URL_HOME);
  };

  const onClickSalvar = ()=> {
    console.log('onClickSalvar');
    console.log(bimestres);
  }

  const onChangeDisciplinas = disciplinaId => {
    setDisciplinaSelecionada(disciplinaId);
    obterDadosBimestres(disciplinaId, 0)
  };

  const onChangeTab = (item) => {
    console.log(item);
  }

  const obterDadosBimestres = async (disciplinaId, numeroBimestre) => {
    const params = {
      bimestre: numeroBimestre,
      disciplinaCodigo: disciplinaId,
      professorRf: usuario.rf
    }
    const dados = await api.get('v1/avaliacoes/notas', params);
    if (dados && dados.data) {
      setBimestres([...dados.data.bimestres]);
      setNotaTipo(dados.data.notaTipo);
      setAuditoriaInfo({
        auditoriaAlterado: dados.data.auditoriaAlterado,
        auditoriaInserido: dados.data.auditoriaInserido
      });
    }
  }

  const onChangeAvaliacao = ()=> {
    setModoEdicao(true);
  }

  const onSalvarNotas = click => {
    return new Promise((resolve, reject) => {
      const valorParaSalvar = { };
      return api
        .post(`v1/avaliacoes/notas`, valorParaSalvar)
        .then(salvouNotas => {
          if (salvouNotas && salvouNotas.status == 200) {
            sucesso('Suas informações foram salvas com sucesso.');
            if (click) {
              aposSalvarNotas();
            }
            resolve(true);
            return true;
          } else {
            resolve(false);
            return false;
          }
        })
        .catch(e => {
          erros(e);
          reject(e);
        });
    });
  };

  const aposSalvarNotas = () => {
    setModoEdicao(false);
    // TODO - Obter nota por id - atualizar data alteracao e inserção
  };

  const onClickCancelar = async () => {
    if (!desabilitarCampos && modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        setModoEdicao(false);
        // TODO - Obter nota por id - atualizar data alteracao e inserção
        obterDadosBimestres(disciplinaSelecionada)
      }
    }
  };

  return (
    <Container>
      <Cabecalho pagina="Lançamento de notas" />
      <Card>
        <div className="col-md-12">
          <div className="row">
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
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-2"
                onClick={onClickCancelar}
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickSalvar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
              />
            </div>
          </div>
          {
            bimestres && bimestres.length  ?
            <>
              <div className="row">
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <ContainerTabsCard type="card" onChangeTab={onChangeTab}>
                    {bimestres.map((item, i) => {
                        return (
                          <TabPane tab={item.descricao} key={i}>
                            <Avaliacao
                              dados={item}
                              notaTipo={notaTipo}
                              onChangeAvaliacao={onChangeAvaliacao}
                            ></Avaliacao>
                          </TabPane>
                        );
                   })}
                  </ContainerTabsCard>
                </div>
              </div>
              <div className="row mt-2 mb-2 mt-2">
               <div className="col-md-12">
                <ContainerAuditoria style={{float: "left"}}>
                  <span>
                    <p>{auditoriaInfo.auditoriaInserido || ''}</p>
                    <p>{auditoriaInfo.auditoriaAlterado || ''}</p>
                  </span>
                </ContainerAuditoria>
                <span style={{float: "right"}} className="mt-1 ml-1">
                  Aluno ausente na data da avaliação
                </span>
                <span className="icon-legenda-aluno-ausente">
                  <i className="fas fa-user-times"/>
                </span>
                </div>
              </div>
            </>
          :''
          }
        </div>
      </Card>
    </Container>
  );
};

export default Notas;
