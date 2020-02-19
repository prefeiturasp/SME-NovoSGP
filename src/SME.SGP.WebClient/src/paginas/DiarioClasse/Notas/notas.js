import { Tabs } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader, Grid } from '~/componentes';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import notasConceitos from '~/dtos/notasConceitos';
import { setModoEdicaoGeral } from '~/redux/modulos/notasConceitos/actions';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';

import BotoesAcoessNotasConceitos from './botoesAcoes';
import { Container, ContainerAuditoria } from './notas.css';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import Row from '~/componentes/row';
import Alert from '~/componentes/alert';

const { TabPane } = Tabs;

const Notas = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const dispatch = useDispatch();
  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const [tituloNotasConceitos, setTituloNotasConceitos] = useState('');
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [notaTipo, setNotaTipo] = useState();
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

  const [desabilitarCampos, setDesabilitarCampos] = useState(false);

  useEffect(() => {
    const somenteConsulta = verificaSomenteConsulta(permissoesTela);
    const desabilitar =
      somenteConsulta ||
      !permissoesTela.podeAlterar ||
      !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);
  }, [permissoesTela]);

  const resetarTela = useCallback(() => {
    setDisciplinaSelecionada(undefined);
    setBimestreCorrente(0);
    setNotaTipo(0);
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
        turmaHistorico: usuario.turmaSelecionada.consideraHistorico,
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
    const url = `v1/professores/turmas/${usuario.turmaSelecionada.turma}/disciplinas`;
    const disciplinas = await api.get(url);

    setListaDisciplinas(disciplinas.data);
    if (disciplinas.data && disciplinas.data.length === 1) {
      const disciplina = disciplinas.data[0];
      setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
      setDesabilitarDisciplina(true);
      obterDadosBimestres(disciplina.codigoComponenteCurricular);
    }
    if (
      match &&
      match.params &&
      match.params.disciplinaId &&
      match.params.bimestre
    ) {
      setDisciplinaSelecionada(String(match.params.disciplinaId));
      obterDadosBimestres(match.params.disciplinaId, match.params.bimestre);
    }
  }, [obterDadosBimestres, usuario.turmaSelecionada.turma]);

  const obterTituloTela = useCallback(async () => {
    const url = `v1/avaliacoes/notas/turmas/${usuario.turmaSelecionada.turma}/anos-letivos/${usuario.turmaSelecionada.anoLetivo}/tipos?consideraHistorico=${usuario.turmaSelecionada.consideraHistorico}`;
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
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
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
              notaTipo === notasConceitos.Conceitos ? nota.notaConceito : null,
            nota: notaTipo === notasConceitos.Notas ? nota.notaConceito : null,
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
          disciplinaId: disciplinaSelecionada,
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
    if (modoEdicaoGeral) {
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
      resetarTela();
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

        bimestrePesquisado.alunos.forEach(aluno => {
          return aluno.notasAvaliacoes.forEach(nota => {
            const notaOriginal = nota.notaConceito;
            /* eslint-disable */
            nota.notaOriginal = notaOriginal;
            /* eslint-enable */
            return nota;
          });
        });

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
      obterDadosBimestres(disciplinaSelecionada, bimestreCorrente);
      dispatch(setModoEdicaoGeral(false));
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
      {!usuario.turmaSelecionada.turma ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma.',
                  estiloTitulo: { fontSize: '18px' },
                }}
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
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
                  desabilitarBotao={desabilitarCampos}
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
                  placeholder="Selecione um componente curricular"
                  disabled={
                    desabilitarDisciplina || !usuario.turmaSelecionada.turma
                  }
                />
              </div>
            </div>

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
                          desabilitarCampos={desabilitarCampos}
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
                          desabilitarCampos={desabilitarCampos}
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
                          desabilitarCampos={desabilitarCampos}
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
                          desabilitarCampos={desabilitarCampos}
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
                </div>
              </div>
            </>
          </div>
        </Card>
      </Loader>
    </Container>
  );
};

export default Notas;
