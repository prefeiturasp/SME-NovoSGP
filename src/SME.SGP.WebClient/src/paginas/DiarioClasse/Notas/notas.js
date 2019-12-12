import { Tabs } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import notasConceitos from '~/dtos/notasConceitos';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import BotoesAcoessNotasConceitos from './botoesAcoes';
import { Container, ContainerAuditoria } from './notas.css';

const { TabPane } = Tabs;

const Notas = () => {
  const usuario = useSelector(store => store.usuario);

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [notaTipo, setNotaTipo] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [auditoriaInfo, setAuditoriaInfo] = useState({
    auditoriaAlterado: '',
    auditoriaInserido: '',
  });

  const [primeiroBimestre, setPrimeiroBimestre] = useState([]);
  const [segundoBimestre, setSegundoBimestre] = useState([]);
  const [terceiroBimestre, setTerceiroBimestre] = useState([]);
  const [quartoBimestre, setQuartoBimestre] = useState([]);

  const [bimestres, setBimestres] = useState([]);

  const obterBimestres = useCallback(
    async (disciplinaId, numeroBimestre) => {
      const params = {
        anoLetivo: usuario.turmaSelecionada.anoLetivo,
        bimestre: numeroBimestre,
        disciplinaCodigo: disciplinaId,
        modalidade: usuario.turmaSelecionada.modalidade,
        turmaCodigo: usuario.turmaSelecionada.turma,
      };
      const dados = await api
        .get('v1/avaliacoes/notas/', { params })
        .catch(e => erros(e));

      return dados ? dados.data : [];
    },
    [
      usuario.turmaSelecionada.anoLetivo,
      usuario.turmaSelecionada.modalidade,
      usuario.turmaSelecionada.turma,
    ]
  );

  const obterDadosBimestres = useCallback(
    async (disciplinaId, numeroBimestre) => {
      const dados = await obterBimestres(disciplinaId, numeroBimestre);
      if (dados) {
        dados.bimestres.forEach(item => {
          switch (Number(item.numero)) {
            case 1:
              setPrimeiroBimestre(item);
              break;
            case 2:
              setSegundoBimestre(item);
              break;
            case 3:
              setTerceiroBimestre(item);
              break;
            case 4:
              setQuartoBimestre(item);
              break;

            default:
              break;
          }
        });

        // setBimestres([...dados.bimestres]);

        setNotaTipo(dados.notaTipo);
        setAuditoriaInfo({
          auditoriaAlterado: dados.auditoriaAlterado,
          auditoriaInserido: dados.auditoriaInserido,
        });
      }
    },
    [obterBimestres]
  );

  // const resetarTela = useCallback(() => {
  //   if (disciplinaSelecionada) {
  //     setDisciplinaSelecionada(undefined);
  //   }
  //   // if (bimestres && bimestres.length > 0) {
  //   //   setBimestres([]);
  //   // }

  //   setNotaTipo(0);
  //   setDesabilitarCampos(false);
  //   setModoEdicao(false);
  //   setAuditoriaInfo({
  //     auditoriaAlterado: '',
  //     auditoriaInserido: '',
  //   });
  // }, [disciplinaSelecionada]);

  const obterDisciplinas = useCallback(async () => {
    const url = `v1/professores/123/turmas/${usuario.turmaSelecionada.turma}/disciplinas`;
    const disciplinas = await api.get(url);

    setListaDisciplinas(disciplinas.data);
    if (disciplinas.data && disciplinas.data.length === 1) {
      const disciplina = disciplinas.data[0];
      setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
      setDesabilitarDisciplina(true);
      obterDadosBimestres(disciplina.codigoComponenteCurricular);
    }
  }, [obterDadosBimestres, usuario.turmaSelecionada.turma]);

  useEffect(() => {
    if (usuario.turmaSelecionada.turma) {
      obterDisciplinas();
    } else {
      // TODO - Resetar tela
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      // resetarTela();
    }
  }, [obterDisciplinas, usuario.turmaSelecionada.turma]);

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

  const aposSalvarNotas = () => {
    setModoEdicao(false);
    // TODO - Obter nota por id - atualizar data alteracao e inserção
  };

  const onSalvarNotas = click => {
    return new Promise((resolve, reject) => {
      const valorParaSalvar = [];
      const bimestresEmEdicao = bimestres.filter(item => item.modoEdicao);

      bimestresEmEdicao.forEach(b => {
        b.alunos.forEach(aluno => {
          aluno.notasAvaliacoes.forEach(nota => {
            if (nota.notaConceito) {
              valorParaSalvar.push({
                alunoId: aluno.id,
                atividadeAvaliativaId: nota.atividadeAvaliativaId,
                conceito:
                  notaTipo === notasConceitos.Conceitos ? nota.notaConceito : 0,
                nota: notaTipo === notasConceitos.Notas ? nota.notaConceito : 0,
              });
            }
          });
        });
      });

      return api
        .post(`v1/avaliacoes/notas`, {
          turmaId: usuario.turmaSelecionada.turma,
          notasConceitos: valorParaSalvar,
        })
        .then(salvouNotas => {
          if (salvouNotas && salvouNotas.status === 200) {
            sucesso('Suas informações foram salvas com sucesso.');
            if (click) {
              aposSalvarNotas();
            }
            resolve(true);
            return true;
          }
          resolve(false);
          return false;
        })
        .catch(e => {
          erros(e);
          reject(e);
        });
    });
  };

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

  const onClickSalvar = () => {
    onSalvarNotas();
  };

  const onChangeDisciplinas = disciplinaId => {
    if (disciplinaId) {
      obterDadosBimestres(disciplinaId, 0);
      setDisciplinaSelecionada(disciplinaId);
    } else {
      // resetarTela();
    }
  };

  const onChangeTab = async numeroBimestre => {
    let bimestre = {};
    switch (Number(numeroBimestre)) {
      case 1:
        bimestre = primeiroBimestre;
        break;
      case 2:
        bimestre = segundoBimestre;
        break;
      case 3:
        bimestre = terceiroBimestre;
        break;
      case 4:
        bimestre = quartoBimestre;
        break;
      default:
        break;
    }

    if (bimestre && !bimestre.modoEdicao) {
      const dados = await obterBimestres(disciplinaSelecionada, numeroBimestre);
      if (dados && dados.bimestres && dados.bimestres.length) {
        const bimestrePesquisado = dados.bimestres.find(
          item => Number(item.numero) === Number(numeroBimestre)
        );

        switch (Number(numeroBimestre)) {
          case 1:
            setPrimeiroBimestre(bimestrePesquisado);
            break;
          case 2:
            setSegundoBimestre(bimestrePesquisado);
            break;
          case 3:
            setTerceiroBimestre(bimestrePesquisado);
            break;
          case 4:
            setQuartoBimestre(bimestrePesquisado);
            break;
          default:
            break;
        }
      }
    }
  };

  const onClickCancelar = async cancelar => {
    if (cancelar) {
      setBimestres([]);
      setModoEdicao(false);
      obterDadosBimestres(disciplinaSelecionada, 0);
    }
  };

  const onChangeOrdenacao = bimestreOrdenado => {
    const bimestreAtualizado = {
      descricao: bimestreOrdenado.descricao,
      numero: bimestreOrdenado.numero,
      alunos: [...bimestreOrdenado.alunos],
      avaliacoes: [...bimestreOrdenado.avaliacoes],
    };
    switch (Number(bimestreOrdenado.numero)) {
      case 1:
        setPrimeiroBimestre(bimestreAtualizado);
        break;
      case 2:
        setSegundoBimestre(bimestreAtualizado);
        break;
      case 3:
        setTerceiroBimestre(bimestreAtualizado);
        break;
      case 4:
        setQuartoBimestre(bimestreAtualizado);
        break;
      default:
        break;
    }
  };

  return (
    <Container>
      <Cabecalho pagina="Lançamento de notas" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoessNotasConceitos
                onClickVoltar={onClickVoltar}
                onClickCancelar={onClickCancelar}
                onClickSalvar={onClickSalvar}
                desabilitarCampos={desabilitarCampos}
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
          {true ? (
            <>
              <div className="row">
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <ContainerTabsCard type="card" onChange={onChangeTab}>
                    {/* {bimestres.map(item => {
                      return (
                        <TabPane tab={item.descricao} key={item.numero}>
                          <Avaliacao
                            dados={item}
                            notaTipo={notaTipo}
                            onChangeOrdenacao={onChangeOrdenacao}
                          />
                        </TabPane>
                      );
                    })} */}
                    {primeiroBimestre.numero ? (
                      <TabPane tab="Primeiro" key={1}>
                        <Avaliacao
                          dados={primeiroBimestre}
                          notaTipo={notaTipo}
                          onChangeOrdenacao={onChangeOrdenacao}
                        />
                      </TabPane>
                    ) : (
                      ''
                    )}
                    {segundoBimestre.numero ? (
                      <TabPane tab="Segundo" key={2}>
                        <Avaliacao
                          dados={segundoBimestre}
                          notaTipo={notaTipo}
                          onChangeOrdenacao={onChangeOrdenacao}
                        />
                      </TabPane>
                    ) : (
                      ''
                    )}
                    {terceiroBimestre.numero ? (
                      <TabPane tab="Terceiro" key={3}>
                        <Avaliacao
                          dados={terceiroBimestre}
                          notaTipo={notaTipo}
                          onChangeOrdenacao={onChangeOrdenacao}
                        />
                      </TabPane>
                    ) : (
                      ''
                    )}
                    {quartoBimestre.numero ? (
                      <TabPane tab="Quarto" key={4}>
                        <Avaliacao
                          dados={quartoBimestre}
                          notaTipo={notaTipo}
                          onChangeOrdenacao={onChangeOrdenacao}
                        />
                      </TabPane>
                    ) : (
                      ''
                    )}
                  </ContainerTabsCard>
                </div>
              </div>
              <div className="row mt-2 mb-2 mt-2">
                <div className="col-md-12">
                  <ContainerAuditoria style={{ float: 'left' }}>
                    <span>
                      <p>{auditoriaInfo.auditoriaInserido || ''}</p>
                      <p>{auditoriaInfo.auditoriaAlterado || ''}</p>
                    </span>
                  </ContainerAuditoria>
                  <span style={{ float: 'right' }} className="mt-1 ml-1">
                    Aluno ausente na data da avaliação
                  </span>
                  <span className="icon-legenda-aluno-ausente">
                    <i className="fas fa-user-times" />
                  </span>
                </div>
              </div>
            </>
          ) : (
            ''
          )}
        </div>
      </Card>
    </Container>
  );
};

export default Notas;
