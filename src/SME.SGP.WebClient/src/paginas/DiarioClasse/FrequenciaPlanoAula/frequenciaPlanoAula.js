import React, { useEffect, useState, useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';
import { Switch } from 'antd';
import { CampoData, Auditoria, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import ListaFrequencia from '~/componentes-sgp/ListaFrequencia/listaFrequencia';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import CardCollapse from '~/componentes/cardCollapse';
import { Colors } from '~/componentes/colors';
import PlanoAula from '../PlanoAula/plano-aula';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import Alert from '~/componentes/alert';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import RotasDto from '~/dtos/rotasDto';
import { SelecionarDisciplina } from '~/redux/modulos/planoAula/actions';
import { stringNulaOuEmBranco } from '~/utils/funcoes/gerais';
import ModalMultiLinhas from '~/componentes/modalMultiLinhas';
import modalidade from '~/dtos/modalidade';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import Grid from '~/componentes/grid';
import { store } from '~/redux';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';

const FrequenciaPlanoAula = () => {
  const usuario = useSelector(state => state.usuario);
  const dispatch = useDispatch();

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [permiteRegistroFrequencia, setPermiteRegistroFrequencia] = useState(
    true
  );
  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const { turmaSelecionada, ehProfessor, ehProfessorCj } = usuario;
  const ehEja = !!(
    turmaSelecionada &&
    String(turmaSelecionada.modalidade) === String(modalidade.EJA)
  );
  const ehMedio = !!(
    turmaSelecionada &&
    String(turmaSelecionada.modalidade) === String(modalidade.ENSINO_MEDIO)
  );
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const anoLetivo = turmaSelecionada ? turmaSelecionada.anoLetivo : 0;

  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);

  const [listaDisciplinas, setListaDisciplinas] = useState();
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState();
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState();
  const [listaDatasAulas, setListaDatasAulas] = useState();
  const [dataSelecionada, setDataSelecionada] = useState();

  const [frequencia, setFrequencia] = useState();
  const [aulaId, setAulaId] = useState(0);
  const [frequenciaId, setFrequenciaId] = useState(0);
  const [
    carregandoDiasParaHabilitar,
    setCarregandoDiasParaHabilitar,
  ] = useState(false);
  const [exibirCardFrequencia, setExibirCardFrequencia] = useState(false);
  const [modoEdicaoFrequencia, setModoEdicaoFrequencia] = useState(false);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [diasParaHabilitar, setDiasParaHabilitar] = useState();
  const [auditoria, setAuditoria] = useState([]);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [modoEdicaoPlanoAula, setModoEdicaoPlanoAula] = useState(false);
  const [aula, setAula] = useState();
  const [auditoriaPlano, setAuditoriaPlano] = useState([]);
  const [planoAula, setPlanoAula] = useState({
    aulaId: 0,
    id: 0,
    qtdAulas: 0,
    temObjetivos: ehProfessor && !ehEja,
    descricao: null,
    desenvolvimentoAula: null,
    recuperacaoAula: null,
    licaoCasa: null,
    objetivosAprendizagemAula: [],
    migrado: false,
  });
  const [temObjetivos, setTemObjetivos] = useState(false);
  const [errosValidacaoPlano, setErrosValidacaoPlano] = useState([]);
  const [materias, setMaterias] = useState([]);
  const [mostrarErros, setMostarErros] = useState(false);

  const [carregandoSalvar, setCarregandoSalvar] = useState(false);

  const [planoAulaExpandido, setPlanoAulaExpandido] = useState(false);

  const dadosAulaFrequencia = useSelector(
    state => state.calendarioProfessor.dadosAulaFrequencia
  );

  const obterDatasDeAulasDisponiveis = useCallback(
    async disciplinaId => {
      setCarregandoDiasParaHabilitar(true);
      const datasDeAulas = await api
        .get(
          `v1/calendarios/frequencias/aulas/datas/${anoLetivo}/turmas/${turmaId}/disciplinas/${disciplinaId}`
        )
        .catch(e => {
          setCarregandoDiasParaHabilitar(false);
          erros(e);
        });

      setCarregandoDiasParaHabilitar(false);
      if (datasDeAulas && datasDeAulas.data && datasDeAulas.data.length) {
        setListaDatasAulas(datasDeAulas.data);
        const habilitar = datasDeAulas.data.map(item =>
          window.moment(item.data).format('YYYY-MM-DD')
        );
        setDiasParaHabilitar(habilitar);
      } else {
        setListaDatasAulas();
        setDiasParaHabilitar();
      }
    },
    [anoLetivo, turmaId]
  );

  const resetarTelaFrequencia = (naoDisciplina, naoData) => {
    if (!naoDisciplina) {
      setDisciplinaSelecionada(undefined);
      setDisciplinaIdSelecionada(undefined);
    }
    if (!naoData) {
      setDataSelecionada();
    }
    setFrequencia();
    setExibirCardFrequencia(false);
    setModoEdicaoFrequencia(false);
    setExibirAuditoria(true);
  };

  const setarDisciplina = async disciplinaId => {
    resetarTelaFrequencia(true);
    const disciplina = listaDisciplinas.find(
      disc => String(disc.codigoComponenteCurricular) === disciplinaId
    );
    setDisciplinaSelecionada(disciplina);
    setDisciplinaIdSelecionada(disciplinaId);
    if (disciplinaId) {
      await obterDatasDeAulasDisponiveis(disciplinaId);
    } else {
      setListaDatasAulas();
      setDiasParaHabilitar();
    }
  };

  const obterDisciplinas = useCallback(async () => {
    setCarregandoDisciplinas(true);
    const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
    );
    if (disciplinas.data && disciplinas.data.length) {
      setListaDisciplinas(disciplinas.data);
    }
    if (disciplinas.data && disciplinas.data.length === 1) {
      const disciplina = disciplinas.data[0];
      setDisciplinaSelecionada(disciplina);
      setDisciplinaIdSelecionada(String(disciplina.codigoComponenteCurricular));
      setDesabilitarDisciplina(true);
      if (!diasParaHabilitar) {
        await obterDatasDeAulasDisponiveis(
          disciplina.codigoComponenteCurricular
        );
      }
      dispatch(SelecionarDisciplina(disciplina));
    }
    setCarregandoDisciplinas(false);
  }, [turmaId, diasParaHabilitar, dispatch, obterDatasDeAulasDisponiveis]);

  useEffect(() => {
    async function buscarDisciplinas() {
      if (
        turmaId &&
        aulaId === 0 &&
        !disciplinaSelecionada &&
        !disciplinaIdSelecionada &&
        !listaDisciplinas &&
        !listaDatasAulas &&
        !diasParaHabilitar &&
        !frequencia
      ) {
        await obterDisciplinas(turmaId);
      }
    }

    buscarDisciplinas();
    const somenteConsultarFrequencia = verificaSomenteConsulta(permissoesTela);
    setSomenteConsulta(somenteConsultarFrequencia);
  }, [
    aulaId,
    diasParaHabilitar,
    disciplinaIdSelecionada,
    disciplinaSelecionada,
    frequencia,
    listaDatasAulas,
    listaDisciplinas,
    obterDisciplinas,
    permissoesTela,
    turmaId,
  ]);

  useEffect(() => {
    resetarTelaFrequencia();
    setAulaId(0);
    setDisciplinaSelecionada();
    setDisciplinaIdSelecionada();
    setListaDisciplinas();
    setListaDatasAulas();
    setDiasParaHabilitar();
    setDesabilitarDisciplina(false);
  }, [turmaSelecionada.turma]);

  useEffect(() => {
    const desabilitar =
      frequenciaId > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);
  }, [
    frequenciaId,
    permissoesTela.podeAlterar,
    permissoesTela.podeIncluir,
    somenteConsulta,
  ]);

  const obterListaFrequencia = async id => {
    setAulaId(id);

    const frequenciaAlunos = await api
      .get(`v1/calendarios/frequencias`, { params: { aulaId: id } })
      .catch(e => erros(e));
    if (frequenciaAlunos && frequenciaAlunos.data) {
      setFrequenciaId(frequenciaAlunos.data.id);
      setAuditoria({
        criadoPor: frequenciaAlunos.data.criadoPor,
        criadoRf: frequenciaAlunos.data.criadoRf,
        criadoEm: frequenciaAlunos.data.criadoEm,
        alteradoPor: frequenciaAlunos.data.alteradoPor,
        alteradoRf: frequenciaAlunos.data.alteradoRf,
        alteradoEm: frequenciaAlunos.data.alteradoEm,
      });
      setExibirAuditoria(true);
      setFrequencia(frequenciaAlunos.data.listaFrequencia);
      setPermiteRegistroFrequencia(!frequenciaAlunos.data.desabilitado);
    }
  };

  const [carregandoMaterias, setCarregandoMaterias] = useState(false);

  const obterPlanoAula = useCallback(
    async dadosAula => {
      setCarregandoMaterias(true);

      const idAula = dadosAula.idAula || dadosAula[0].idAula;
      const plano = await api
        .get(`v1/planos/aulas/${idAula}`)
        .then(resp => {
          return resp;
        })
        .catch(e => {
          setPlanoAulaExpandido(false);
          erros(e);
        });

      const dadosPlano = plano && plano.data;

      if (dadosPlano) {
        planoAula.qtdAulas = dadosPlano.qtdAulas;
        if (dadosPlano.id > 0) {
          dadosPlano.objetivosAprendizagemAula.forEach(objetivo => {
            objetivo.selected = true;
          });
          dadosPlano.objetivosAprendizagemAula = [
            ...dadosPlano.objetivosAprendizagemAula,
          ];
          dadosPlano.temObjetivos =
            (disciplinaSelecionada.regencia || ehProfessor) && !ehEja;
          setPlanoAula(dadosPlano);
          const audPlano = {
            criadoEm: dadosPlano.criadoEm,
            criadoPor: dadosPlano.criadoPor,
            alteradoEm: dadosPlano.alteradoEm,
            alteradoPor: dadosPlano.alteradoPor,
          };
          setAuditoriaPlano(audPlano);
        } else {
          setModoEdicaoPlanoAula(false);
        }
      }

      if (disciplinaSelecionada.regencia || ehProfessor || ehProfessorCj) {
        let disciplinas = {};
        if (disciplinaSelecionada.regencia) {
          setTemObjetivos(true);
          disciplinas = await api.get(
            `v1/professores/turmas/${turmaId}/disciplinas/planejamento?codigoDisciplina=${disciplinaSelecionada.codigoComponenteCurricular}&regencia=true`
          );
          if (disciplinas.data && disciplinas.data.length > 0) {
            const disciplinasRegencia = [];
            disciplinas.data.forEach(disciplina => {
              disciplinasRegencia.push({
                id: disciplina.codigoComponenteCurricular,
                descricao: disciplina.nome,
              });
            });
            setMaterias([...disciplinasRegencia]);
          }
        } else {
          const dataAula = dadosAula.data || dadosAula[0].data;
          disciplinas = await api.get(
            `v1/objetivos-aprendizagem/disciplinas/turmas/${turmaId}/componentes/${disciplinaSelecionada.codigoComponenteCurricular}?dataAula=${dataAula}`
          );
          const dadosDisciplinas = disciplinas.data;
          if (dadosDisciplinas) {
            setTemObjetivos(true);
            setMaterias([...dadosDisciplinas]);
          } else {
            disciplinas = await api.get(
              `v1/professores/turmas/${turmaId}/disciplinas/planejamento?codigoDisciplina=${disciplinaSelecionada.codigoComponenteCurricular}&regencia=false`
            );
            if (disciplinas.data && disciplinas.data.length > 0) {
              const dados = disciplinas.data[0];
              setTemObjetivos(dados.possuiObjetivos);
              if (dados.possuiObjetivos) {
                const materia = {
                  id: dados.codigoComponenteCurricular,
                  descricao: dados.nome,
                };
                const mat = [];
                mat.push(materia);
                setMaterias([...mat]);
              }
            }
          }
        }
      }
      setCarregandoMaterias(false);
    },
    [
      disciplinaSelecionada,
      ehEja,
      ehProfessor,
      ehProfessorCj,
      planoAula,
      turmaId,
    ]
  );

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

  const aposSalvarFrequencia = () => {
    setExibirCardFrequencia(false);
    setModoEdicaoFrequencia(false);
  };

  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const onSalvarFrequencia = click => {
    setCarregandoSalvar(true);
    return new Promise((resolve, reject) => {
      const valorParaSalvar = {
        aulaId,
        listaFrequencia: frequencia,
      };
      return api
        .post(`v1/calendarios/frequencias`, valorParaSalvar)
        .then(salvouFrequencia => {
          if (salvouFrequencia && salvouFrequencia.status === 200) {
            sucesso('Frequência realizada com sucesso.');
            if (click) {
              aposSalvarFrequencia();
            }
            setCarregandoSalvar(false);
            setTimeout(() => {
              setCarregandoGeral(false);
            }, 1000);
            resolve(true);
            return true;
          }
          resolve(false);
          return false;
        })
        .catch(e => {
          setCarregandoSalvar(false);
          setTimeout(() => {
            setCarregandoGeral(false);
          }, 1000);
          erros(e);
          reject(e);
        });
    });
  };

  const validaPlanoAula = () => {
    if (
      !temObjetivos &&
      ehProfessorCj &&
      stringNulaOuEmBranco(planoAula.descricao)
    ) {
      errosValidacaoPlano.push(
        'Meus objetivos - O campo meus objetivos específicos é obrigatório'
      );
    }
    if (stringNulaOuEmBranco(planoAula.desenvolvimentoAula)) {
      errosValidacaoPlano.push(
        'Desenvolvimento da aula - A sessão de desenvolvimento da aula deve ser preenchida'
      );
    }
    if (
      !ehProfessorCj &&
      temObjetivos &&
      !ehEja &&
      !ehMedio &&
      !planoAula.migrado &&
      planoAula.objetivosAprendizagemJurema.length === 0
    ) {
      errosValidacaoPlano.push(
        'Objetivos de aprendizagem - É obrigatório selecionar ao menos um objetivo de aprendizagem'
      );
    }
  };

  const obterAulaSelecionada = useCallback(
    async data => {
      if (listaDatasAulas) {
        const aulaDataSelecionada = listaDatasAulas.filter(item => {
          return (
            window.moment(item.data).format('DD/MM/YYYY') ===
            window.moment(data).format('DD/MM/YYYY')
          );
        });

        return aulaDataSelecionada;
      }
      return null;
    },
    [listaDatasAulas]
  );

  const onSalvarPlanoAula = async () => {
    const objetivosId = [];
    planoAula.objetivosAprendizagemAula.forEach(obj => {
      if (obj.selected) {
        objetivosId.push(obj.id);
      }
    });
    const plano = {
      descricao: planoAula.descricao,
      desenvolvimentoAula: planoAula.desenvolvimentoAula,
      recuperacaoAula: planoAula.recuperacaoAula,
      licaoCasa: planoAula.licaoCasa,
      aulaId,
      objetivosAprendizagemJurema: temObjetivos ? objetivosId : [],
    };
    planoAula.objetivosAprendizagemJurema = [...objetivosId];

    await validaPlanoAula();
    if (errosValidacaoPlano.length === 0) {
      await api
        .post('v1/planos/aulas', plano)
        .then(salvouPlano => {
          if (salvouPlano && salvouPlano.status === 200) {
            sucesso('Plano de aula salvo com sucesso.');
            setModoEdicaoPlanoAula(false);
            setPlanoAulaExpandido(false);
          }
        })
        .catch(e => {
          erros(e);
        });
    } else {
      setMostarErros(true);
    }
  };

  const onClickVoltar = async () => {
    if (!desabilitarCampos && (modoEdicaoFrequencia || modoEdicaoPlanoAula)) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        if (modoEdicaoFrequencia) {
          await onSalvarFrequencia();
        }
        if (modoEdicaoPlanoAula) {
          await onSalvarPlanoAula();
        }
        irParaHome();
      } else {
        irParaHome();
      }
    } else {
      irParaHome();
    }
  };

  const resetarPlanoAula = useCallback(async () => {
    planoAula.aulaId = 0;
    planoAula.descricao = null;
    setTemObjetivos(false);
    planoAula.qtdAulas = 0;
    planoAula.desenvolvimentoAula = null;
    planoAula.licaoCasa = null;
    planoAula.recuperacaoAula = null;
    planoAula.objetivosAprendizagemAula = [];
    planoAula.objetivosAprendizagemAula = [
      ...planoAula.objetivosAprendizagemAula,
    ];
    const materiasVazia = [];
    setModoEdicaoPlanoAula(false);
    setMaterias([...materiasVazia]);
    setPlanoAula(planoAula);
  }, [planoAula]);

  const onClickCancelar = async () => {
    if (!desabilitarCampos && (modoEdicaoFrequencia || modoEdicaoPlanoAula)) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        obterListaFrequencia(aulaId);
        setModoEdicaoFrequencia(false);
        const aulaSelecionada = await obterAulaSelecionada(dataSelecionada);
        obterPlanoAula(aulaSelecionada);
        setModoEdicaoPlanoAula(false);
        resetarPlanoAula();
      }
    }
  };

  const onClickSalvar = click => {
    setCarregandoGeral(true);
    if (modoEdicaoFrequencia && permiteRegistroFrequencia) {
      onSalvarFrequencia(click);
    }
    if (modoEdicaoPlanoAula) {
      onSalvarPlanoAula();
    }
    store.dispatch(salvarDadosAulaFrequencia());
    setTimeout(() => {
      setDataSelecionada();
      setCarregandoGeral(false);
    }, 1000);
  };

  const onCloseErros = () => {
    setErrosValidacaoPlano([]);
    setMostarErros(false);
  };

  const onClickFrequencia = () => {
    if (
      !desabilitarCampos &&
      !exibirCardFrequencia &&
      permiteRegistroFrequencia
    ) {
      let temAulas = false;
      if (frequencia && frequencia.length) {
        const aulas = frequencia.filter(
          item => item.aulas && item.aulas.length
        );
        temAulas = !!(aulas && aulas.length);
      }
      if (temAulas) {
        setModoEdicaoFrequencia(true);
      }
    }
    setExibirCardFrequencia(!exibirCardFrequencia);
  };

  const onClickPlanoAula = useCallback(() => {
    setPlanoAulaExpandido(!planoAulaExpandido);
  }, [planoAulaExpandido]);

  useEffect(() => {
    if (!planoAula.aulaId && planoAulaExpandido && aula) {
      obterPlanoAula(aula);
    }
  }, [obterPlanoAula, planoAula.aulaId, planoAulaExpandido, aula]);

  const onChangeDisciplinas = useCallback(
    async disciplinaId => {
      if (!disciplinaId) store.dispatch(salvarDadosAulaFrequencia());
      if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
        const confirmarParaSalvar = await pergutarParaSalvar();
        if (confirmarParaSalvar) {
          if (modoEdicaoFrequencia) {
            await onSalvarFrequencia();
            setarDisciplina(disciplinaId);
          }
          if (modoEdicaoPlanoAula) {
            await onSalvarPlanoAula();
            setarDisciplina(disciplinaId);
          }
        } else {
          setarDisciplina(disciplinaId);
          resetarPlanoAula();
        }
      } else {
        setarDisciplina(disciplinaId);
      }
    },
    [
      modoEdicaoFrequencia,
      modoEdicaoPlanoAula,
      onSalvarFrequencia,
      onSalvarPlanoAula,
      resetarPlanoAula,
      setarDisciplina,
    ]
  );

  const [temAvaliacao, setTemAvaliacao] = useState(undefined);
  const [dataVigente, setDataVigente] = useState(false);

  const obterAvaliacao = useCallback(async () => {
    if (dataSelecionada && planoAula) {
      if (planoAula.idAtividadeAvaliativa) {
        setDataVigente(
          window.moment(dataSelecionada).isSameOrAfter(window.moment(), 'day')
        );
        setTemAvaliacao(planoAula.idAtividadeAvaliativa);
      } else {
        setTemAvaliacao(undefined);
      }
    }
  }, [dataSelecionada, planoAula]);

  useEffect(() => {
    obterAvaliacao();
  }, [obterAvaliacao]);

  useEffect(() => {
    return () => {
      store.dispatch(salvarDadosAulaFrequencia());
    };
  }, []);

  const [exibeEscolhaAula, setExibeEscolhaAula] = useState(false);
  const [ehAulaCj, setEhAulaCj] = useState(false);

  const aoTrocarAulaCj = () => {
    setEhAulaCj(!ehAulaCj);
  };

  useEffect(() => {
    if (exibeEscolhaAula) {
      setCarregandoMaterias(true);
      const aulaDataSelecionada = obterAulaSelecionada(dataSelecionada);
      const aulaSelecionada = aulaDataSelecionada.find(
        item => item.aulaCJ === ehAulaCj
      );

      if (aulaSelecionada) {
        setAula(aulaSelecionada);
        if (aulaSelecionada && aulaSelecionada.idAula) {
          obterListaFrequencia(aulaSelecionada.idAula);
        }
      }
      setCarregandoMaterias(false);
    }
  }, [dataSelecionada, ehAulaCj, exibeEscolhaAula, obterAulaSelecionada]);

  const validaSeTemIdAula = useCallback(
    async data => {
      resetarTelaFrequencia(true, true);

      const aulaDataSelecionada = await obterAulaSelecionada(data);

      if (aulaDataSelecionada && aulaDataSelecionada.length) {
        if (
          usuario &&
          !usuario.ehProfessor &&
          !usuario.ehProfessorCj &&
          !usuario.ehProfessorPoa &&
          aulaDataSelecionada.length > 1
        ) {
          setExibeEscolhaAula(true);
        } else {
          const aulaSelecionada = aulaDataSelecionada.find(
            item => item.aulaCJ === usuario.ehProfessorCj
          );

          if (aulaSelecionada) {
            setAula(aulaSelecionada);
            if (aulaSelecionada && aulaSelecionada.idAula) {
              obterListaFrequencia(aulaSelecionada.idAula);
            }
          }
        }
      }
      setTimeout(() => {
        setCarregandoGeral(false);
      }, 1000);
    },
    [obterAulaSelecionada, usuario]
  );

  const onChangeData = useCallback(
    async data => {
      setDataSelecionada(data);
      setExibirCardFrequencia(false);

      setAula();
      resetarPlanoAula();
      if (planoAulaExpandido) onClickPlanoAula();

      setCarregandoGeral(true);

      if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
        const confirmarParaSalvar = await pergutarParaSalvar();
        if (confirmarParaSalvar) {
          if (modoEdicaoFrequencia) {
            await onSalvarFrequencia();
          }
          if (modoEdicaoPlanoAula) {
            await onSalvarPlanoAula();
          }
          await validaSeTemIdAula(data);
        } else {
          await validaSeTemIdAula(data);
        }
      } else {
        await validaSeTemIdAula(data);
      }
    },
    [
      modoEdicaoFrequencia,
      modoEdicaoPlanoAula,
      planoAulaExpandido,
      onClickPlanoAula,
      onSalvarFrequencia,
      onSalvarPlanoAula,
      resetarPlanoAula,
      validaSeTemIdAula,
    ]
  );

  useEffect(() => {
    if (
      dadosAulaFrequencia &&
      Object.entries(dadosAulaFrequencia).length &&
      dadosAulaFrequencia.disciplinaId &&
      listaDisciplinas &&
      listaDisciplinas.length &&
      !disciplinaIdSelecionada
    ) {
      onChangeDisciplinas(String(dadosAulaFrequencia.disciplinaId));
    }
    if (
      dadosAulaFrequencia &&
      Object.entries(dadosAulaFrequencia).length &&
      dadosAulaFrequencia.dia &&
      diasParaHabilitar &&
      diasParaHabilitar.length &&
      !dataSelecionada
    ) {
      onChangeData(window.moment(dadosAulaFrequencia.dia));
    }
  }, [
    dadosAulaFrequencia,
    listaDisciplinas,
    dataSelecionada,
    diasParaHabilitar,
    disciplinaIdSelecionada,
    onChangeDisciplinas,
    onChangeData,
  ]);

  const onChangeFrequencia = () => {
    setModoEdicaoFrequencia(true);
  };

  const LinkAcao = styled.span`
    cursor: pointer;
    font-weight: bold;
  `;

  const Label = styled.label``;

  const acessarEditarAvaliacao = () => {
    history.push(`${RotasDto.CADASTRO_DE_AVALIACAO}/editar/${temAvaliacao}`);
  };

  const acessarNotasConceitos = () => {
    history.push(RotasDto.NOTAS);
  };

  return (
    <>
      {usuario && turmaSelecionada.turma ? null : (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'frequencia-selecione-turma',
            mensagem: 'Você precisa escolher uma turma',
          }}
          className="mb-2"
        />
      )}
      {temAvaliacao ? (
        <div className="row">
          <Grid cols={12} className="px-4">
            <div
              className="alert alert-info alert-dismissible fade show text-center"
              role="alert"
            >
              Atenção, existe uma avaliação neste dia:
              <LinkAcao onClick={acessarEditarAvaliacao}>
                Editar Avaliação
              </LinkAcao>
              {dataVigente && (
                <>
                  {' ou '}
                  <LinkAcao onClick={acessarNotasConceitos}>
                    Acessar Notas e Conceitos
                  </LinkAcao>
                </>
              )}
            </div>
          </Grid>
        </div>
      ) : null}
      <Cabecalho pagina="Frequência/Plano de aula" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <Button
                id={shortid.generate()}
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                id={shortid.generate()}
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-2"
                onClick={onClickCancelar}
                disabled={!modoEdicaoFrequencia && !modoEdicaoPlanoAula}
              />
              <Loader loading={carregandoSalvar} tip="">
                <Button
                  id={shortid.generate()}
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={() => onClickSalvar(true)}
                  disabled={
                    desabilitarCampos ||
                    (!modoEdicaoFrequencia && !modoEdicaoPlanoAula)
                  }
                />
              </Loader>
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <Loader loading={carregandoDisciplinas} tip="">
                <SelectComponent
                  id="disciplina"
                  name="disciplinaId"
                  lista={listaDisciplinas || []}
                  valueOption="codigoComponenteCurricular"
                  valueText="nome"
                  valueSelect={disciplinaIdSelecionada}
                  onChange={onChangeDisciplinas}
                  placeholder="Selecione um componente curricular"
                  disabled={
                    !listaDisciplinas ||
                    !turmaSelecionada.turma ||
                    desabilitarDisciplina
                  }
                />
              </Loader>
            </div>
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
              <CampoData
                valor={dataSelecionada}
                onChange={onChangeData}
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                desabilitado={
                  !listaDisciplinas ||
                  !disciplinaIdSelecionada ||
                  !diasParaHabilitar ||
                  carregandoDiasParaHabilitar
                }
                carregando={carregandoDiasParaHabilitar}
                diasParaHabilitar={diasParaHabilitar}
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
              {exibeEscolhaAula && (
                <>
                  <Switch
                    onChange={aoTrocarAulaCj}
                    checked={ehAulaCj}
                    size="small"
                    className="mr-2"
                  />
                  <Label className="my-auto">Aula CJ</Label>
                </>
              )}
            </div>
          </div>
          <div className="row">
            <Loader loading={carregandoGeral} className="w-100 my-2">
              {disciplinaSelecionada && dataSelecionada ? (
                <>
                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                    <CardCollapse
                      key="frequencia-collapse"
                      onClick={onClickFrequencia}
                      titulo="Frequência"
                      indice="frequencia-collapse"
                      show={exibirCardFrequencia}
                      alt="card-collapse-frequencia"
                    >
                      {frequencia && frequencia.length ? (
                        <>
                          <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                            <Ordenacao
                              conteudoParaOrdenar={frequencia}
                              ordenarColunaNumero="numeroAlunoChamada"
                              ordenarColunaTexto="nomeAluno"
                              retornoOrdenado={retorno =>
                                setFrequencia(retorno)
                              }
                            />
                            <ListaFrequencia
                              dados={frequencia}
                              frequenciaId={frequenciaId}
                              onChangeFrequencia={onChangeFrequencia}
                              permissoesTela={permissoesTela}
                            />
                          </div>
                          {exibirAuditoria && (
                            <Auditoria
                              className="mt-2"
                              criadoEm={auditoria.criadoEm}
                              criadoPor={auditoria.criadoPor}
                              alteradoPor={auditoria.alteradoPor}
                              alteradoEm={auditoria.alteradoEm}
                            />
                          )}
                        </>
                      ) : null}
                    </CardCollapse>
                  </div>
                  <div className="col-sm-12 col-md-12 col-lg-12">
                    <PlanoAula
                      onClick={onClickPlanoAula}
                      disciplinaIdSelecionada={disciplinaIdSelecionada}
                      dataSelecionada={dataSelecionada}
                      planoAula={planoAula}
                      ehProfessorCj={ehProfessorCj}
                      carregandoMaterias={carregandoMaterias}
                      listaMaterias={materias}
                      dataAula={aula && aula.data ? aula.data : null}
                      ehEja={ehEja}
                      ehMedio={ehMedio}
                      setModoEdicao={e => setModoEdicaoPlanoAula(e)}
                      setTemObjetivos={e => setTemObjetivos(e)}
                      permissoesTela={permissoesTela}
                      somenteConsulta={somenteConsulta}
                      expandido={planoAulaExpandido}
                      temObjetivos={temObjetivos}
                      temAvaliacao={temAvaliacao}
                      auditoria={auditoriaPlano}
                      ehRegencia={
                        disciplinaSelecionada
                          ? disciplinaSelecionada.regencia
                          : false
                      }
                    />
                  </div>
                </>
              ) : null}
            </Loader>
          </div>
        </div>
        <ModalMultiLinhas
          key="errosBimestre"
          visivel={mostrarErros}
          onClose={onCloseErros}
          type="error"
          conteudo={errosValidacaoPlano}
          titulo="Erros plano de aula"
        />
      </Card>
    </>
  );
};

export default FrequenciaPlanoAula;
