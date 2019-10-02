import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  Salvar,
  PrePost,
  Post,
  setBimestresErro,
  setLimpartBimestresErro,
  LimparBimestres,
} from '../../../redux/modulos/planoAnual/action';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import { Colors, Base } from '../../../componentes/colors';
import _ from 'lodash';
import Card from '../../../componentes/card';
import Bimestre from './bimestre';
import Row from '../../../componentes/row';
import Service from '../../../servicos/Paginas/PlanoAnualServices';
import Alert from '../../../componentes/alert';
import ModalMultiLinhas from '../../../componentes/modalMultiLinhas';
import ModalConfirmacao from '../../../componentes/modalConfirmacao';
import history from '../../../servicos/history';
import { URL_HOME } from '../../../constantes/url';
import { erro, sucesso } from '../../../servicos/alertas';
import ModalConteudoHtml from '../../../componentes/modalConteudoHtml';
import {
  Titulo,
  TituloAno,
  Planejamento,
  RegistroMigrado,
} from './planoAnual.css.js';
import modalidade from '~/dtos/modalidade';
import SelectComponent from '~/componentes/select';

export default function PlanoAnual() {
  const bimestres = useSelector(store => store.bimestres.bimestres);
  const notificacoes = useSelector(store => store.notificacoes);
  const bimestresErro = useSelector(store => store.bimestres.bimestresErro);
  const usuario = useSelector(store => store.usuario);

  const turmaSelecionada = usuario.turmaSelecionada;
  const emEdicao = bimestres.filter(x => x.ehEdicao).length > 0;
  const ehDisabled = usuario.turmaSelecionada.length === 0;
  const dispatch = useDispatch();
  const [modalConfirmacaoVisivel, setModalConfirmacaoVisivel] = useState({
    modalVisivel: false,
    sairTela: false,
  });

  const ehEja =
    turmaSelecionada[0] && turmaSelecionada[0].codModalidade === modalidade.EJA
      ? true
      : false;

  const ehMedio =
    turmaSelecionada[0] &&
    turmaSelecionada[0].codModalidade === modalidade.ENSINO_MEDIO
      ? true
      : false;

  const [disciplinaSemObjetivo, setDisciplinaSemObjetivo] = useState(false);
  const [modalCopiarConteudo, setModalCopiarConteudo] = useState({
    visivel: false,
    listSelect: [],
    turmasSelecionadas: [],
    turmasComPlanoAnual: [],
    loader: false,
  });

  const recarregarPlanoAnual =
    bimestres.filter(bimestre => bimestre.recarregarPlanoAnual).length > 0;

  const LayoutEspecial = ehEja || ehMedio || disciplinaSemObjetivo;

  const anoLetivo = turmaSelecionada[0] ? turmaSelecionada[0].anoLetivo : 0;
  const escolaId = turmaSelecionada[0] ? turmaSelecionada[0].codEscola : 0;
  const anoEscolar = turmaSelecionada[0] ? turmaSelecionada[0].ano : 0;
  const turmaId = turmaSelecionada[0] ? turmaSelecionada[0].codTurma : 0;

  useEffect(() => {
    VerificarEnvio();

    if (recarregarPlanoAnual) verificarSeEhEdicao();
  }, [bimestres]);

  useEffect(() => {
    if (!ehDisabled) verificarSeEhEdicao();
  }, [turmaSelecionada]);

  const onF5Click = e => {
    if (e.code === 'F5') {
      if (emEdicao) {
        e.preventDefault();
        setModalConfirmacaoVisivel({
          modalVisivel: true,
          sairTela: false,
        });
      }
    }
  };

  document.onkeydown = onF5Click;
  document.onkeypress = onF5Click;
  document.onkeyup = onF5Click;

  const VerificarEnvio = () => {
    const BimestresParaEnviar = bimestres.filter(x => x.paraEnviar);

    if (BimestresParaEnviar && BimestresParaEnviar.length > 0)
      dispatch(Post(BimestresParaEnviar));
  };

  const verificarSeEhEdicao = () => {
    if (!turmaSelecionada[0]) return;

    Service.validarPlanoExistente({
      AnoLetivo: anoLetivo,
      Bimestre: 1,
      EscolaId: escolaId,
      TurmaId: turmaId,
    })
      .then(ehEdicao => {
        Service.getDisciplinasProfessor(usuario.rf, turmaId)
          .then(res => {
            ObtenhaBimestres(_.cloneDeep(res), !ehEdicao);
          })
          .catch(() => {
            erro(`Não foi possivel obter as disciplinas do professor`);
          });
      })
      .catch(() => {
        erro(
          `Não foi possivel obter os dados do ${
            ehEja ? 'plano semestral' : 'plano anual'
          }`
        );
      });
  };

  const ObtenhaNomebimestre = index => `${index}º Bimestre`;

  const confirmarCancelamento = () => {
    if (modalConfirmacaoVisivel.sairTela) {
      history.push(URL_HOME);
    } else {
      cancelarModalConfirmacao();
      verificarSeEhEdicao();
    }
  };

  const onClickCancelar = () => {
    verificarSeEhEdicao();
  };

  const cancelarModalConfirmacao = () => {
    setModalConfirmacaoVisivel({
      modalVisivel: false,
      sairTela: false,
    });
  };

  const onClickSalvar = () => {
    dispatch(PrePost());
  };

  const onCopiarConteudoClick = async () => {
    Service.obterBimestre({
      AnoLetivo: anoLetivo,
      Bimestre: 1,
      EscolaId: escolaId,
      TurmaId: turmaId,
    })
      .then(async res => {
        if (res.status === 200) {
          const turmasCopiarConteudo = await ObtenhaTurmasCopiarConteudo();

          if (!turmasCopiarConteudo) {
            return;
          }

          if (turmasCopiarConteudo.length === 0) {
            erro('Nenhuma turma elegivel para copiar o conteudo');
            return;
          }

          const disciplinasAtual = bimestres[1].materias.map(materia => {
            return {
              codigo: materia.codigo,
              materia: materia.materia,
            };
          });
          const promissesTurmas = [];

          for (let i = 0; i < turmasCopiarConteudo.length; i++) {
            promissesTurmas.push(
              Service.getDisciplinasProfessor(
                usuario.rf,
                turmasCopiarConteudo[i].codigo
              )
            );
          }

          Promise.all(promissesTurmas)
            .then(resultados => {
              for (let i = 0; i < resultados.length; i++) {
                const disciplinasResultado = resultados[i];

                turmasCopiarConteudo[i].disponivelCopia = _.isEqual(
                  disciplinasAtual.map(x => x.codigo),
                  disciplinasResultado.map(x => x.codigo)
                );

                const temTurmaElegivel =
                  turmasCopiarConteudo.filter(turma => turma.disponivelCopia)
                    .length > 0;

                if (temTurmaElegivel) {
                  modalCopiarConteudo.listSelect = turmasCopiarConteudo;
                  modalCopiarConteudo.visivel = true;
                  modalCopiarConteudo.turmasComPlanoAnual = turmasCopiarConteudo
                    .filter(x => x.temPlano)
                    .map(x => x.codigo);

                  setModalCopiarConteudo({ ...modalCopiarConteudo });
                } else {
                  erro(
                    'Não há nenhuma turma elegivel para receber a copia deste plano'
                  );
                }
              }
            })
            .catch(() => {
              erro('Não foi possivel obter as turmas disponiveis');
            });
        } else {
          erro('Este plano ainda não foi salvo na base de dados');
        }
      })
      .catch(() => {
        erro('Não foi possivel obter os dados do plano');
      });
  };

  const ObtenhaBimestres = (disciplinas = [], ehEdicao) => {
    dispatch(LimparBimestres());

    let semObjetivo = false;

    const qtdBimestres = ehEja ? 2 : 4;

    const disciplinasComObjetivo = disciplinas.filter(
      disciplina => disciplina.possuiObjetivos
    );

    if (disciplinasComObjetivo.length === 0) semObjetivo = true;

    for (let i = 1; i <= qtdBimestres; i++) {
      const Nome = ObtenhaNomebimestre(i);

      const objetivo = '';

      const bimestre = {
        anoLetivo,
        anoEscolar,
        escolaId,
        turmaId,
        ehExpandido: false,
        indice: i,
        nome: Nome,
        materias: disciplinas,
        objetivo: objetivo,
        paraEnviar: false,
        recarregarPlanoAnual: false,
        ehEdicao,
        LayoutEspecial: LayoutEspecial || semObjetivo,
        ehExpandido: ehEdicao,
        jaSincronizou: false,
      };

      dispatch(Salvar(i, bimestre));
    }

    setDisciplinaSemObjetivo(semObjetivo);
  };

  const ObtenhaTurmasCopiarConteudo = async () => {
    const turmasIrmas = usuario.turmasUsuario.filter(
      turma =>
        turma.ano === turmaSelecionada[0].ano &&
        turma.codEscola === turmaSelecionada[0].codEscola &&
        turma.codigo !== turmaSelecionada[0].codTurma
    );

    const promissesTurmas = [];

    turmasIrmas.forEach(turma => {
      const promise = Service.validarPlanoExistente({
        AnoLetivo: anoLetivo,
        Bimestre: 1,
        EscolaId: escolaId,
        TurmaId: turma.codigo,
      });

      promissesTurmas.push(promise);
    });

    return Promise.all(promissesTurmas)
      .then(res => {
        res.forEach((resposta, indice) => {
          if (resposta) turmasIrmas[indice].temPlano = resposta;
        });

        return turmasIrmas;
      })
      .catch(() => {
        erro('Não foi possivel obter as disciplinas elegiveis');
        return null;
      });
  };

  const modalCopiarConteudoAlertaVisivel = () => {
    return modalCopiarConteudo.turmasSelecionadas.some(selecionada =>
      modalCopiarConteudo.turmasComPlanoAnual.includes(selecionada * 1)
    );
  };

  const modalCopiarConteudoAtencaoTexto = () => {
    const turmasReportar = usuario.turmasUsuario
      ? usuario.turmasUsuario
          .filter(
            turma =>
              modalCopiarConteudo.turmasSelecionadas.includes(
                `${turma.codigo}`
              ) &&
              modalCopiarConteudo.turmasComPlanoAnual.includes(turma.codigo)
          )
          .map(turma => turma.turma)
      : [];

    return turmasReportar.length > 1
      ? `As turmas ${turmasReportar.join(
          ', '
        )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      : `A turma ${
          turmasReportar[0]
        } já possui plano anual que será sobrescrito ao realizar a cópia. Deseja continuar?`;
  };

  const onChangeCopiarConteudo = selecionadas => {
    modalCopiarConteudo.turmasSelecionadas = selecionadas;
    setModalCopiarConteudo({ ...modalCopiarConteudo });
  };

  const onCloseCopiarConteudo = () => {
    setModalCopiarConteudo({
      ...modalCopiarConteudo,
      visivel: false,
      listSelect: [],
      turmasSelecionadas: [],
      loader: false,
      turmasComPlanoAnual: [],
    });
  };

  const onConfirmarCopiarConteudo = () => {
    setModalCopiarConteudo({
      ...modalCopiarConteudo,
      loader: true,
    });

    CopiarConteudo();
  };

  const CopiarConteudo = async () => {
    const promissesBimestres = [];

    const qtdBimestres = ehEja ? 2 : 4;

    for (let i = 1; i <= qtdBimestres; i++) {
      const promise = Service.obterBimestre({
        AnoLetivo: anoLetivo,
        Bimestre: i,
        EscolaId: escolaId,
        TurmaId: turmaId,
      });

      promissesBimestres.push(promise);
    }

    Promise.all(promissesBimestres)
      .then(resultados => {
        const PlanoAnualEnviar = {
          Id: resultados[0].data.id,
          AnoLetivo: bimestres[1].anoLetivo,
          EscolaId: bimestres[1].escolaId,
          TurmaId: bimestres[1].turmaId,
          Bimestres: [],
        };

        resultados.forEach((res, index) => {
          const bimestrePlanoAnual = res.data;
          PlanoAnualEnviar.Bimestres.push({
            Bimestre: index + 1,
            ObjetivosAprendizagem: bimestrePlanoAnual.objetivosAprendizagem,
            Descricao: bimestrePlanoAnual.descricao,
          });
        });

        return PlanoAnualEnviar;
      })
      .then(PlanoAnualEnviar => {
        Service.copiarConteudo(
          PlanoAnualEnviar,
          usuario.rf,
          modalCopiarConteudo.turmasSelecionadas
        )
          .then(() => sucesso('Plano copiado com sucesso'))
          .catch(erro => {
            dispatch(
              setBimestresErro({
                type: 'erro',
                content: erro.error,
                title: 'Ocorreu uma falha',
                onClose: () => dispatch(setLimpartBimestresErro()),
                visible: true,
              })
            );
          })
          .finally(() => {
            onCloseCopiarConteudo();
          });
      })
      .catch(() => {
        erro('Não foi possivel copiar o conteudo');
      });
  };

  const onCancelarCopiarConteudo = () => {
    onCloseCopiarConteudo();
  };

  const voltarParaHome = () => {
    if (emEdicao)
      setModalConfirmacaoVisivel({
        modalVisivel: true,
        sairTela: true,
      });
    else history.push(URL_HOME);
  };

  return (
    <>
      <div className="col-md-12">
        {!turmaSelecionada[0] ? (
          <Row className="mb-0 pb-0">
            <Grid cols={12} className="mb-0 pb-0">
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma.',
                }}
                className="mb-0"
              />
            </Grid>
          </Row>
        ) : null}
      </div>
      <ModalMultiLinhas
        key="errosBimestre"
        visivel={bimestresErro.visible}
        onClose={bimestresErro.onClose}
        type={bimestresErro.type}
        conteudo={bimestresErro.content}
        titulo={bimestresErro.title}
      />
      <ModalConfirmacao
        key="confirmacaoDeSaida"
        visivel={modalConfirmacaoVisivel.modalVisivel}
        onConfirmacaoPrincipal={cancelarModalConfirmacao}
        onConfirmacaoSecundaria={confirmarCancelamento}
        onClose={cancelarModalConfirmacao}
        labelPrincipal="Não"
        labelSecundaria="Sim"
        titulo="Atenção"
        conteudo="Você não salvou as informações preenchidas"
        perguntaDoConteudo="Deseja realmente cancelar as alterações?"
      />
      <ModalConteudoHtml
        key={'copiarConteudo'}
        visivel={modalCopiarConteudo.visivel}
        onConfirmacaoPrincipal={onConfirmarCopiarConteudo}
        onConfirmacaoSecundaria={onCancelarCopiarConteudo}
        onClose={onCloseCopiarConteudo}
        labelBotaoPrincipal="Copiar"
        tituloAtencao={modalCopiarConteudoAlertaVisivel() ? 'Atenção' : null}
        perguntaAtencao={
          modalCopiarConteudoAlertaVisivel()
            ? modalCopiarConteudoAtencaoTexto()
            : null
        }
        labelBotaoSecundario="Cancelar"
        titulo="Copiar Conteúdo"
        closable={false}
        loader={modalCopiarConteudo.loader}
        desabilitarBotaoPrincipal={
          modalCopiarConteudo.turmasSelecionadas &&
          modalCopiarConteudo.turmasSelecionadas.length < 1
        }
      >
        <label
          htmlFor="SelecaoTurma"
          alt="Selecione uma ou mais turmas de destino"
        >
          Copiar para a(s) turma(s)
        </label>
        <SelectComponent
          id="SelecaoTurma"
          lista={modalCopiarConteudo.listSelect}
          valueOption="codigo"
          valueText="turma"
          onChange={onChangeCopiarConteudo}
          valueSelect={modalCopiarConteudo.turmasSelecionadas}
          multiple
        />
      </ModalConteudoHtml>
      <Grid cols={12} className="p-l-10 mb-10">
        <Planejamento>PLANEJAMENTO</Planejamento>
        <Titulo>
          {ehEja ? 'Plano Semestral' : 'Plano Anual'}
          <TituloAno>
            {` / ${anoLetivo ? anoLetivo : new Date().getFullYear()}`}
          </TituloAno>
        </Titulo>
        <RegistroMigrado className="float-right">
          Registro Migrado
        </RegistroMigrado>
      </Grid>
      <Card className="col-md-12 p-0">
        <Grid cols={6} className="d-flex justify-content-start mb-3">
          <Button
            label="Copiar Conteúdo"
            icon="share-square"
            color={Colors.Azul}
            onClick={onCopiarConteudoClick}
            border
            disabled={turmaSelecionada[0] ? false : true}
          />
        </Grid>
        <Grid cols={6} className="d-flex justify-content-end mb-3">
          <Button
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            onClick={voltarParaHome}
            border
            className="mr-3"
          />
          <Button
            label="Cancelar"
            color={Colors.Roxo}
            onClick={onClickCancelar}
            border
            bold
            className="mr-3"
          />
          <Button
            label="Salvar"
            color={Colors.Roxo}
            onClick={onClickSalvar}
            disabled={!emEdicao || ehDisabled}
            border={!emEdicao || ehDisabled}
            bold
          />
        </Grid>
        <Grid cols={12}>
          {bimestres
            ? bimestres.map(bim => {
                return (
                  <Bimestre
                    disabled={ehDisabled}
                    key={bim.indice}
                    indice={bim.indice}
                    LayoutEspecial={LayoutEspecial}
                  />
                );
              })
            : null}
        </Grid>
      </Card>
    </>
  );
}
