import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { useEffect, useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import * as Yup from 'yup';
import moment from 'moment';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import CampoTexto from '~/componentes/campoTexto';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import RadioGroupButton from '~/componentes/radioGroupButton';
import SelectComponent from '~/componentes/select';
import {
  confirmar,
  erros,
  sucesso,
  erro,
  exibirAlerta,
} from '~/servicos/alertas';
import api from '~/servicos/api';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import RotasDTO from '~/dtos/rotasDto';
import { ModalConteudoHtml, Loader } from '~/componentes';
import Alert from '~/componentes/alert';
import modalidade from '~/dtos/modalidade';
import ServicoAula from '~/servicos/Paginas/ServicoAula';
import { removerAlerta } from '~/redux/modulos/alertas/actions';
import { store } from '~/redux';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

const CadastroAula = ({ match, location }) => {
  const usuario = useSelector(state => state.usuario);
  const permissaoTela = useSelector(
    state => state.usuario.permissoes[RotasDTO.CALENDARIO_PROFESSOR]
  );
  const diaAula = useSelector(
    state => state.calendarioProfessor.diaSelecionado
  );
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const ueId = turmaSelecionada ? turmaSelecionada.unidadeEscolar : 0;

  const [dataAula, setDataAula] = useState();
  const [idAula, setIdAula] = useState(0);
  const [auditoria, setAuditoria] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(!match.params.id);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [validacoes, setValidacoes] = useState({});
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [quantidadeMaximaAulas, setQuantidadeMaximaAulas] = useState(undefined);
  const [controlaQuantidadeAula, setControlaQuantidadeAula] = useState(true);
  const [refForm, setRefForm] = useState({});
  const [refFormRecorrencia, setRefFormRecorrencia] = useState({});
  const [ehReposicao, setEhReposicao] = useState(false);
  const [quantidadeRecorrencia, setQuantidadeRecorrencia] = useState(0);
  const [existeFrequenciaPlanoAula, setExisteFrequenciaPlanoAula] = useState(
    false
  );
  const [somenteLeitura, setSomenteLeitura] = useState(false);
  const [
    idNotificacaoSomenteLeitura,
    setIdNotificacaoSomenteLeitura,
  ] = useState(null);
  const [ehAulaUnica, setEhAulaUnica] = useState(false);
  const [ehRegencia, setEhRegencia] = useState(false);
  const [ehEJA, setEhEja] = useState(false);
  const [ehRecorrencia, setEhRecorrencia] = useState(false);
  const [
    visualizarFormExcRecorrencia,
    setVisualizarFormExcRecorrencia,
  ] = useState(false);
  const [dentroPeriodo, setDentroPeriodo] = useState(true);

  const [inicial, setInicial] = useState({
    tipoAula: 1,
    disciplinaId: undefined,
    disciplinaCompartilhadaId: undefined,
    quantidadeTexto: '',
    quantidadeRadio: 0,
    dataAula: '',
    recorrenciaAula: '',
    quantidade: 0,
    tipoCalendarioId: '',
    ueId: '',
    turmaId: '',
    dataAulaCompleta: undefined,
  });

  const [aula] = useState(inicial);
  const [idDisciplina, setIdDisciplina] = useState();
  const [disciplinaCompartilhada, setDisciplinaCompartilhada] = useState(false);
  const [
    listaDisciplinasCompartilhadas,
    setListaDisciplinasCompartilhadas,
  ] = useState([]);

  const opcoesTipoAula = [
    { label: 'Normal', value: 1 },
    { label: 'Reposição', value: 2 },
  ];

  const opcoesQuantidadeAulas = [
    {
      label: '1',
      value: 1,
      disabled:
        (quantidadeMaximaAulas < 1 && controlaQuantidadeAula) ||
        (ehRegencia && ehEJA && !ehReposicao),
    },
    {
      label: '2',
      value: 2,
      disabled:
        (quantidadeMaximaAulas < 2 && controlaQuantidadeAula) ||
        (ehRegencia && ehEJA && !ehReposicao),
    },
  ];

  const recorrencia = {
    AULA_UNICA: 1,
    REPETIR_BIMESTRE_ATUAL: 2,
    REPETIR_TODOS_BIMESTRES: 3,
  };

  const [opcoesRecorrencia, setOpcoesRecorrencia] = useState([
    { label: 'Aula única', value: recorrencia.AULA_UNICA },
    {
      label: 'Repetir no Bimestre atual',
      value: recorrencia.REPETIR_BIMESTRE_ATUAL,
    },
    {
      label: 'Repetir em todos os Bimestres',
      value: recorrencia.REPETIR_TODOS_BIMESTRES,
    },
  ]);

  const [opcoesExcluirRecorrencia, setOpcoesExcluirRecorrencia] = useState([
    { label: 'Somente o dia', value: 1 },
    { label: 'Bimestre atual', value: 2 },
    { label: 'Todos os bimestres', value: 3 },
  ]);

  const valoresIniciaisExclusao = {
    tipoRecorrenciaExclusao: recorrencia.AULA_UNICA,
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  useEffect(() => {
    return () => {
      store.dispatch(removerAlerta(idNotificacaoSomenteLeitura));
    };
  }, []);

  const [desabilitaPorGrade, setDesabilitaPorGrade] = useState(false);
  const [carregandoSalvar, setCarregandoSalvar] = useState(false);

  const [desabilitaPorQuantidade, setDesabilitaPorQuantidade] = useState(false);

  const onChangeDisciplinas = async (id, listaDisc, isReposicao) => {
    onChangeCampos();

    setIdDisciplina(id);

    const lista =
      (listaDisciplinas && listaDisciplinas.length > 0 && listaDisciplinas) ||
      listaDisc ||
      [];

    if (!lista || lista.length === 0) return;

    if (id) {
      const disciplina = lista.find(
        d => String(d.codigoComponenteCurricular) === String(id)
      );

      if (!disciplina) return;

      const regencia = !!disciplina.regencia;
      setEhRegencia(regencia);

      let resultado;

      if (disciplina && !disciplina.territorioSaber) {
        resultado = await api
          .get(
            `v1/grades/aulas/turmas/${turmaId}/disciplinas/${id}?ehRegencia=${regencia}`,
            {
              params: {
                data: dataAula ? dataAula.format('YYYY-MM-DD') : null,
              },
            }
          )
          .then(res => res)
          .catch(err => {
            const mensagemErro =
              err &&
              err.response &&
              err.response.data &&
              err.response.data.mensagens;

            if (mensagemErro) {
              erro(mensagemErro.join(','));
              return null;
            }

            erro('Ocorreu um erro, por favor contate o suporte');

            return null;
          });
      }

      if (resultado) {
        if (resultado.status === 200) {
          setControlaQuantidadeAula(
            resultado.data.quantidadeAulasRestante <= 1
          );
          setQuantidadeMaximaAulas(resultado.data.quantidadeAulasRestante);
          setDesabilitaPorGrade(resultado.data.quantidadeAulasRestante < 1);
          setDesabilitaPorQuantidade(
            resultado.data.quantidadeAulasRestante <= 1
          );
          if (
            resultado.data.quantidadeAulasRestante > 0 &&
            resultado.data.quantidadeAulasRestante <= 2
          ) {
            refForm.setFieldValue(
              'quantidadeRadio',
              resultado.data.quantidadeAulasRestante
            );
          }
        } else if (resultado.status === 204) {
          setControlaQuantidadeAula(false);
        }
      }
    }
  };

  useEffect(() => {
    if (idDisciplina && listaDisciplinas.length) {
      const disciplina = listaDisciplinas.find(
        item =>
          item.codigoComponenteCurricular.toString() === idDisciplina.toString()
      );
      if (disciplina && disciplina[0])
        setDisciplinaCompartilhada(disciplina[0].compartilhada);
    } else if (refForm && refForm.setFieldValue)
      refForm.setFieldValue('quantidadeTexto', '');
  }, [idDisciplina, listaDisciplinas, refForm]);

  const buscarDisciplinasCompartilhadas = async () => {
    const disciplinas = await api.get(
      `v1/professores/turmas/${turmaId}/docencias-compartilhadas/disciplinas`
    );
    return disciplinas.data;
  };

  const trataSomenteLeitura = async () => {
    if (somenteLeitura) {
      const id = exibirAlerta(
        'warning',
        'Você possui permissão somente de leitura nesta aula',
        true
      );

      setIdNotificacaoSomenteLeitura(id);
      setListaDisciplinas(await buscarDisciplinasCompartilhadas());
    }
  };

  useEffect(() => {
    trataSomenteLeitura();
  }, [somenteLeitura]);

  const buscarDisciplinas = async () => {
    setListaDisciplinasCompartilhadas(await buscarDisciplinasCompartilhadas());
  };

  useEffect(() => {
    if (disciplinaCompartilhada) buscarDisciplinas();
  }, [disciplinaCompartilhada]);

  const getRecorrenciasHabilitadas = (opcoes, dadosRecorrencia) => {
    opcoes.forEach(item => {
      if (
        item.value === dadosRecorrencia.recorrenciaAula ||
        item.value === recorrencia.AULA_UNICA
      ) {
        item.disabled = false;
      } else {
        item.disabled = true;
      }
    });
    return opcoes;
  };

  const consultaPorId = async id => {
    const buscaAula = await api
      .get(`v1/calendarios/professores/aulas/${id}`)
      .catch(e => {
        if (
          e &&
          e.response &&
          e.response.data &&
          Array.isArray(e.response.data)
        ) {
          erros(e);
        }
      });
    setNovoRegistro(false);
    if (buscaAula && buscaAula.data) {
      setDataAula(moment(buscaAula.data.dataAula));
      const respRecorrencia = await api.get(
        `v1/calendarios/professores/aulas/${id}/recorrencias/serie`
      );
      const dadosRecorrencia = respRecorrencia.data;

      if (respRecorrencia && dadosRecorrencia) {
        setEhAulaUnica(
          dadosRecorrencia.recorrenciaAula === recorrencia.AULA_UNICA
        );

        setExisteFrequenciaPlanoAula(
          dadosRecorrencia.existeFrequenciaOuPlanoAula
        );
      }

      if (
        respRecorrencia &&
        dadosRecorrencia &&
        dadosRecorrencia.recorrenciaAula !== recorrencia.AULA_UNICA
      ) {
        setQuantidadeRecorrencia(dadosRecorrencia.quantidadeAulasRecorrentes);

        setOpcoesRecorrencia([
          ...getRecorrenciasHabilitadas(opcoesRecorrencia, dadosRecorrencia),
        ]);
        setOpcoesExcluirRecorrencia([
          ...getRecorrenciasHabilitadas(
            opcoesExcluirRecorrencia,
            dadosRecorrencia
          ),
        ]);
      }

      setSomenteLeitura(buscaAula.data.somenteLeitura);

      const val = {
        tipoAula: buscaAula.data.tipoAula,
        disciplinaId: buscaAula.data.disciplinaId.toString(),
        disciplinaCompartilhadaId:
          buscaAula.data.disciplinaCompartilhadaId &&
          buscaAula.data.disciplinaCompartilhadaId.toString(),
        dataAula: buscaAula.data.dataAula
          ? window.moment(buscaAula.data.dataAula)
          : window.moment(),
        recorrenciaAula: recorrencia.AULA_UNICA,
        id: buscaAula.data.id,
        tipoCalendarioId: buscaAula.data.tipoCalendarioId,
        ueId: buscaAula.data.ueId,
        turmaId: buscaAula.data.turmaId,
        dataAulaCompleta: window.moment(buscaAula.data.dataAula),
      };
      if (buscaAula.data.quantidade > 0 && buscaAula.data.quantidade < 3) {
        val.quantidadeRadio = buscaAula.data.quantidade;
        val.quantidadeTexto = '';
      } else if (
        buscaAula.data.quantidade > 0 &&
        buscaAula.data.quantidade > 2
      ) {
        val.quantidadeTexto = buscaAula.data.quantidade;
      }
      setIdDisciplina(buscaAula.data.disciplinaId.toString());
      setInicial(val);
      setAuditoria({
        criadoPor: buscaAula.data.criadoPor,
        criadoRf: buscaAula.data.criadoRF > 0 ? buscaAula.data.criadoRF : '',
        criadoEm: buscaAula.data.criadoEm,
        alteradoPor: buscaAula.data.alteradoPor,
        alteradoRf:
          buscaAula.data.alteradoRF > 0 ? buscaAula.data.alteradoRF : '',
        alteradoEm: buscaAula.data.alteradoEm,
      });
      setExibirAuditoria(true);
      setDentroPeriodo(buscaAula.data.dentroPeriodo);
    }
  };

  const validaF5 = () => {
    // TODO
    // Manter enquanto não é realizado o refactor da tela e do calendário!
    // Somente quando for novo registro, ao dar F5 a página perde a data selecionada no calendário do professor!
    setCarregandoSalvar(true);
    setTimeout(() => {
      history.push(RotasDTO.CALENDARIO_PROFESSOR);
      setCarregandoSalvar(false);
    }, 2000);
  };

  const validarConsultaModoEdicaoENovo = async () => {
    setBreadcrumbManual(
      match.url,
      'Cadastro de Aula',
      '/calendario-escolar/calendario-professor'
    );

    if (match?.params?.id) {
      setNovoRegistro(false);
      setIdAula(match.params.id);
      consultaPorId(match.params.id);
    } else if (diaAula) {
      setNovoRegistro(true);
      setDataAula(window.moment(diaAula));
      if (refForm?.setFieldValue) {
        refForm.setFieldValue('dataAulaCompleta', window.moment(diaAula));
      }
    } else if (!valorNuloOuVazio(location.search)) {
      const query = queryString.parse(location.search);
      if (refForm?.setFieldValue) {
        refForm.setFieldValue('dataAulaCompleta', window.moment(query.diaAula));
        setInicial(valoresAntigos => ({
          ...valoresAntigos,
          dataAulaCompleta: window.moment(query.diaAula),
        }));
      }
      setNovoRegistro(true);
    } else {
      validaF5();
    }
  };

  useEffect(() => {
    async function obterListaDisciplinas() {
      try {
        setCarregandoSalvar(true);
        const { data, status } = await api.get(
          `v1/professores/turmas/${turmaId}/disciplinas`
        );

        if (data && status === 200) {
          setCarregandoSalvar(false);
          setListaDisciplinas(data);

          if (data?.length === 1) {
            setInicial(estadoAnterior => ({
              ...estadoAnterior,
              disciplinaId: data[0].codigoComponenteCurricular.toString(),
            }));

            if (Object.keys(refForm).length > 0) {
              onChangeDisciplinas(data[0].codigoComponenteCurricular, data);
            }

            const { regencia } = data ? data[0] : false;
            setEhRegencia(regencia);
          }
        }
      } catch (error) {
        erro('Não foi possível obter os componentes curriculares.');
        setCarregandoSalvar(false);
      }
    }

    if (turmaId) {
      obterListaDisciplinas();
      validarConsultaModoEdicaoENovo();
    }
  }, [refForm]);

  useEffect(() => {
    if (ehReposicao) refForm.setFieldValue('recorrenciaAula', 1);
  }, [ehReposicao, refForm]);

  const montaValidacoes = useCallback(() => {
    const validacaoQuantidade = Yup.number()
      .typeError('O valor informado deve ser um número')
      .when('quantidadeRadio', (quantidadeRadio, schema) => {
        return quantidadeRadio <= 0
          ? schema.required('A quantidade de aulas é obrigatória')
          : schema.required(false);
      })
      .required('A quantidade de aulas é obrigatória')
      .positive('Valor inválido')
      .integer();

    const val = {
      tipoAula: Yup.string().required('Tipo obrigatório'),
      disciplinaId: Yup.string().required('Componente curricular obrigatório'),
      dataAulaCompleta: momentSchema.required('Data obrigatória'),
      recorrenciaAula: Yup.string().required('Recorrência obrigatória'),
      quantidadeTexto:
        idDisciplina || idDisciplina !== ''
          ? controlaQuantidadeAula
            ? validacaoQuantidade.lessThan(
                quantidadeMaximaAulas + 1,
                `Valor não pode ser maior que ${quantidadeMaximaAulas}`
              )
            : validacaoQuantidade
          : Yup.string().required(false),
    };

    if (disciplinaCompartilhada) {
      val.disciplinaCompartilhadaId = Yup.string().required(
        'Componente curricular compartilhado'
      );
    }

    if (!ehReposicao) {
      // TODO
      if (ehRecorrencia) {
        // TODO
      }
      // TODO
      if (controlaQuantidadeAula) {
        // TODO
      }
      if (ehRegencia) {
        if (turmaSelecionada.modalidade === modalidade.EJA) {
          setInicial(estadoAntigo => {
            return { ...estadoAntigo, quantidadeTexto: 5, quantidadeRadio: '' };
          });
          setEhEja(true);
        } else {
          setInicial(estadoAntigo => {
            return {
              ...estadoAntigo,
              quantidadeTexto: '',
              quantidadeRadio: 1,
            };
          });
          setEhEja(false);
        }
      }
    }

    setValidacoes(Yup.object(val));
  }, [
    controlaQuantidadeAula,
    disciplinaCompartilhada,
    ehRecorrencia,
    ehRegencia,
    ehReposicao,
    idDisciplina,
    quantidadeMaximaAulas,
    turmaSelecionada.modalidade,
  ]);

  useEffect(() => {
    if (quantidadeMaximaAulas && quantidadeMaximaAulas === 1) {
      refForm.setFieldValue('quantidadeTexto', '');
      refForm.setFieldValue('quantidadeRadio', quantidadeMaximaAulas);
      setDesabilitaPorQuantidade(true);
    }
  }, [quantidadeMaximaAulas, refForm]);

  useEffect(() => {
    montaValidacoes();
  }, [montaValidacoes]);

  const resetarTela = form => {
    form.resetForm();
    setControlaQuantidadeAula(true);
    setQuantidadeMaximaAulas(0);
    setModoEdicao(false);
    setEhAulaUnica(false);
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );

      if (confirmou) {
        resetarTela(form);
      }
    }
  };

  const salvar = async valoresForm => {
    const dados = { ...valoresForm };

    const data =
      dados.dataAulaCompleta && dados.dataAulaCompleta.format('YYYY-MM-DD');
    dados.dataAula = moment(`${data}T00:00:00-03:00`);

    if (dados.quantidadeRadio && dados.quantidadeRadio > 0) {
      dados.quantidade = dados.quantidadeRadio;
    } else if (dados.quantidadeTexto && dados.quantidadeTexto > 0) {
      dados.quantidade = dados.quantidadeTexto;
    }

    if (novoRegistro) {
      dados.tipoCalendarioId = match.params.tipoCalendarioId;
      dados.ueId = ueId;
      dados.turmaId = turmaId;
    }

    dados.dataAula = dados.dataAula.format();

    if (dados && dados.disciplinaId) {
      const componenteCurricular = listaDisciplinas.find(
        item => String(item.codigoComponenteCurricular) === dados.disciplinaId
      );
      if (componenteCurricular)
        dados.disciplinaNome = componenteCurricular.nome;
    }

    try {
      setCarregandoSalvar(true);
      const { data: dataRespAula, status, response } = await ServicoAula.salvar(
        idAula,
        dados
      );

      if (dataRespAula && status === 200) {
        history.push('/calendario-escolar/calendario-professor');
        sucesso(dataRespAula.mensagens[0]);
      } else if (response) {
        setCarregandoSalvar(false);
        erro(
          response.status === 601
            ? response.data.mensagens
            : 'Houve uma falha ao salvar a aula, por favor contate o suporte'
        );
      }
    } catch (error) {
      erro('Não foi possível salvar a aula.');
      setCarregandoSalvar(false);
    }
  };

  const onClickCadastrar = async valoresForm => {
    setCarregandoSalvar(true);
    const observacao = existeFrequenciaPlanoAula
      ? `Esta aula${
          ehAulaUnica ? '' : ', ou sua recorrencia'
        }, já possui frequência registrada, após a alteração você deverá acessar a aula e revisar a frequência`
      : '';
    if (
      quantidadeRecorrencia > 1 &&
      valoresForm.recorrenciaAula !== recorrencia.AULA_UNICA
    ) {
      const confirmado = await confirmar(
        'Atenção',
        observacao,
        `Você tem certeza que deseja alterar ${quantidadeRecorrencia} ocorrências desta aula a partir desta data?`,
        'Sim',
        'Não'
      );
      if (confirmado) {
        await salvar(valoresForm);
        history.push('/calendario-escolar/calendario-professor');
      }
    } else {
      if (existeFrequenciaPlanoAula) {
        const confirmado = await confirmar(
          'Atenção',
          observacao,
          'Você tem certeza que deseja alterar ?',
          'Sim',
          'Não'
        );

        if (!confirmado) return;
      }

      await salvar(valoresForm);
    }
    setCarregandoSalvar(false);
  };

  const onClickVoltar = async form => {
    if (dentroPeriodo && modoEdicao && !somenteLeitura) {
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?',
        'Sim',
        'Não'
      );

      if (confirmado) {
        validaAntesDoSubmit(form);
      } else {
        history.push('/calendario-escolar/calendario-professor');
      }
    } else {
      history.push('/calendario-escolar/calendario-professor');
    }
  };

  const excluir = async tipoRecorrencia => {
    const disciplina = listaDisciplinas.find(
      item => String(item.codigoComponenteCurricular) === String(idDisciplina)
    );

    const disciplinaBase64 = btoa(disciplina.nome);

    const exclusao = await api
      .delete(
        `v1/calendarios/professores/aulas/${idAula}/recorrencias/${tipoRecorrencia}/disciplinaNome/${disciplinaBase64}`
      )
      .catch(e => erros(e));
    if (exclusao) {
      if (tipoRecorrencia === recorrencia.AULA_UNICA) {
        sucesso('Aula excluída com sucesso.');
      } else if (exclusao.status === 200) sucesso(exclusao.data.mensagens[0]);
      history.push('/calendario-escolar/calendario-professor');
    }
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const observacao = existeFrequenciaPlanoAula
        ? 'Obs: Esta aula ou sua recorrência possui frequência ou plano de aula registrado, ao excluí-la estará excluindo esse registro também'
        : '';

      if (quantidadeRecorrencia > 1) {
        setVisualizarFormExcRecorrencia(true);
      } else {
        const confirmado = await confirmar(
          `Excluir aula  - ${dataAula.format('dddd')}, ${dataAula.format(
            'DD/MM/YYYY'
          )}`,
          `Você tem certeza que deseja excluir esta aula? ${observacao}`,
          'Deseja continuar?',
          'Excluir',
          'Cancelar'
        );
        if (confirmado) {
          excluir(recorrencia.AULA_UNICA);
        }
      }
    }
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(aula);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const getDataFormatada = useCallback(() => {
    if (refForm && Object.keys(refForm).length > 0) {
      const diaAulaContexto = refForm?.getFormikContext()?.values
        .dataAulaCompleta;
      const titulo = `${
        diaAulaContexto ? diaAulaContexto.format('dddd') : ''
      }, ${diaAulaContexto ? diaAulaContexto.format('DD/MM/YYYY') : ''} `;
      return titulo;
    }

    return undefined;
  }, [refForm]);

  return (
    <Loader loading={carregandoSalvar} tip="Carregando...">
      <div className="col-md-12">
        {quantidadeMaximaAulas <= 0 ? (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'cadastro-aula-quantidade-maxima',
              mensagem:
                'Não é possível criar aula normal porque o limite da grade curricular foi atingido',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        ) : null}
      </div>
      <div className="col-md-12">
        {!dentroPeriodo ? (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-perido-fechamento',
              mensagem:
                'Apenas é possível consultar este registro pois o período não está em aberto.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        ) : (
          ''
        )}
      </div>
      <Cabecalho pagina={`Cadastro de Aula - ${getDataFormatada()}`} />
      <Card>
        <ModalConteudoHtml
          key="reiniciarSenha"
          visivel={visualizarFormExcRecorrencia}
          onConfirmacaoPrincipal={() =>
            excluir(refFormRecorrencia.state.values.tipoRecorrenciaExclusao)
          }
          onConfirmacaoSecundaria={() => setVisualizarFormExcRecorrencia(false)}
          onClose={() => {}}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          titulo={`Excluir aula - ${getDataFormatada()}`}
          closable={false}
        >
          <Formik
            enableReinitialize
            initialValues={valoresIniciaisExclusao}
            validationSchema={validacoes}
            ref={refFormik => setRefFormRecorrencia(refFormik)}
            onSubmit={() => {}}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <div className="row justify-content-start">
                  <div
                    className="col-sm-12 col-md-12"
                    style={{ paddingTop: '10px' }}
                  >
                    <p>{`Essa aula se repete por ${quantidadeRecorrencia}${
                      quantidadeRecorrencia > 1 ? ' vezes' : ' vez'
                    } em seu planejamento.${
                      existeFrequenciaPlanoAula
                        ? ' Obs: Esta aula ou sua recorrência possui frequência ou plano de aula registrado, ao excluí-la estará excluindo esse registro também'
                        : ''
                    }`}</p>
                    <p>Qual opção de exclusão você deseja realizar?</p>
                  </div>
                  <div className="col-sm-12 col-md-12 d-block">
                    <RadioGroupButton
                      form={form}
                      id="tipo-recorrencia-exclusao"
                      label="Realizar exclusão"
                      opcoes={opcoesExcluirRecorrencia}
                      name="tipoRecorrenciaExclusao"
                      onChange={() => {}}
                    />
                  </div>
                </div>
              </Form>
            )}
          </Formik>
        </ModalConteudoHtml>
        <Formik
          enableReinitialize
          initialValues={inicial}
          validationSchema={validacoes}
          ref={refFormik => setRefForm(refFormik)}
          onSubmit={valores => onClickCadastrar(valores)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="row pb-3">
                <div className="col-md-4 pb-2 d-flex justify-content-start">
                  <CampoData
                    form={form}
                    placeholder="Data da aula"
                    formatoData="DD/MM/YYYY"
                    label=""
                    desabilitado={somenteLeitura || !dentroPeriodo}
                    name="dataAulaCompleta"
                    onChange={onChangeCampos}
                  />
                </div>
                <div className="col-md-8 pb-2 d-flex justify-content-end">
                  <Button
                    id={shortid.generate()}
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={() => onClickVoltar(form)}
                  />
                  <Button
                    id={shortid.generate()}
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    className="mr-2"
                    onClick={() => onClickCancelar(form)}
                    disabled={somenteLeitura || !dentroPeriodo || !modoEdicao}
                  />
                  <Button
                    id={shortid.generate()}
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    hidden={novoRegistro}
                    onClick={onClickExcluir}
                    disabled={somenteLeitura || !dentroPeriodo}
                  />

                  <Button
                    id={shortid.generate()}
                    label={novoRegistro ? 'Cadastrar' : 'Alterar'}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    disabled={
                      somenteLeitura ||
                      (novoRegistro && !permissaoTela.podeIncluir) ||
                      (!novoRegistro && !permissaoTela.podeAlterar) ||
                      !dentroPeriodo
                    }
                    onClick={() => validaAntesDoSubmit(form)}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col-sm-12 col-md-5 col-lg-3 col-xl-3 mb-2 mr-0 pr-0">
                  <RadioGroupButton
                    desabilitado={
                      somenteLeitura || !dentroPeriodo || !novoRegistro
                    }
                    id="tipo-aula"
                    label="Tipo de aula"
                    form={form}
                    opcoes={opcoesTipoAula}
                    name="tipoAula"
                    onChange={e => {
                      setDesabilitaPorGrade(e.target.value !== 2);
                      setDesabilitaPorQuantidade(e.target.value !== 2);
                      setEhReposicao(e.target.value === 2);
                      onChangeCampos();
                      setControlaQuantidadeAula(ehReposicao);
                    }}
                  />
                </div>
                <div className="col-sm-12 col-md-7 col-lg-9 col-xl-6 mb-2">
                  <SelectComponent
                    id="disciplinaId"
                    form={form}
                    name="disciplinaId"
                    lista={listaDisciplinas}
                    valueOption="codigoComponenteCurricular"
                    valueText="nome"
                    onChange={e => {
                      onChangeDisciplinas(e, form);
                      onChangeCampos();
                    }}
                    label="Componente curricular"
                    placeholder="Selecione um componente curricular"
                    disabled={
                      somenteLeitura ||
                      !dentroPeriodo ||
                      !!(
                        listaDisciplinas &&
                        listaDisciplinas.length &&
                        listaDisciplinas.length === 1
                      ) ||
                      !novoRegistro
                    }
                  />
                </div>
                {disciplinaCompartilhada && (
                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 pb-3">
                    <SelectComponent
                      id="disciplinaCompartilhadaId"
                      form={form}
                      disabled={somenteLeitura || !dentroPeriodo}
                      name="disciplinaCompartilhadaId"
                      lista={listaDisciplinasCompartilhadas}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      label="Componente curricular compartilhado"
                      placeholder="Selecione um componente curricular compartilhado"
                    />
                  </div>
                )}
                <div className="col-sm-12 col-md-8 col-lg-8 col-xl-5 mb-2 d-flex justify-content-start">
                  <RadioGroupButton
                    id="quantidadeRadio"
                    label="Quantidade de Aulas"
                    form={form}
                    desabilitado={
                      somenteLeitura ||
                      !dentroPeriodo ||
                      desabilitaPorGrade ||
                      desabilitaPorQuantidade
                    }
                    opcoes={opcoesQuantidadeAulas}
                    name="quantidadeRadio"
                    onChange={() => {
                      onChangeCampos();
                      refForm.setFieldValue('quantidadeTexto', '');
                    }}
                    className="text-nowrap"
                  />
                  <div className="mt-4 ml-0 mr-2 text-nowrap">
                    ou informe a quantidade
                  </div>
                  <CampoTexto
                    form={form}
                    name="quantidadeTexto"
                    className="mt-3"
                    style={{ width: '70px' }}
                    id="quantidadeTexto"
                    desabilitado={
                      somenteLeitura ||
                      !dentroPeriodo ||
                      !idDisciplina ||
                      (quantidadeMaximaAulas < 3 && controlaQuantidadeAula) ||
                      (ehRegencia && !ehReposicao) ||
                      desabilitaPorGrade ||
                      desabilitaPorQuantidade
                    }
                    onChange={() => {
                      refForm.setFieldValue('quantidadeRadio', 0);
                      onChangeCampos();
                    }}
                    icon
                  />
                </div>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-7 mb-2">
                  <RadioGroupButton
                    id="recorrencia"
                    label="Recorrência"
                    form={form}
                    opcoes={opcoesRecorrencia}
                    name="recorrenciaAula"
                    desabilitado={
                      somenteLeitura ||
                      !dentroPeriodo ||
                      ehReposicao ||
                      ehAulaUnica
                    }
                    onChange={e => {
                      onChangeCampos();
                      setEhRecorrencia(e.target.value !== 1);
                    }}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        {exibirAuditoria ? (
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRf}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRf}
          />
        ) : (
          ''
        )}
      </Card>
    </Loader>
  );
};

CadastroAula.propTypes = {
  match: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  location: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

CadastroAula.defaultProps = {
  match: {},
  location: {},
};

export default CadastroAula;
