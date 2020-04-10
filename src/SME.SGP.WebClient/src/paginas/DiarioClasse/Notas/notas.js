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
import {
  setModoEdicaoGeral,
  setModoEdicaoGeralNotaFinal,
  setExpandirLinha,
} from '~/redux/modulos/notasConceitos/actions';
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
  const { ehProfessorCj } = usuario;

  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const [tituloNotasConceitos, setTituloNotasConceitos] = useState('');
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [carregandoDisciplina, setCarregandoDisciplina] = useState(false);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [notaTipo, setNotaTipo] = useState();
  const [carregandoListaBimestres, setCarregandoListaBimestres] = useState(
    false
  );
  const [auditoriaInfo, setAuditoriaInfo] = useState({
    auditoriaAlterado: '',
    auditoriaInserido: '',
    auditoriaBimestreInserido: '',
    auditoriaBimestreAlterado: '',
  });
  const [bimestreCorrente, setBimestreCorrente] = useState(0);
  const [primeiroBimestre, setPrimeiroBimestre] = useState([]);
  const [segundoBimestre, setSegundoBimestre] = useState([]);
  const [terceiroBimestre, setTerceiroBimestre] = useState([]);
  const [quartoBimestre, setQuartoBimestre] = useState([]);

  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [ehRegencia, setEhRegencia] = useState(false);

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
      auditoriaBimestreInserido: '',
      auditoriaBimestreAlterado: '',
    });
    dispatch(setModoEdicaoGeral(false));
    dispatch(setModoEdicaoGeralNotaFinal(false));
    dispatch(setExpandirLinha([]));
  }, [dispatch]);

  const obterListaConceitos = async periodoFim => {
    const lista = await api
      .get(`v1/avaliacoes/notas/conceitos?data=${periodoFim}`)
      .catch(e => erros(e));

    if (lista && lista.data && lista.data.length) {
      const novaLista = lista.data.map(item => {
        item.id = String(item.id);
        return item;
      });
      return novaLista;
    }
    return [];
  };

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
      usuario.turmaSelecionada.consideraHistorico,
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
          dados.bimestres.forEach(async item => {
            item.alunos.forEach(aluno => {
              aluno.notasAvaliacoes.forEach(nota => {
                const notaOriginal = nota.notaConceito;
                /* eslint-disable */
                nota.notaOriginal = notaOriginal;
                /* eslint-enable */
              });
              aluno.notasBimestre.forEach(nota => {
                const notaOriginal = nota.notaConceito;
                /* eslint-disable */
                nota.notaOriginal = notaOriginal;
                /* eslint-enable */
              });
            });

            setNotaTipo(dados.notaTipo);

            let listaTiposConceitos = [];
            if (Number(notasConceitos.Conceitos) === Number(dados.notaTipo)) {
              listaTiposConceitos = await obterListaConceitos(item.periodoFim);
            }

            const bimestreAtualizado = {
              fechamentoTurmaId: item.fechamentoTurmaId,
              descricao: item.descricao,
              numero: item.numero,
              alunos: [...item.alunos],
              avaliacoes: [...item.avaliacoes],
              periodoInicio: item.periodoInicio,
              periodoFim: item.periodoFim,
              mediaAprovacaoBimestre: dados.mediaAprovacaoBimestre,
              listaTiposConceitos,
              observacoes: item.observacoes,
              podeLancarNotaFinal: item.podeLancarNotaFinal,
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

          setAuditoriaInfo({
            auditoriaAlterado: dados.auditoriaAlterado,
            auditoriaInserido: dados.auditoriaInserido,
            auditoriaBimestreAlterado: dados.auditoriaBimestreAlterado,
            auditoriaBimestreInserido: dados.auditoriaBimestreInserido,
          });
        } else {
          setAuditoriaInfo({
            auditoriaAlterado: '',
            auditoriaInserido: '',
            auditoriaBimestreAlterado: '',
            auditoriaBimestreInserido: '',
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
    setCarregandoDisciplina(true);
    const url = `v1/professores/turmas/${usuario.turmaSelecionada.turma}/disciplinas`;
    const disciplinas = await api.get(url);

    setListaDisciplinas(disciplinas.data);
    if (disciplinas.data && disciplinas.data.length === 1) {
      const disciplina = disciplinas.data[0];
      setEhRegencia(disciplina.regencia);
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
    setCarregandoDisciplina(false);
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
      dispatch(setModoEdicaoGeral(false));
      dispatch(setModoEdicaoGeralNotaFinal(false));
      dispatch(setExpandirLinha([]));
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

  const salvarNotasAvaliacoes = (resolve, reject, click) => {
    const valoresBimestresSalvar = [];

    if (primeiroBimestre.modoEdicao) {
      valoresBimestresSalvar.push(
        ...montarBimestreParaSalvar(primeiroBimestre)
      );
    }
    if (segundoBimestre.modoEdicao) {
      valoresBimestresSalvar.push(...montarBimestreParaSalvar(segundoBimestre));
    }
    if (terceiroBimestre.modoEdicao) {
      valoresBimestresSalvar.push(
        ...montarBimestreParaSalvar(terceiroBimestre)
      );
    }
    if (quartoBimestre.modoEdicao) {
      valoresBimestresSalvar.push(...montarBimestreParaSalvar(quartoBimestre));
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
          dispatch(setModoEdicaoGeralNotaFinal(false));
          dispatch(setExpandirLinha([]));
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
  };

  const pergutarParaSalvarNotaFinal = bimestresSemAvaliacaoBimestral => {
    if (
      bimestresSemAvaliacaoBimestral &&
      bimestresSemAvaliacaoBimestral.length
    ) {
      return confirmar(
        'Atenção',
        bimestresSemAvaliacaoBimestral,
        'Deseja continuar mesmo assim com o fechamento do(s) bimestre(s)?'
      );
    }

    return new Promise(resolve => resolve(true));
  };

  const montarBimestreParaSalvarNotaFinal = bimestreParaMontar => {
    const notaConceitoAlunos = [];
    bimestreParaMontar.alunos.forEach(aluno => {
      aluno.notasBimestre.forEach(notaFinal => {
        if (notaFinal.modoEdicao) {
          notaConceitoAlunos.push({
            codigoAluno: aluno.id,
            disciplinaId:
              notasConceitos.Notas == notaTipo
                ? disciplinaSelecionada
                : notaFinal.disciplinaId,
            nota:
              notaTipo === notasConceitos.Notas ? notaFinal.notaConceito : 0,
            conceitoId:
              notaTipo === notasConceitos.Conceitos
                ? notaFinal.notaConceito
                : 0,
          });
        }
      });
    });
    // TODO REVISAR NA EDICAO E NA ADD E INSERT DE CONCEITOS!!!!
    return {
      id: bimestreParaMontar.fechamentoTurmaId,
      turmaId: usuario.turmaSelecionada.turma,
      bimestre: bimestreParaMontar.numero,
      disciplinaId: disciplinaSelecionada,
      notaConceitoAlunos,
    };
  };

  const montaQtdAvaliacaoBimestralPendent = (
    bimestre,
    bimestresSemAvaliacaoBimestral
  ) => {
    if (bimestre.observacoes && bimestre.observacoes.length) {
      bimestre.observacoes.forEach(item => {
        bimestresSemAvaliacaoBimestral.push(item);
      });
    }
  };

  const salvarNotasFinais = (resolve, reject, click) => {
    const valoresBimestresSalvar = [];
    const bimestresSemAvaliacaoBimestral = [];

    if (primeiroBimestre.modoEdicao) {
      montaQtdAvaliacaoBimestralPendent(
        primeiroBimestre,
        bimestresSemAvaliacaoBimestral
      );
      valoresBimestresSalvar.push(
        montarBimestreParaSalvarNotaFinal(primeiroBimestre)
      );
    }
    if (segundoBimestre.modoEdicao) {
      montaQtdAvaliacaoBimestralPendent(
        segundoBimestre,
        bimestresSemAvaliacaoBimestral
      );
      valoresBimestresSalvar.push(
        montarBimestreParaSalvarNotaFinal(segundoBimestre)
      );
    }
    if (terceiroBimestre.modoEdicao) {
      montaQtdAvaliacaoBimestralPendent(
        terceiroBimestre,
        bimestresSemAvaliacaoBimestral
      );
      valoresBimestresSalvar.push(
        montarBimestreParaSalvarNotaFinal(terceiroBimestre)
      );
    }
    if (quartoBimestre.modoEdicao) {
      montaQtdAvaliacaoBimestralPendent(
        quartoBimestre,
        bimestresSemAvaliacaoBimestral
      );
      valoresBimestresSalvar.push(
        montarBimestreParaSalvarNotaFinal(quartoBimestre)
      );
    }

    return pergutarParaSalvarNotaFinal(bimestresSemAvaliacaoBimestral)
      .then(salvarAvaliacaoFinal => {
        if (salvarAvaliacaoFinal) {
          return api
            .post(`/v1/fechamentos/turmas`, valoresBimestresSalvar)
            .then(salvouNotas => {
              if (salvouNotas && salvouNotas.status === 200) {
                sucesso('Suas informações foram salvas com sucesso.');
                dispatch(setModoEdicaoGeral(false));
                dispatch(setModoEdicaoGeralNotaFinal(false));
                dispatch(setExpandirLinha([]));
                if (click) {
                  aposSalvarNotas();
                }
                return resolve(true);
              }
              return resolve(false);
            })
            .catch(e => {
              erros(e);
              reject(e);
            });
        }
        return resolve(false);
      })
      .catch(e => {
        erros(e);
        reject(e);
      });
  };

  const onSalvarNotas = (click, salvarNotaFinal) => {
    return new Promise((resolve, reject) => {
      if (salvarNotaFinal) {
        return salvarNotasFinais(resolve, reject, click);
      }
      return salvarNotasAvaliacoes(resolve, reject, click);
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

  const onClickSalvar = salvarNotaFinal => {
    onSalvarNotas(true, salvarNotaFinal);
  };

  const validaSeEhRegencia = disciplinaId => {
    if (disciplinaId) {
      const disciplina = listaDisciplinas.find(
        item => item.codigoComponenteCurricular == disciplinaId
      );
      if (disciplina) {
        setEhRegencia(!!disciplina.regencia);
      } else {
        setEhRegencia(false);
      }
    }
  };

  const onChangeDisciplinas = async disciplinaId => {
    validaSeEhRegencia(disciplinaId);

    dispatch(setModoEdicaoGeral(false));
    dispatch(setModoEdicaoGeralNotaFinal(false));
    dispatch(setExpandirLinha([]));

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
    dispatch(setExpandirLinha([]));
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
          aluno.notasAvaliacoes.forEach(nota => {
            const notaOriginal = nota.notaConceito;
            /* eslint-disable */
            nota.notaOriginal = notaOriginal;
            /* eslint-enable */
          });
          aluno.notasBimestre.forEach(nota => {
            const notaOriginal = nota.notaConceito;
            /* eslint-disable */
            nota.notaOriginal = notaOriginal;
            /* eslint-enable */
          });
        });

        let listaTiposConceitos = [];
        if (Number(notasConceitos.Conceitos) === Number(dados.notaTipo)) {
          listaTiposConceitos = await obterListaConceitos(
            bimestrePesquisado.periodoFim
          );
        }

        const bimestreAtualizado = {
          fechamentoTurmaId: bimestrePesquisado.fechamentoTurmaId,
          descricao: bimestrePesquisado.descricao,
          numero: bimestrePesquisado.numero,
          alunos: [...bimestrePesquisado.alunos],
          avaliacoes: [...bimestrePesquisado.avaliacoes],
          periodoInicio: bimestrePesquisado.periodoInicio,
          periodoFim: bimestrePesquisado.periodoFim,
          mediaAprovacaoBimestre: dados.mediaAprovacaoBimestre,
          listaTiposConceitos,
          observacoes: bimestrePesquisado.observacoes,
          podeLancarNotaFinal: bimestrePesquisado.podeLancarNotaFinal,
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

        setAuditoriaInfo({
          auditoriaAlterado: dados.auditoriaAlterado,
          auditoriaInserido: dados.auditoriaInserido,
          auditoriaBimestreAlterado: dados.auditoriaBimestreAlterado,
          auditoriaBimestreInserido: dados.auditoriaBimestreInserido,
        });
      } else {
        setAuditoriaInfo({
          auditoriaAlterado: '',
          auditoriaInserido: '',
          auditoriaBimestreAlterado: '',
          auditoriaBimestreInserido: '',
        });
      }
      setCarregandoListaBimestres(false);
    }
  };

  const onClickCancelar = async cancelar => {
    if (cancelar) {
      obterDadosBimestres(disciplinaSelecionada, bimestreCorrente);
      dispatch(setModoEdicaoGeral(false));
      dispatch(setModoEdicaoGeralNotaFinal(false));
      dispatch(setExpandirLinha([]));
    }
  };

  const onChangeOrdenacao = bimestreOrdenado => {
    dispatch(setExpandirLinha([]));
    const bimestreAtualizado = {
      fechamentoTurmaId: bimestreOrdenado.fechamentoTurmaId,
      descricao: bimestreOrdenado.descricao,
      numero: bimestreOrdenado.numero,
      alunos: [...bimestreOrdenado.alunos],
      avaliacoes: [...bimestreOrdenado.avaliacoes],
      periodoInicio: bimestreOrdenado.periodoInicio,
      periodoFim: bimestreOrdenado.periodoFim,
      mediaAprovacaoBimestre: bimestreOrdenado.mediaAprovacaoBimestre,
      listaTiposConceitos: bimestreOrdenado.listaTiposConceitos,
      observacoes: bimestreOrdenado.observacoes,
      podeLancarNotaFinal: bimestreOrdenado.podeLancarNotaFinal,
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
                <Loader loading={carregandoDisciplina} tip="">
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
                    allowClear={false}
                  />
                </Loader>
              </div>
            </div>

            {disciplinaSelecionada ? (
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
                            ehProfessorCj={ehProfessorCj}
                            ehRegencia={ehRegencia}
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
                            ehProfessorCj={ehProfessorCj}
                            ehRegencia={ehRegencia}
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
                            ehProfessorCj={ehProfessorCj}
                            ehRegencia={ehRegencia}
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
                            ehProfessorCj={ehProfessorCj}
                            ehRegencia={ehRegencia}
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
                <div className="row mt-2 mb-2 mt-2">
                  <div className="col-md-12">
                    <ContainerAuditoria style={{ float: 'left' }}>
                      <span>
                        <p>{auditoriaInfo.auditoriaBimestreInserido || ''}</p>
                        <p>{auditoriaInfo.auditoriaBimestreAlterado || ''}</p>
                      </span>
                    </ContainerAuditoria>
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
