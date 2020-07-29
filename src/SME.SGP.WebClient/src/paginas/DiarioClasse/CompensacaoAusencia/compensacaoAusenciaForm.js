import { Form, Formik } from 'formik';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import * as Yup from 'yup';
import { CampoTexto, Colors, Label, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Auditoria from '~/componentes/auditoria';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import Editor from '~/componentes/editor/editor';
import SelectComponent from '~/componentes/select';
import modalidade from '~/dtos/modalidade';
import RotasDto from '~/dtos/rotasDto';
import { confirmar, erro, erros, sucesso } from '~/servicos/alertas';
import { setBreadcrumbManual } from '~/servicos/breadcrumb-services';
import history from '~/servicos/history';
import ServicoPeriodoFechamento from '~/servicos/Paginas/Calendario/ServicoPeriodoFechamento';
import ServicoCompensacaoAusencia from '~/servicos/Paginas/DiarioClasse/ServicoCompensacaoAusencia';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import CopiarCompensacao from './copiarCompensacao';
import ListaAlunos from './listasAlunos/listaAlunos';
import ListaAlunosAusenciasCompensadas from './listasAlunos/listaAlunosAusenciasCompensadas';
import {
  Badge,
  BotaoListaAlunos,
  ColunaBotaoListaAlunos,
  ListaCopiarCompensacoes,
} from './styles';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const CompensacaoAusenciaForm = ({ match }) => {
  const usuario = useSelector(store => store.usuario);

  const permissoesTela = usuario.permissoes[RotasDto.COMPENSACAO_AUSENCIA];
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);

  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [refForm, setRefForm] = useState({});
  const [auditoria, setAuditoria] = useState([]);
  const [idsAlunos, setIdsAlunos] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [temRegencia, setTemRegencia] = useState(false);
  const [novoRegistro, setNovoRegistro] = useState(true);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [exibirAuditoria, setExibirAuditoria] = useState(false);
  const [carregandoDados, setCarregandoDados] = useState(false);
  const [alunosAusenciaTurma, setAlunosAusenciaTurma] = useState([]);
  const [carregouInformacoes, setCarregouInformacoes] = useState(false);
  const [idCompensacaoAusencia, setIdCompensacaoAusencia] = useState(0);
  const [carregandoDisciplinas, setCarregandoDisciplinas] = useState(false);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [bimestreSugeridoCopia, setBimestreSugeridoCopia] = useState(null);
  const [selecaoAlunoSelecionado, setSelecaoAlunoSelecionado] = useState('');
  const [listaDisciplinasRegencia, setListaDisciplinasRegencia] = useState([]);
  const [alunosAusenciaCompensada, setAlunosAusenciaCompensada] = useState([]);
  const [exibirCopiarCompensacao, setExibirCopiarCompensacao] = useState(false);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [foraDoPeriodo, setForaDoPeriodo] = useState(false);
  const [
    turmaSelecionadaFiltroPrincipal,
    setTurmaSelecionadaFiltroPrincipal,
  ] = useState(undefined);
  const [
    idsAlunosAusenciaCompensadas,
    setIdsAlunosAusenciaCompensadas,
  ] = useState([]);
  const [
    alunosAusenciaTurmaOriginal,
    setAlunosAusenciaTurmaOriginal,
  ] = useState([]);
  const [
    carregandoListaAlunosFrequencia,
    setCarregandoListaAlunosFrequencia,
  ] = useState(false);

  const [compensacoesParaCopiar, setCompensacoesParaCopiar] = useState({
    compensacaoOrigemId: 0,
    turmasIds: [],
    bimestre: 0,
  });

  const [valoresIniciais, setValoresIniciais] = useState({
    disciplinaId: undefined,
    bimestre: '',
    atividade: '',
    descricao: '',
  });

  const [validacoes] = useState(
    Yup.object({
      descricao: Yup.string().required('Disciplina obrigatória'),
      disciplinaId: Yup.string().required('Disciplina obrigatória'),
      bimestre: Yup.string().required('Bimestre obrigatório'),
      atividade: Yup.string()
        .required('Atividade obrigatória')
        .max(250, 'Máximo 250 caracteres'),
    })
  );

  let listaBi = [];
  if (turmaSelecionada.modalidade === modalidade.EJA) {
    listaBi = [
      { valor: 1, descricao: '1°' },
      { valor: 2, descricao: '2°' },
    ];
  } else {
    listaBi = [
      { valor: 1, descricao: '1°' },
      { valor: 2, descricao: '2°' },
      { valor: 3, descricao: '3°' },
      { valor: 4, descricao: '4°' },
    ];
  }

  const [listaBimestres] = useState(listaBi);

  const ForaPerido = () => {
    return (
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
    );
  };

  const podeAlterarNoPeriodo = useCallback(
    async bimestre => {
      const podeAlterar = await ServicoPeriodoFechamento.verificarSePodeAlterarNoPeriodo(
        turmaSelecionada.turma,
        bimestre
      ).catch(e => {
        erros(e);
      });
      if (podeAlterar && podeAlterar.data) {
        setDesabilitarCampos(false);
        return true;
      }
      setDesabilitarCampos(true);
      return false;
    },
    [turmaSelecionada.turma]
  );

  useEffect(() => {
    if (!novoRegistro) {
      setBreadcrumbManual(
        match.url,
        'Alterar Compensação de Ausência',
        '/diario-classe/compensacao-ausencia'
      );
    }
  }, [match.url, novoRegistro]);

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoesTela, naoSetarSomenteConsultaNoStore)
    );
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  useEffect(() => {
    const desabilitar = novoRegistro
      ? somenteConsulta || !permissoesTela.podeIncluir
      : somenteConsulta || !permissoesTela.podeAlterar;
    setDesabilitarCampos(desabilitar);
  }, [somenteConsulta, novoRegistro, permissoesTela]);

  const removerAlunosDuplicadosEdicao = (alunosTurma, alunosEdicao) => {
    const novaLista = alunosTurma.filter(
      aluno => !alunosEdicao.find(al => al.id == aluno.id)
    );
    return novaLista;
  };

  const obterAlunosComAusencia = useCallback(
    async (disciplinaId, bimestre, listaAlunosEdicao) => {
      setCarregandoListaAlunosFrequencia(true);
      const alunos = await ServicoCompensacaoAusencia.obterAlunosComAusencia(
        turmaSelecionada.turma,
        disciplinaId,
        bimestre
      ).catch(e => {
        setCarregandoListaAlunosFrequencia(false);
        setAlunosAusenciaTurma([]);
        setAlunosAusenciaTurmaOriginal([]);
        erros(e);
      });
      if (alunos && alunos.data && alunos.data.length) {
        if (listaAlunosEdicao && listaAlunosEdicao.length) {
          const listaSemDuplicados = removerAlunosDuplicadosEdicao(
            alunos.data,
            listaAlunosEdicao
          );
          setAlunosAusenciaTurma([...listaSemDuplicados]);
          setAlunosAusenciaTurmaOriginal([...listaSemDuplicados]);
        } else {
          setAlunosAusenciaTurma([...alunos.data]);
          setAlunosAusenciaTurmaOriginal([...alunos.data]);
        }
      } else {
        setAlunosAusenciaTurma([]);
        setAlunosAusenciaTurmaOriginal([]);
      }
      setCarregandoListaAlunosFrequencia(false);
    },
    [turmaSelecionada.turma]
  );

  // Usando somente quando o registro é edição, vem com disciploinas regencia para marcar o label!
  const selecionarDisciplinas = useCallback(
    (disciplinasRegenciaEdicao, disciplinasRegencia) => {
      const disciplinas = [...disciplinasRegencia];
      disciplinasRegenciaEdicao.forEach(item => {
        disciplinas.forEach((disci, indice) => {
          if (item.codigo == disci.codigoComponenteCurricular) {
            disciplinas[indice].selecionada = !disciplinas[indice].selecionada;
            disciplinas[indice].codigo =
              disciplinas[indice].codigoComponenteCurricular;
          }
        });
      });
      setListaDisciplinasRegencia(disciplinas);
    },
    []
  );

  const obterDisciplinasRegencia = useCallback(
    async (
      codigoDisciplinaSelecionada,
      disciplinasLista,
      disciplinasRegenciaEdicao
    ) => {
      const disciplina = disciplinasLista.find(
        c => c.codigoComponenteCurricular == codigoDisciplinaSelecionada
      );
      if (disciplina && disciplina.regencia) {
        const disciplinasRegencia = await ServicoDisciplina.obterDisciplinasPlanejamento(
          codigoDisciplinaSelecionada,
          turmaSelecionada.turma,
          false,
          disciplina.regencia
        ).catch(e => erros(e));

        if (
          disciplinasRegencia &&
          disciplinasRegencia.data &&
          disciplinasRegencia.data.length
        ) {
          if (disciplinasRegenciaEdicao && disciplinasRegenciaEdicao.length) {
            selecionarDisciplinas(
              disciplinasRegenciaEdicao,
              disciplinasRegencia.data
            );
          } else {
            setListaDisciplinasRegencia(disciplinasRegencia.data);
          }
          setTemRegencia(true);
        }
      } else {
        setListaDisciplinasRegencia([]);
        setTemRegencia(false);
      }
    },
    [turmaSelecionada.turma, selecionarDisciplinas]
  );

  const consultaPorId = useCallback(
    async disciplinas => {
      if (idCompensacaoAusencia && disciplinas.length) {
        setCarregandoDados(true);
        const resultado = await ServicoCompensacaoAusencia.obterPorId(
          idCompensacaoAusencia
        ).catch(e => {
          erros(e);
          setCarregandoDados(false);
        });

        if (resultado && resultado.data) {
          obterDisciplinasRegencia(
            resultado.data.disciplinaId,
            disciplinas,
            resultado.data.disciplinasRegencia
          );

          const dentroPeriodo = await podeAlterarNoPeriodo(
            String(resultado.data.bimestre)
          );
          setForaDoPeriodo(!dentroPeriodo);

          const disciplinasRegencia =
            resultado.data.disciplinasRegencia &&
            resultado.data.disciplinasRegencia.length
              ? resultado.data.disciplinasRegencia
              : [];

          setValoresIniciais({
            disciplinaId: String(resultado.data.disciplinaId),
            bimestre: String(resultado.data.bimestre),
            atividade: resultado.data.atividade,
            descricao: resultado.data.descricao,
            disciplinasRegencia,
          });

          // Usado no Modal de copiar compensação!
          setBimestreSugeridoCopia(String(resultado.data.bimestre));
          if (resultado.data.alunos && resultado.data.alunos.length) {
            setAlunosAusenciaCompensada(resultado.data.alunos);
          }

          if (dentroPeriodo) {
            obterAlunosComAusencia(
              resultado.data.disciplinaId,
              resultado.data.bimestre,
              resultado.data.alunos
            );
          }

          setAuditoria({
            criadoPor: resultado.data.criadoPor,
            criadoRf: resultado.data.criadoRf,
            criadoEm: resultado.data.criadoEm,
            alteradoPor: resultado.data.alteradoPor,
            alteradoRf: resultado.data.alteradoRf,
            alteradoEm: resultado.data.alteradoEm,
          });
          setExibirAuditoria(true);
          setCarregouInformacoes(true);
          setCarregandoDados(false);
        } else {
          setCarregandoDados(false);
        }
        setNovoRegistro(false);
      }
    },
    [
      idCompensacaoAusencia,
      obterAlunosComAusencia,
      podeAlterarNoPeriodo,
      obterDisciplinasRegencia,
    ]
  );

  useEffect(() => {
    const temId = match && match.params && match.params.id;

    if (turmaSelecionada.turma && temId) {
      setIdCompensacaoAusencia(match.params.id);
    }
  }, [match, turmaSelecionada.turma]);

  useEffect(() => {
    if (disciplinaSelecionada && listaDisciplinas && listaDisciplinas.length) {
      obterDisciplinasRegencia(String(disciplinaSelecionada), listaDisciplinas);
    }
  }, [disciplinaSelecionada, listaDisciplinas, obterDisciplinasRegencia]);

  const resetarForm = useCallback(() => {
    setListaDisciplinas([]);
    setDisciplinaSelecionada(undefined);
    if (refForm && refForm.resetForm) {
      refForm.resetForm();
    }
    setListaDisciplinasRegencia([]);
    setTemRegencia(false);
  }, [refForm]);

  const onChangeCampos = () => {
    if (
      !foraDoPeriodo &&
      !desabilitarCampos &&
      carregouInformacoes &&
      !modoEdicao
    ) {
      setModoEdicao(true);
    }
  };

  const selecionarDisciplina = indice => {
    const disciplinas = [...listaDisciplinasRegencia];
    disciplinas[indice].selecionada = !disciplinas[indice].selecionada;
    disciplinas[indice].codigo = disciplinas[indice].codigoComponenteCurricular;
    setListaDisciplinasRegencia(disciplinas);
    onChangeCampos();
  };

  const onChangeDisciplina = (codigoDisciplina, form) => {
    setDisciplinaSelecionada(codigoDisciplina);
    setAlunosAusenciaTurma([]);
    setAlunosAusenciaTurmaOriginal([]);
    setAlunosAusenciaCompensada([]);
    setIdsAlunosAusenciaCompensadas([]);
    setIdsAlunos([]);
    obterDisciplinasRegencia(codigoDisciplina, listaDisciplinas);
    onChangeCampos();
    form.setFieldValue('bimestre', '');
  };

  const obterDisciplinas = useCallback(async () => {
    setCarregandoDisciplinas(true);
    const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaSelecionada.turma
    );
    if (disciplinas.data && disciplinas.data.length) {
      setListaDisciplinas(disciplinas.data);
    } else {
      setListaDisciplinas([]);
    }

    const valoresIniciaisForm = {
      disciplinaId: '',
      bimestre: '',
      atividade: '',
      descricao: '',
    };

    const registroNovo = !(match && match.params && match.params.id);

    if (disciplinas.data && disciplinas.data.length === 1) {
      setDesabilitarDisciplina(true);

      const disciplina = disciplinas.data[0];

      if (registroNovo) {
        valoresIniciaisForm.disciplinaId = String(
          disciplina.codigoComponenteCurricular
        );

        setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
      }
      setCarregandoDisciplinas(false);
    } else {
      setDesabilitarDisciplina(false);
    }

    if (registroNovo) {
      setCarregouInformacoes(true);
      setValoresIniciais(valoresIniciaisForm);
    } else {
      consultaPorId(
        disciplinas && disciplinas.data && disciplinas.data.length
          ? disciplinas.data
          : []
      );
    }
    setCarregandoDisciplinas(false);
  }, [match, turmaSelecionada.turma, consultaPorId]);

  useEffect(() => {
    if (!turmaSelecionada.turma) {
      history.push('/diario-classe/compensacao-ausencia');
    }

    if (
      turmaSelecionadaFiltroPrincipal &&
      turmaSelecionadaFiltroPrincipal != turmaSelecionada.turma
    ) {
      // Somente quando troca a turma para outra turma no filtro principal!
      resetarForm();
    }
    setTurmaSelecionadaFiltroPrincipal(turmaSelecionada.turma);
    if (
      turmaSelecionadaFiltroPrincipal &&
      turmaSelecionada.turma &&
      turmaSelecionadaFiltroPrincipal == turmaSelecionada.turma &&
      listaDisciplinas.length < 1 &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterDisciplinas(turmaSelecionada.turma);
    }
  }, [
    obterDisciplinas,
    resetarForm,
    turmaSelecionadaFiltroPrincipal,
    listaDisciplinas,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
  ]);

  const limparListas = () => {
    setAlunosAusenciaCompensada([]);
    setIdsAlunosAusenciaCompensadas([]);
    setIdsAlunos([]);
    setAlunosAusenciaTurma([]);
    setAlunosAusenciaTurmaOriginal([]);
  };

  const onChangeBimestre = async (bimestre, form) => {
    limparListas();
    const dentroPeriodo = await podeAlterarNoPeriodo(String(bimestre));
    setForaDoPeriodo(!dentroPeriodo);

    if (dentroPeriodo) {
      let podeEditar = false;
      const exucutandoCalculoFrequencia = await ServicoCompensacaoAusencia.obterStatusCalculoFrequencia(
        turmaSelecionada.turma,
        form.values.disciplinaId,
        bimestre
      ).catch(e => {
        erros(e);
      });
      if (
        exucutandoCalculoFrequencia &&
        exucutandoCalculoFrequencia.status === 200
      ) {
        const temProcessoEmExecucao =
          exucutandoCalculoFrequencia && exucutandoCalculoFrequencia.data;

        if (temProcessoEmExecucao) {
          podeEditar = false;
        } else {
          podeEditar = true;
        }

        if (podeEditar) {
          setAlunosAusenciaCompensada([]);
          setIdsAlunosAusenciaCompensadas([]);
          setIdsAlunos([]);
          if (bimestre && form && form.values.disciplinaId) {
            obterAlunosComAusencia(form.values.disciplinaId, bimestre);
          } else {
            setAlunosAusenciaTurma([]);
            setAlunosAusenciaTurmaOriginal([]);
          }
          onChangeCampos();
        } else {
          erro(
            'No momento não é possível realizar a edição pois tem cálculo(s) em processo, tente mais tarde!'
          );
        }
      }
    }
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length == 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const onClickExcluir = async () => {
    if (!novoRegistro) {
      const confirmado = await confirmar(
        'Excluir compensação',
        '',
        'Você tem certeza que deseja excluir este registro',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const excluir = await ServicoCompensacaoAusencia.deletar([
          idCompensacaoAusencia,
        ]).catch(e => erros(e));

        if (excluir && excluir.status == 200) {
          sucesso('Compensação excluída com sucesso.');
          history.push('/diario-classe/compensacao-ausencia');
        }
      }
    }
  };

  const resetarTelaEdicaoComId = async form => {
    const dadosEdicao = await ServicoCompensacaoAusencia.obterPorId(
      match.params.id
    ).catch(e => {
      erros(e);
    });
    if (dadosEdicao && dadosEdicao.status == 200) {
      setIdsAlunos([]);
      setIdsAlunosAusenciaCompensadas([]);
      if (dadosEdicao.data.alunos && dadosEdicao.data.alunos.length) {
        setAlunosAusenciaCompensada(dadosEdicao.data.alunos);
      } else {
        setAlunosAusenciaCompensada([]);
      }

      const dentroPeriodo = await podeAlterarNoPeriodo(
        String(form.values.bimestre)
      );
      setForaDoPeriodo(!dentroPeriodo);

      if (dentroPeriodo) {
        obterAlunosComAusencia(
          form.values.disciplinaId,
          form.values.bimestre,
          dadosEdicao.data.alunos
        );
      }
      obterDisciplinasRegencia(
        form.values.disciplinaId,
        listaDisciplinas,
        dadosEdicao.data.disciplinasRegencia
      );
      form.resetForm();
      setModoEdicao(false);
    }
  };

  const onClickCancelar = async form => {
    if (modoEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        if (match && match.params && match.params.id) {
          resetarTelaEdicaoComId(form);
        } else {
          setIdsAlunos([]);
          setAlunosAusenciaTurma([]);
          setAlunosAusenciaTurmaOriginal([]);
          setIdsAlunosAusenciaCompensadas([]);
          setAlunosAusenciaCompensada([]);
          form.resetForm();
          setModoEdicao(false);
        }
      }
    }
  };

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async form => {
    if (modoEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        validaAntesDoSubmit(form);
      } else {
        history.push('/diario-classe/compensacao-ausencia');
      }
    } else {
      history.push('/diario-classe/compensacao-ausencia');
    }
  };

  const onClickCadastrar = async valoresForm => {
    const paramas = valoresForm;
    paramas.id = idCompensacaoAusencia;
    paramas.turmaId = turmaSelecionada.turma;

    paramas.disciplinasRegenciaIds = [];
    if (temRegencia) {
      const somenteSelecionados = listaDisciplinasRegencia.filter(
        item => item.selecionada
      );
      paramas.disciplinasRegenciaIds = somenteSelecionados.map(item =>
        String(item.codigoComponenteCurricular)
      );
    }
    paramas.alunos = alunosAusenciaCompensada.map(item => {
      return {
        id: item.id,
        qtdFaltasCompensadas: item.quantidadeFaltasCompensadas,
      };
    });

    const cadastrado = await ServicoCompensacaoAusencia.salvar(
      paramas.id,
      paramas
    ).catch(e => erros(e));

    if (cadastrado && cadastrado.status == 200) {
      if (
        compensacoesParaCopiar &&
        compensacoesParaCopiar.compensacaoOrigemId &&
        compensacoesParaCopiar.dadosTurmas &&
        compensacoesParaCopiar.dadosTurmas.length
      ) {
        await ServicoCompensacaoAusencia.copiarCompensacao(
          compensacoesParaCopiar
        )
          .then(resposta => {
            if (resposta.status === 200) {
              sucesso(resposta.data);
            }
          })
          .catch(e => erros(e));
      }
      if (idCompensacaoAusencia) {
        sucesso('Compensação alterada com sucesso.');
      } else {
        sucesso('Compensação criada com sucesso.');
      }
      history.push('/diario-classe/compensacao-ausencia');
    }
  };

  const obterListaAlunosComIdsSelecionados = (list, ids) => {
    return list.filter(item => ids.find(id => id == item.id));
  };

  const obterListaAlunosSemIdsSelecionados = (list, ids) => {
    return list.filter(item => !ids.find(id => id == item.id));
  };

  const onClickAdicionarAlunos = () => {
    if (!desabilitarCampos && idsAlunos && idsAlunos.length) {
      const novaListaAlunosAusenciaCompensada = obterListaAlunosComIdsSelecionados(
        alunosAusenciaTurma,
        idsAlunos
      );

      const novaListaAlunos = obterListaAlunosSemIdsSelecionados(
        alunosAusenciaTurma,
        idsAlunos
      );

      const novaListaAlunosOriginal = obterListaAlunosSemIdsSelecionados(
        alunosAusenciaTurmaOriginal,
        idsAlunos
      );

      onChangeCampos();
      setAlunosAusenciaTurmaOriginal([...novaListaAlunosOriginal]);
      setAlunosAusenciaTurma([...novaListaAlunos]);
      setAlunosAusenciaCompensada([
        ...novaListaAlunosAusenciaCompensada,
        ...alunosAusenciaCompensada,
      ]);
      setIdsAlunos([]);
    }
  };

  const onClickRemoverAlunos = async () => {
    if (
      !desabilitarCampos &&
      idsAlunosAusenciaCompensadas &&
      idsAlunosAusenciaCompensadas.length
    ) {
      const listaAlunosRemover = alunosAusenciaCompensada.filter(item =>
        idsAlunosAusenciaCompensadas.find(id => id == item.id)
      );
      const confirmado = await confirmar(
        'Excluir aluno',
        listaAlunosRemover.map(item => {
          return `${item.id} - ${item.nome}`;
        }),
        'A frequência do(s) seguinte(s) aluno(s) será recalculada somente quando salvar as suas alterações',
        'Excluir',
        'Cancelar',
        true
      );

      if (confirmado) {
        const novaListaAlunosOriginal = obterListaAlunosComIdsSelecionados(
          alunosAusenciaTurmaOriginal,
          idsAlunosAusenciaCompensadas
        );

        const novaListaAlunos = obterListaAlunosComIdsSelecionados(
          alunosAusenciaCompensada,
          idsAlunosAusenciaCompensadas
        );

        const novaListaAlunosAusenciaCompensada = obterListaAlunosSemIdsSelecionados(
          alunosAusenciaCompensada,
          idsAlunosAusenciaCompensadas
        );

        onChangeCampos();
        setAlunosAusenciaTurmaOriginal([...novaListaAlunosOriginal]);
        setAlunosAusenciaTurma([...novaListaAlunos, ...alunosAusenciaTurma]);
        setAlunosAusenciaCompensada([...novaListaAlunosAusenciaCompensada]);
        setIdsAlunosAusenciaCompensadas([]);
      }
    }
  };

  const onSelectRowAlunos = ids => {
    if (!desabilitarCampos) {
      setIdsAlunos(ids);
    }
  };

  const onSelectRowAlunosAusenciaCompensada = ids => {
    if (!desabilitarCampos) {
      setIdsAlunosAusenciaCompensadas(ids);
    }
  };

  const atualizarValoresListaCompensacao = novaListaAlunos => {
    if (!desabilitarCampos) {
      onChangeCampos();
      setAlunosAusenciaCompensada([...novaListaAlunos]);
    }
  };

  const onChangeSelecaoAluno = e => {
    const valor = e.target.value;

    const listaParaPesquisar =
      alunosAusenciaTurmaOriginal && alunosAusenciaTurmaOriginal.length
        ? alunosAusenciaTurmaOriginal
        : alunosAusenciaTurma;

    if (!selecaoAlunoSelecionado) {
      setAlunosAusenciaTurmaOriginal(alunosAusenciaTurma);
    }

    if (!valor) {
      setAlunosAusenciaTurma([...alunosAusenciaTurmaOriginal]);
    }

    if (valor) {
      const listaNova = listaParaPesquisar.filter(aluno => {
        return aluno.nome
          .toString()
          .toLowerCase()
          .includes(valor.toLowerCase());
      });
      setAlunosAusenciaTurma(listaNova);
    }

    setSelecaoAlunoSelecionado(valor);
    setIdsAlunos([]);
  };

  const abrirCopiarCompensacao = () => {
    setExibirCopiarCompensacao(true);
  };

  const fecharCopiarCompensacao = () => {
    setExibirCopiarCompensacao(false);
  };

  const onCopiarCompensacoes = (valores, dadosTurmas) => {
    const valoresCopia = {
      compensacaoOrigemId: idCompensacaoAusencia,
      turmasIds: valores.turmas,
      bimestre: valores.bimestre,
      dadosTurmas,
    };
    setCompensacoesParaCopiar(valoresCopia);
  };

  const montarExibicaoCompensacoesCopiar = () => {
    return compensacoesParaCopiar.dadosTurmas.map(turma => {
      return (
        <div className="font-weight-bold" key={`turma-${shortid.generate()}`}>
          - {turma.nome}
        </div>
      );
    });
  };

  return (
    <>
      {exibirCopiarCompensacao ? (
        <CopiarCompensacao
          visivel={exibirCopiarCompensacao}
          turmaId={turmaSelecionada.turma}
          listaBimestres={listaBimestres}
          onCloseCopiarCompensacao={fecharCopiarCompensacao}
          onCopiarCompensacoes={onCopiarCompensacoes}
          compensacoesParaCopiar={compensacoesParaCopiar}
          bimestreSugerido={bimestreSugeridoCopia}
        />
      ) : (
        ''
      )}
      {foraDoPeriodo &&
      turmaSelecionada &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <ForaPerido />
      ) : (
        ''
      )}
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Cadastrar Compensação de Ausência" />
      <Card>
        <Loader
          loading={carregandoDados || carregandoListaAlunosFrequencia}
          tip=""
        >
          <Formik
            enableReinitialize
            ref={refF => setRefForm(refF)}
            initialValues={valoresIniciais}
            validationSchema={validacoes}
            onSubmit={onClickCadastrar}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form className="col-md-12 mb-4">
                <div className="d-flex justify-content-end pb-4">
                  <Button
                    id="btn-voltar"
                    label="Voltar"
                    icon="arrow-left"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={() => onClickVoltar(form)}
                  />
                  <Button
                    id="btn-cancelar"
                    label="Cancelar"
                    color={Colors.Roxo}
                    border
                    className="mr-2"
                    onClick={() => onClickCancelar(form)}
                    disabled={!modoEdicao}
                  />
                  <Button
                    id="btn-excluir"
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    disabled={
                      somenteConsulta ||
                      !permissoesTela.podeExcluir ||
                      novoRegistro ||
                      foraDoPeriodo
                    }
                    onClick={onClickExcluir}
                  />
                  <Button
                    id="btn-salvar"
                    label={`${
                      idCompensacaoAusencia > 0 ? 'Alterar' : 'Cadastrar'
                    }`}
                    color={Colors.Roxo}
                    border
                    bold
                    className="mr-2"
                    onClick={() => validaAntesDoSubmit(form)}
                    disabled={
                      ehTurmaInfantil(
                        modalidadesFiltroPrincipal,
                        turmaSelecionada
                      ) ||
                      desabilitarCampos ||
                      !modoEdicao
                    }
                  />
                </div>

                <div className="row">
                  <div className="col-sm-12 col-md-8 col-lg-4 col-xl-4 mb-2">
                    <Loader loading={carregandoDisciplinas} tip="">
                      <SelectComponent
                        form={form}
                        id="disciplina"
                        label="Disciplina"
                        name="disciplinaId"
                        lista={listaDisciplinas}
                        valueOption="codigoComponenteCurricular"
                        valueText="nome"
                        onChange={valor => onChangeDisciplina(valor, form)}
                        placeholder="Disciplina"
                        disabled={
                          desabilitarCampos ||
                          desabilitarDisciplina ||
                          !novoRegistro
                        }
                        allowClear={false}
                      />
                    </Loader>
                  </div>
                  <div className="col-sm-12 col-md-4 col-lg-2 col-xl-2 mb-2">
                    <SelectComponent
                      form={form}
                      id="bimestre"
                      label="Bimestre"
                      name="bimestre"
                      lista={listaBimestres}
                      valueOption="valor"
                      valueText="descricao"
                      onChange={bi => onChangeBimestre(bi, form)}
                      placeholder="Bimestre"
                      disabled={!novoRegistro}
                      allowClear={false}
                    />
                  </div>
                  <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-2">
                    <CampoTexto
                      form={form}
                      label="Atividade"
                      placeholder="Atividade"
                      name="atividade"
                      onChange={onChangeCampos}
                      type="input"
                      maxLength="250"
                      desabilitado={desabilitarCampos}
                    />
                  </div>
                  {temRegencia && listaDisciplinasRegencia && (
                    <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
                      <Label text="Componente curricular" />
                      {listaDisciplinasRegencia.map((disciplina, indice) => {
                        return (
                          <Badge
                            key={disciplina.codigoComponenteCurricular}
                            role="button"
                            onClick={e => {
                              e.preventDefault();
                              if (!desabilitarCampos) {
                                selecionarDisciplina(indice);
                              }
                            }}
                            aria-pressed={disciplina.selecionada && true}
                            alt={disciplina.nome}
                            className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mr-2"
                          >
                            {disciplina.nome}
                          </Badge>
                        );
                      })}
                    </div>
                  )}

                  <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                    <Editor
                      form={form}
                      name="descricao"
                      onChange={onChangeCampos}
                      label="Detalhamento da atividade"
                      desabilitar={desabilitarCampos}
                    />
                  </div>
                </div>
                <div className="row">
                  <div className="col-sm-5 col-md-5 col-lg-5 col-xl-5 mb-2">
                    <CampoTexto
                      label="Seleção dos alunos"
                      placeholder="Digite o nome do aluno"
                      onChange={onChangeSelecaoAluno}
                      value={selecaoAlunoSelecionado}
                      type="input"
                      icon
                    />
                  </div>
                </div>
                <div className="mt-2" style={{ flexGrow: 1, display: 'flex' }}>
                  <div>
                    <ListaAlunos
                      lista={alunosAusenciaTurma}
                      onSelectRow={onSelectRowAlunos}
                      idsAlunos={idsAlunos}
                    />
                  </div>
                  <ColunaBotaoListaAlunos style={{ margin: '15px' }}>
                    <BotaoListaAlunos
                      className="mb-2"
                      onClick={onClickAdicionarAlunos}
                    >
                      <i className="fas fa-chevron-right" />
                    </BotaoListaAlunos>
                    <BotaoListaAlunos onClick={onClickRemoverAlunos}>
                      <i className="fas fa-chevron-left" />
                    </BotaoListaAlunos>
                  </ColunaBotaoListaAlunos>
                  <div>
                    <ListaAlunosAusenciasCompensadas
                      listaAusenciaCompensada={alunosAusenciaCompensada}
                      onSelectRow={onSelectRowAlunosAusenciaCompensada}
                      idsAlunosAusenciaCompensadas={
                        idsAlunosAusenciaCompensadas
                      }
                      atualizarValoresListaCompensacao={
                        atualizarValoresListaCompensacao
                      }
                      desabilitarCampos={desabilitarCampos}
                    />
                  </div>
                </div>
                {exibirAuditoria ? (
                  <Auditoria
                    criadoEm={auditoria.criadoEm}
                    criadoPor={auditoria.criadoPor}
                    alteradoPor={auditoria.alteradoPor}
                    alteradoEm={auditoria.alteradoEm}
                  />
                ) : (
                  ''
                )}
                <div className="row mt-3">
                  <div className="col-md-12">
                    <Button
                      label="Copiar Compensação"
                      icon="share-square"
                      color={Colors.Azul}
                      className="mr-3"
                      border
                      onClick={abrirCopiarCompensacao}
                      disabled={novoRegistro || desabilitarCampos}
                    />
                    {compensacoesParaCopiar &&
                    compensacoesParaCopiar.compensacaoOrigemId &&
                    compensacoesParaCopiar.dadosTurmas.length ? (
                      <ListaCopiarCompensacoes>
                        <div className="mb-1">
                          Compensação será copiada para:
                        </div>
                        <div
                          className="font-weight-bold"
                          key={`bimestre-${shortid.generate()}`}
                        >
                          - Bimestre {compensacoesParaCopiar.bimestre}
                        </div>
                        {montarExibicaoCompensacoesCopiar()}
                      </ListaCopiarCompensacoes>
                    ) : (
                      ''
                    )}
                  </div>
                </div>
              </Form>
            )}
          </Formik>
        </Loader>
      </Card>
    </>
  );
};

export default CompensacaoAusenciaForm;
