import { Tabs } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Loader, ModalConteudoHtml, Colors } from '~/componentes';
import Button from '~/componentes/button';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import Editor from '~/componentes/editor/editor';
import Row from '~/componentes/row';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import notasConceitos from '~/dtos/notasConceitos';
import RotasDto from '~/dtos/rotasDto';
import {
  setExpandirLinha,
  setModoEdicaoGeral,
  setModoEdicaoGeralNotaFinal,
} from '~/redux/modulos/notasConceitos/actions';
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoNotas from '~/servicos/ServicoNotas';
import BotoesAcoessNotasConceitos from './botoesAcoes';
import { Container, ContainerAuditoria } from './notas.css';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import ServicoPeriodoFechamento from '~/servicos/Paginas/Calendario/ServicoPeriodoFechamento';
import moment from 'moment';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const { TabPane } = Tabs;

const Notas = ({ match }) => {
  const usuario = useSelector(store => store.usuario);
  const dispatch = useDispatch();
  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );
  const modoEdicaoGeralNotaFinal = useSelector(
    store => store.notasConceitos.modoEdicaoGeralNotaFinal
  );

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const { ehProfessorCj } = usuario;

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
  const [percentualMinimoAprovados, setPercentualMinimoAprovados] = useState(0);
  const [exibeModalJustificativa, setExibeModalJustificativa] = useState(false);
  const [valoresIniciais] = useState({ descricao: undefined });
  const [refForm, setRefForm] = useState({});

  const [validacoes] = useState(
    Yup.object({
      descricao: Yup.string().required('Justificativa obrigatória'),
    })
  );

  // Usado somente no Modal de Justificativa!
  const [proximoBimestre, setProximoBimestre] = useState(bimestreCorrente);
  const [clicouNoBotaoSalvar, setClicouNoBotaoSalvar] = useState(false);
  const [clicouNoBotaoVoltar, setClicouNoBotaoVoltar] = useState(false);

  const [podeLancaNota, setPodeLancaNota] = useState(true);

  const [showMsgPeriodoFechamento, setShowMsgPeriodoFechamento] = useState(
    false
  );

  const validaSeDesabilitaCampos = useCallback(
    async bimestre => {
      const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
        modalidadesFiltroPrincipal,
        usuario.turmaSelecionada
      );
      const somenteConsulta = verificaSomenteConsulta(
        permissoesTela,
        naoSetarSomenteConsultaNoStore
      );
      const desabilitar =
        somenteConsulta ||
        !permissoesTela.podeAlterar ||
        !permissoesTela.podeIncluir;

      let dentroDoPeriodo = true;
      if (!desabilitar && bimestre && usuario.turmaSelecionada.turma) {
        const retorno = await ServicoPeriodoFechamento.verificarSePodeAlterarNoPeriodo(
          usuario.turmaSelecionada.turma,
          bimestre
        ).catch(e => {
          erros(e);
        });
        if (retorno && retorno.status == 200) {
          dentroDoPeriodo = retorno.data;
        }
      }

      if (desabilitar) {
        setDesabilitarCampos(desabilitar);
        setShowMsgPeriodoFechamento(false);
        return;
      }

      if (!dentroDoPeriodo) {
        setDesabilitarCampos(true);
        setShowMsgPeriodoFechamento(true);
      } else {
        setDesabilitarCampos(desabilitar);
        setShowMsgPeriodoFechamento(false);
      }
    },
    [usuario.turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]
  );

  useEffect(() => {
    validaSeDesabilitaCampos(bimestreCorrente);
  }, [bimestreCorrente, validaSeDesabilitaCampos]);

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
    resetarBimestres();
    dispatch(setModoEdicaoGeral(false));
    dispatch(setModoEdicaoGeralNotaFinal(false));
    dispatch(setExpandirLinha([]));
  }, [dispatch]);

  useEffect(() => {
    resetarTela();
  }, [usuario.turmaSelecionada]);

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
        semestre: usuario.turmaSelecionada.periodo,
      };
      const dados = await api
        .get('v1/avaliacoes/notas/', { params })
        .catch(e => erros(e));

      const resultado = dados ? dados.data : [];
      if (
        resultado.percentualAlunosInsuficientes &&
        resultado.percentualAlunosInsuficientes > 0
      ) {
        setPercentualMinimoAprovados(resultado.percentualAlunosInsuficientes);
      }
      return resultado;
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
        validaPeriodoFechamento(dados);
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

            setBimestreCorrente(dados.bimestreAtual);
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
        setTimeout(() => {
          setCarregandoListaBimestres(false);
        }, 700);
      } else {
        resetarTela();
      }
    },
    [obterBimestres, resetarTela]
  );

  const obterDisciplinas = useCallback(async () => {
    const url = `v1/professores/turmas/${usuario.turmaSelecionada.turma}/disciplinas`;
    const disciplinas = await api.get(url).then(res => {
      if (res.data) setDesabilitarDisciplina(false);

      return res;
    });

    setListaDisciplinas(disciplinas.data);
    if (disciplinas.data && disciplinas.data.length === 1) {
      const disciplina = disciplinas.data[0];
      setPodeLancaNota(disciplina.lancaNota);
      setEhRegencia(disciplina.regencia);
      setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
      setDesabilitarDisciplina(true);
      if (disciplina.lancaNota) {
        obterDadosBimestres(disciplina.codigoComponenteCurricular);
      }
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
    if (usuario && usuario.turmaSelecionada && usuario.turmaSelecionada.turma) {
      const url = `v1/avaliacoes/notas/turmas/${usuario.turmaSelecionada.turma}/anos-letivos/${usuario.turmaSelecionada.anoLetivo}/tipos?consideraHistorico=${usuario.turmaSelecionada.consideraHistorico}`;
      const tipoNotaTurmaSelecionada = await api.get(url);
      if (
        Number(notasConceitos.Conceitos) ===
        Number(tipoNotaTurmaSelecionada.data)
      ) {
        return 'Lançamento de Conceitos';
      }
      return 'Lançamento de Notas';
    }
  }, [usuario.turmaSelecionada.anoLetivo, usuario.turmaSelecionada.turma]);

  useEffect(() => {
    obterTituloTela().then(titulo => {
      setTituloNotasConceitos(titulo);
    });
  }, [obterTituloTela]);

  useEffect(() => {
    if (
      !ehTurmaInfantil(modalidadesFiltroPrincipal, usuario.turmaSelecionada)
    ) {
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
    primeiroBimestre.alunos = [];
    primeiroBimestre.avaliacoes = [];
    setPrimeiroBimestre(primeiroBimestre);
    segundoBimestre.alunos = [];
    segundoBimestre.avaliacoes = [];
    setSegundoBimestre(segundoBimestre);
    terceiroBimestre.alunos = [];
    terceiroBimestre.avaliacoes = [];
    setTerceiroBimestre(terceiroBimestre);
    quartoBimestre.alunos = [];
    quartoBimestre.avaliacoes = [];
    setQuartoBimestre(quartoBimestre);
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
          const avaliacaoNota = bimestreParaMontar.avaliacoes.find(
            a => a.id === nota.atividadeAvaliativaId
          );
          if (
            window.moment(avaliacaoNota.data) > window.moment(new Date()) &&
            !nota.notaConceito
          )
            return;
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
            disciplinaId: notaFinal.disciplinaId || disciplinaSelecionada,
            nota:
              notaTipo === notasConceitos.Notas ? notaFinal.notaConceito : null,
            conceitoId:
              notaTipo === notasConceitos.Conceitos
                ? notaFinal.notaConceito
                : null,
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
      justificativa: bimestreParaMontar.justificativa,
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
          let valoresBimestresSalvarComNotas = valoresBimestresSalvar.filter(
            x => x.notaConceitoAlunos.length > 0
          );

          if (valoresBimestresSalvarComNotas.length < 1) return resolve(false);

          return api
            .post(`/v1/fechamentos/turmas`, valoresBimestresSalvarComNotas)
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
    if (modoEdicaoGeral || modoEdicaoGeralNotaFinal) {
      validarJustificativaAntesDeSalvar(bimestreCorrente, false, true);
    } else {
      irParaHome();
    }
  };

  const onClickSalvar = () => {
    validarJustificativaAntesDeSalvar(bimestreCorrente, true, false);
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
    let lancaNota = true;
    if (disciplinaId) {
      const componenteSelecionado = listaDisciplinas.find(
        item => item.codigoComponenteCurricular == disciplinaId
      );
      if (componenteSelecionado) {
        lancaNota = componenteSelecionado.lancaNota;
      }
    }
    setPodeLancaNota(lancaNota);

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
      if (lancaNota) {
        obterDadosBimestres(disciplinaId, 0);
      }
      setDisciplinaSelecionada(disciplinaId);
    }
  };

  const getDadosBimestreAtual = () => {
    switch (Number(bimestreCorrente)) {
      case 1:
        return primeiroBimestre;
      case 2:
        return segundoBimestre;
      case 3:
        return terceiroBimestre;
      case 4:
        return quartoBimestre;
      default:
        break;
    }
  };

  const verificaPorcentagemAprovados = () => {
    return ServicoNotas.temQuantidadeMinimaAprovada(
      getDadosBimestreAtual(),
      percentualMinimoAprovados,
      notaTipo
    );
  };

  const aposValidarJustificativaAntesDeSalvar = (
    numeroBimestre,
    clicouSalvar,
    clicouVoltar
  ) => {
    if (!clicouSalvar && !clicouVoltar) {
      confirmarTrocaTab(numeroBimestre);
    }
    if (clicouVoltar) {
      irParaHome();
    }
  };

  const validarJustificativaAntesDeSalvar = async (
    numeroBimestre,
    clicouSalvar = false,
    clicouVoltar = false
  ) => {
    setClicouNoBotaoSalvar(clicouSalvar);
    setClicouNoBotaoVoltar(clicouVoltar);

    if (modoEdicaoGeral || modoEdicaoGeralNotaFinal) {
      let confirmado = true;

      if (!clicouSalvar) {
        confirmado = await pergutarParaSalvar();
      }

      if (confirmado) {
        const bimestre = getDadosBimestreAtual();
        const temPorcentagemAceitavel = verificaPorcentagemAprovados();
        if (
          modoEdicaoGeralNotaFinal &&
          !temPorcentagemAceitavel &&
          bimestre.modoEdicao
        ) {
          setProximoBimestre(numeroBimestre);
          setExibeModalJustificativa(true);
        } else {
          bimestre.justificativa = temPorcentagemAceitavel
            ? null
            : bimestre.justificativa;
          await onSalvarNotas(clicouSalvar, modoEdicaoGeralNotaFinal);
          aposValidarJustificativaAntesDeSalvar(
            numeroBimestre,
            clicouSalvar,
            clicouVoltar
          );
        }
      } else {
        aposValidarJustificativaAntesDeSalvar(
          numeroBimestre,
          clicouSalvar,
          clicouVoltar
        );
      }
    } else {
      aposValidarJustificativaAntesDeSalvar(
        numeroBimestre,
        clicouSalvar,
        clicouVoltar
      );
    }
  };

  const onChangeTab = async numeroBimestre => {
    if (disciplinaSelecionada) {
      validarJustificativaAntesDeSalvar(numeroBimestre, false, false);
    }
  };

  const validaPeriodoFechamento = dados => {
    const temDados =
      dados.bimestres &&
      dados.bimestres.find(
        bimestre => bimestre.alunos && bimestre.alunos.length
      );
    if (temDados) {
      validaSeDesabilitaCampos(dados.bimestreAtual);
    } else {
      setShowMsgPeriodoFechamento(false);
    }
  };

  const confirmarTrocaTab = async numeroBimestre => {
    if (disciplinaSelecionada) {
      resetarBimestres();
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

      setBimestreCorrente(numeroBimestre);

      setCarregandoListaBimestres(true);
      const dados = await obterBimestres(disciplinaSelecionada, numeroBimestre);
      validaPeriodoFechamento(dados);
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
        setNotaTipo(dados.notaTipo);

        setNotaTipo(dados.notaTipo);

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
          justificativa: bimestrePesquisado.justificativa,
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

  const onChangeJustificativa = valor => {
    getDadosBimestreAtual().justificativa = valor;
  };

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  const onConfirmarJustificativa = async () => {
    setExibeModalJustificativa(false);
    await onSalvarNotas(clicouNoBotaoSalvar, modoEdicaoGeralNotaFinal);
    refForm.resetForm();
    aposValidarJustificativaAntesDeSalvar(
      proximoBimestre,
      clicouNoBotaoSalvar,
      clicouNoBotaoVoltar
    );
  };

  return (
    <Container>
      <ModalConteudoHtml
        key="inserirJutificativa"
        visivel={exibeModalJustificativa}
        onClose={() => {}}
        titulo="Inserir justificativa"
        esconderBotaoPrincipal
        esconderBotaoSecundario
        closable={false}
        fecharAoClicarFora={false}
        fecharAoClicarEsc={false}
        width="650px"
      >
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={onConfirmarJustificativa}
          ref={refF => setRefForm(refF)}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form>
              <div className="col-md-12">
                <Alert
                  alerta={{
                    tipo: 'warning',
                    id: 'justificativa-porcentagem',
                    mensagem: `A maioria dos estudantes está com ${
                      notasConceitos.Notas == notaTipo ? 'notas' : 'conceitos'
                    } abaixo do
                               mínimo considerado para aprovação, por isso é necessário que você insira uma justificativa.`,
                    estiloTitulo: { fontSize: '18px' },
                  }}
                  className="mb-2"
                />
              </div>
              <div className="col-md-12">
                <fieldset className="mt-3">
                  <Editor
                    form={form}
                    onChange={onChangeJustificativa}
                    name="descricao"
                  />
                </fieldset>
              </div>
              <div className="d-flex justify-content-end">
                <Button
                  key="btn-cancelar-justificativa"
                  label="Cancelar"
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={() => {
                    onChangeJustificativa('');
                    form.resetForm();
                    setExibeModalJustificativa(false);
                  }}
                />
                <Button
                  key="btn-sim-confirmacao-justificativa"
                  label="Confirmar"
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={() => validaAntesDoSubmit(form)}
                />
              </div>
            </Form>
          )}
        </Formik>
      </ModalConteudoHtml>
      {!usuario.turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, usuario.turmaSelecionada) ? (
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
      {!podeLancaNota ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'pode-lanca-nota',
                  mensagem:
                    'Este componente curricular não permite o lançamento de nota',
                  estiloTitulo: { fontSize: '18px' },
                }}
                className="mb-2"
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      {showMsgPeriodoFechamento &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, usuario.turmaSelecionada) ? (
        <Row className="mb-0 pb-0">
          <Grid cols={12} className="mb-0 pb-0">
            <Container>
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'alerta-perido-fechamento',
                  mensagem:
                    'Apenas é possível consultar este registro pois o período não está em aberto.',
                  estiloTitulo: { fontSize: '18px' },
                }}
              />
            </Container>
          </Grid>
        </Row>
      ) : null}
      <AlertaModalidadeInfantil />
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
                  allowClear={false}
                />
              </div>
            </div>

            {disciplinaSelecionada && podeLancaNota ? (
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
                            disciplinaSelecionada={disciplinaSelecionada}
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
