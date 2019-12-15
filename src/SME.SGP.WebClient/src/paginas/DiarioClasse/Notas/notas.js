import { Tabs } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import notasConceitos from '~/dtos/notasConceitos';
import { setModoEdicaoGeral } from '~/redux/modulos/notasConceitos/actions';
import { erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import BotoesAcoessNotasConceitos from './botoesAcoes';
import { Container, ContainerAuditoria } from './notas.css';

const { TabPane } = Tabs;

const Notas = () => {
  const usuario = useSelector(store => store.usuario);
  const dispatch = useDispatch();
  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const [tituloNotasConceitos, setTituloNotasConceitos] = useState('');
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [notaTipo, setNotaTipo] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [carregandoListaBimestres, setCarregandoListaBimestres] = useState(
    false
  );
  const [auditoriaInfo, setAuditoriaInfo] = useState({
    auditoriaAlterado: '',
    auditoriaInserido: '',
  });
  const [bimestreCorrente, setBimestreCorrente] = useState(0);
  const [primeiroBimestre, setPrimeiroBimestre] = useState([]);
  const [segundoBimestre, setSegundoBimestre] = useState([]);
  const [terceiroBimestre, setTerceiroBimestre] = useState([]);
  const [quartoBimestre, setQuartoBimestre] = useState([]);

  const resetarTela = useCallback(() => {
    setDisciplinaSelecionada(undefined);
    setBimestreCorrente(0);
    setNotaTipo(0);
    setDesabilitarCampos(false);
    setAuditoriaInfo({
      auditoriaAlterado: '',
      auditoriaInserido: '',
    });
    dispatch(setModoEdicaoGeral(false));
  }, [dispatch]);

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
  // Só é chamado quando: Seta, remove ou troca a disciplina e quando cancelar a edição;
  const obterDadosBimestres = useCallback(
    async (disciplinaId, numeroBimestre) => {
      if (disciplinaId > 0) {
        setCarregandoListaBimestres(true);
        const dados = await obterBimestres(disciplinaId, numeroBimestre);
        if (dados && dados.bimestres && dados.bimestres.length) {
          dados.bimestres.forEach(item => {
            item.alunos.forEach(aluno => {
              return aluno.notasAvaliacoes.forEach(nota => {
                const notaOriginal = nota.notaConceito;
                /* eslint-disable */
                nota.notaOriginal = notaOriginal;
                /* eslint-enable */
                return nota;
              });
            });

            const bimestreAtualizado = {
              descricao: item.descricao,
              numero: item.numero,
              alunos: [...item.alunos],
              avaliacoes: [...item.avaliacoes],
            };

            switch (Number(item.numero)) {
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
            if (bimestreAtualizado.alunos.length > 0) {
              setBimestreCorrente(bimestreAtualizado.numero);
            }
          });

          setNotaTipo(dados.notaTipo);
          setAuditoriaInfo({
            auditoriaAlterado: dados.auditoriaAlterado,
            auditoriaInserido: dados.auditoriaInserido,
          });
        }
        setCarregandoListaBimestres(false);
      } else {
        resetarTela();
      }
    },
    [obterBimestres, resetarTela]
  );

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

  const obterTituloTela = useCallback(async () => {
    const url = `v1/avaliacoes/notas/turmas/${usuario.turmaSelecionada.turma}/anos-letivos/${usuario.turmaSelecionada.anoLetivo}/tipos`;
    const tipoNotaTurmaSelecionada = await api.get(url);
    if (
      Number(notasConceitos.Conceitos) === Number(tipoNotaTurmaSelecionada.data)
    ) {
      return 'Lançamento de Conceitos';
    }
    return 'Lançamento de Notas';
  }, [usuario.turmaSelecionada.anoLetivo, usuario.turmaSelecionada.turma]);

  useEffect(() => {
    obterTituloTela().then(titulo => {
      setTituloNotasConceitos(titulo);
    });
  }, [obterTituloTela]);

  useEffect(() => {
    if (usuario.turmaSelecionada.turma) {
      obterDisciplinas();
    } else {
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      resetarTela();
    }
  }, [obterDisciplinas, usuario.turmaSelecionada.turma, resetarTela]);

  const pergutarParaSalvar = () => {
    if (
      window.confirm(`Suas alterações não foram salvas, deseja salvar agora?`)
    ) {
      return true;
    }
    return false;
    // TODO - Voltar esse fonte apois ajuste de modal de confirmação
    // return confirmar(
    //   'Atenção',
    //   '',
    //   'Suas alterações não foram salvas, deseja salvar agora?'
    // );
  };

  const irParaHome = () => {
    history.push(URL_HOME);
  };

  // TODO - Verificar se realmente é necessário usar o resetarBimestres!
  const resetarBimestres = () => {
    // const bimestreVazio = {
    //   descricao: '',
    //   numero: undefined,
    //   alunos: [],
    //   avaliacoes: [],
    // };
    // setPrimeiroBimestre(bimestreVazio);
    // setSegundoBimestre(bimestreVazio);
    // setTerceiroBimestre(bimestreVazio);
    // setQuartoBimestre(bimestreVazio);
  };

  const aposSalvarNotas = () => {
    // resetarBimestres();
    obterDadosBimestres(disciplinaSelecionada, bimestreCorrente);
  };

  const montarBimestreParaSalvar = bimestreParaMontar => {
    const valorParaSalvar = [];
    bimestreParaMontar.alunos.forEach(aluno => {
      aluno.notasAvaliacoes.forEach(nota => {
        if (nota.modoEdicao) {
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
    return valorParaSalvar;
  };

  const onSalvarNotas = click => {
    return new Promise((resolve, reject) => {
      const valoresBimestresSalvar = [];

      if (primeiroBimestre.modoEdicao) {
        valoresBimestresSalvar.push(
          ...montarBimestreParaSalvar(primeiroBimestre)
        );
      }
      if (segundoBimestre.modoEdicao) {
        valoresBimestresSalvar.push(
          ...montarBimestreParaSalvar(segundoBimestre)
        );
      }
      if (terceiroBimestre.modoEdicao) {
        valoresBimestresSalvar.push(
          ...montarBimestreParaSalvar(terceiroBimestre)
        );
      }
      if (quartoBimestre.modoEdicao) {
        valoresBimestresSalvar.push(
          ...montarBimestreParaSalvar(quartoBimestre)
        );
      }

      return api
        .post(`v1/avaliacoes/notas`, {
          turmaId: usuario.turmaSelecionada.turma,
          notasConceitos: valoresBimestresSalvar,
        })
        .then(salvouNotas => {
          if (salvouNotas && salvouNotas.status === 200) {
            sucesso('Suas informações foram salvas com sucesso.');
            dispatch(setModoEdicaoGeral(false));
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
    if (!desabilitarCampos && modoEdicaoGeral) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        await onSalvarNotas(false);
        irParaHome();
      } else {
        irParaHome();
      }
    } else {
      irParaHome();
    }
  };

  const onClickSalvar = () => {
    onSalvarNotas(true);
  };

  const onChangeDisciplinas = async disciplinaId => {
    if (modoEdicaoGeral) {
      const confirmaSalvar = await pergutarParaSalvar();
      if (confirmaSalvar) {
        await onSalvarNotas(false);
        setDisciplinaSelecionada(disciplinaId);
        obterDadosBimestres(disciplinaId, 0);
      } else {
        setDisciplinaSelecionada(disciplinaId);
        resetarTela();
        if (disciplinaId) {
          obterDadosBimestres(disciplinaId, 0);
        }
      }
    } else {
      obterDadosBimestres(disciplinaId, 0);
      setDisciplinaSelecionada(disciplinaId);
    }
  };

  const onChangeTab = async numeroBimestre => {
    setBimestreCorrente(numeroBimestre);
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

    if (bimestre && !bimestre.modoEdicao && disciplinaSelecionada) {
      setCarregandoListaBimestres(true);
      const dados = await obterBimestres(disciplinaSelecionada, numeroBimestre);
      if (dados && dados.bimestres && dados.bimestres.length) {
        const bimestrePesquisado = dados.bimestres.find(
          item => Number(item.numero) === Number(numeroBimestre)
        );

        const bimestreAtualizado = {
          descricao: bimestrePesquisado.descricao,
          numero: bimestrePesquisado.numero,
          alunos: [...bimestrePesquisado.alunos],
          avaliacoes: [...bimestrePesquisado.avaliacoes],
        };

        switch (Number(numeroBimestre)) {
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
      }
      setCarregandoListaBimestres(false);
    }
  };

  const onClickCancelar = async cancelar => {
    if (cancelar) {
      dispatch(setModoEdicaoGeral(false));
      obterDadosBimestres(disciplinaSelecionada, bimestreCorrente);
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
      <Cabecalho pagina={tituloNotasConceitos} />
      <Loader loading={carregandoListaBimestres}>
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
            {bimestreCorrente > 0 ? (
              <>
                <div className="row">
                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                    <ContainerTabsCard
                      type="card"
                      onChange={onChangeTab}
                      activeKey={String(bimestreCorrente)}
                    >
                      {primeiroBimestre.numero ? (
                        <TabPane
                          tab={primeiroBimestre.descricao}
                          key={primeiroBimestre.numero}
                        >
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
                        <TabPane
                          tab={segundoBimestre.descricao}
                          key={segundoBimestre.numero}
                        >
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
                        <TabPane
                          tab={terceiroBimestre.descricao}
                          key={terceiroBimestre.numero}
                        >
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
                        <TabPane
                          tab={quartoBimestre.descricao}
                          key={quartoBimestre.numero}
                        >
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
      </Loader>
    </Container>
  );
};

export default Notas;
