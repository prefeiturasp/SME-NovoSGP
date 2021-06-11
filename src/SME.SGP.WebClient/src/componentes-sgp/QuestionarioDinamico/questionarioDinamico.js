import { Form, Formik } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { Label } from '~/componentes';
import tipoQuestao from '~/dtos/tipoQuestao';
import AtendimentoClinicoTabela from './Componentes/AtendimentoClinico/atendimentoClinicoTabela';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import CampoDinamicoCombo from './Componentes/campoDinamicoCombo';
import CampoDinamicoComboMultiplaEscolha from './Componentes/campoDinamicoComboMultiplaEscolha';
import CampoDinamicoRadio from './Componentes/campoDinamicoRadio';
import CampoDinamicoTexto from './Componentes/campoDinamicoTexto';
import CampoDinamicoUploadArquivos from './Componentes/campoDinamicoUploadArquivos';
import InformacoesEscolares from './Componentes/InformacoesEscolares/informacoesEscolares';
import QuestionarioDinamicoFuncoes from './Funcoes/QuestionarioDinamicoFuncoes';
import QuestionarioDinamicoValidacoes from './Validacoes/QuestionarioDinamicoValidacoes';
import DiasHorariosTabela from './Componentes/DiasHorariosTabela/diasHorariosTabela';
import CampoDinamicoPeriodo from './Componentes/campoDinamicoPeriodo';
import CampoDinamicoCheckbox from './Componentes/campoDinamicoCheckbox';
import CampoDinamicoPeriodoEscolar from './Componentes/campoDinamicoPeriodoEscolar';

const QuestionarioDinamico = props => {
  const dispatch = useDispatch();

  const {
    dados,
    dadosQuestionarioAtual,
    desabilitarCampos,
    codigoAluno,
    codigoTurma,
    anoLetivo,
    urlUpload,
    funcaoRemoverArquivoCampoUpload,
    onChangeQuestionario,
    turmaId,
  } = props;

  const [valoresIniciais, setValoresIniciais] = useState();

  const [refForm, setRefForm] = useState({});

  const obterForm = () => refForm;

  useEffect(() => {
    if (refForm) {
      QuestionarioDinamicoFuncoes.adicionarFormsQuestionarioDinamico(
        () => obterForm(),
        dados.questionarioId,
        dadosQuestionarioAtual,
        dados?.id
      );
    }
  }, [refForm]);

  const montarValoresIniciais = useCallback(() => {
    const valores = {};

    const montarDados = questaoAtual => {
      const resposta = questaoAtual?.resposta;

      let valorRespostaAtual = '';

      if (resposta?.length) {
        switch (questaoAtual?.tipoQuestao) {
          case tipoQuestao.Radio:
            valorRespostaAtual = resposta[0].opcaoRespostaId;
            break;
          case tipoQuestao.Combo:
            valorRespostaAtual = String(resposta[0].opcaoRespostaId || '');
            break;
          case tipoQuestao.ComboMultiplaEscolha:
            valorRespostaAtual = resposta.map(r => String(r.opcaoRespostaId));
            break;
          case tipoQuestao.Checkbox:
            valorRespostaAtual = resposta.map(r => Number(r.opcaoRespostaId));
            break;
          case tipoQuestao.Texto:
          case tipoQuestao.PeriodoEscolar:
            valorRespostaAtual = resposta[0].texto;
            break;
          case tipoQuestao.Periodo:
            valorRespostaAtual = {
              periodoInicio: moment(resposta[0].periodoInicio),
              periodoFim: moment(resposta[0].periodoFim),
            };
            break;
          case tipoQuestao.FrequenciaEstudanteAEE:
          case tipoQuestao.AtendimentoClinico:
            valorRespostaAtual = resposta[0].texto
              ? JSON.parse(resposta[0].texto)
              : '';
            break;
          case tipoQuestao.Upload:
            if (resposta?.length) {
              valorRespostaAtual = resposta
                ?.map(item => {
                  const { arquivo } = item;
                  if (arquivo) {
                    return {
                      uid: arquivo.codigo,
                      xhr: arquivo.codigo,
                      name: arquivo.nome,
                      status: 'done',
                      arquivoId: arquivo.id,
                    };
                  }
                  return '';
                })
                .filter(a => !!a);
            } else {
              valorRespostaAtual = [];
            }
            break;
          default:
            break;
        }
      }

      if (
        valorRespostaAtual?.length &&
        (questaoAtual?.tipoQuestao === tipoQuestao.ComboMultiplaEscolha ||
          questaoAtual?.tipoQuestao === tipoQuestao.Checkbox)
      ) {
        const idsQuestoesComResposta = valorRespostaAtual.filter(valorSalvo => {
          const opcaoResposta = questaoAtual?.opcaoResposta.find(
            q => String(q.id) === String(valorSalvo)
          );

          if (
            opcaoResposta?.questoesComplementares?.find(q => q.resposta?.length)
          ) {
            return true;
          }
          return false;
        });

        if (idsQuestoesComResposta?.length) {
          idsQuestoesComResposta.forEach(idQuestao => {
            const questaoComplmentarComResposta = questaoAtual?.opcaoResposta.find(
              q => String(q.id) === String(idQuestao)
            );

            if (questaoComplmentarComResposta?.questoesComplementares?.length) {
              questaoComplmentarComResposta.questoesComplementares.forEach(
                questao => {
                  montarDados(questao);
                }
              );
            }
          });
        }
      } else if (
        valorRespostaAtual &&
        questaoAtual?.tipoQuestao !== tipoQuestao.Upload &&
        questaoAtual?.tipoQuestao !== tipoQuestao.Texto
      ) {
        const opcaoAtual = questaoAtual?.opcaoResposta.find(
          item => String(item.id) === String(valorRespostaAtual)
        );

        if (opcaoAtual?.questoesComplementares?.length) {
          opcaoAtual.questoesComplementares.forEach(q => {
            montarDados(q);
          });
        }
      }

      if (
        !valorRespostaAtual &&
        questaoAtual?.tipoQuestao === tipoQuestao.Periodo
      ) {
        valores[questaoAtual.id] = {};
      } else {
        valores[questaoAtual.id] = valorRespostaAtual;
      }
    };

    dadosQuestionarioAtual.forEach(questaoAtual => {
      montarDados(questaoAtual);
    });

    setValoresIniciais({ ...valores });
  }, [dadosQuestionarioAtual]);

  useEffect(() => {
    if (dadosQuestionarioAtual?.length) {
      montarValoresIniciais();
    }
  }, [dadosQuestionarioAtual, montarValoresIniciais]);

  const campoAtendimentoClinico = params => {
    const { questaoAtual, label, form } = params;

    return (
      <div className="col-md-12 mb-3">
        <AtendimentoClinicoTabela
          desabilitado={desabilitarCampos}
          label={label}
          form={form}
          questaoAtual={questaoAtual}
          onChange={() => {
            dispatch(setQuestionarioDinamicoEmEdicao(true));
            onChangeQuestionario();
          }}
        />
      </div>
    );
  };

  const labelPersonalizado = (textolabel, observacaoText) => (
    <Label text={textolabel} observacaoText={observacaoText} />
  );

  const montarCampos = (questaoAtual, form, ordemAnterior, ordemSequencial) => {
    const campoQuestaoComplementar = [];

    const montarCampoComplementarPadrao = (vAtual, label, ordemSeq) => {
      const opcaoResposta = QuestionarioDinamicoFuncoes.obterOpcaoRespostaPorId(
        questaoAtual?.opcaoResposta,
        vAtual
      );

      if (opcaoResposta?.questoesComplementares?.length) {
        opcaoResposta.questoesComplementares.forEach(q => {
          campoQuestaoComplementar.push(montarCampos(q, form, label, ordemSeq));
        });
      }
    };

    const ordemLabel = ordemAnterior
      ? `${ordemAnterior}.${ordemSequencial || questaoAtual.ordem}`
      : questaoAtual.ordem;

    const textoLabel = `${ordemLabel} - ${questaoAtual.nome}`;
    const label = labelPersonalizado(textoLabel, questaoAtual?.observacao);

    const valorAtualSelecionado = form.values[questaoAtual.id];

    if (
      questaoAtual?.tipoQuestao === tipoQuestao.ComboMultiplaEscolha ||
      questaoAtual?.tipoQuestao === tipoQuestao.Checkbox
    ) {
      if (valorAtualSelecionado?.length) {
        const listaCamposRenderizar = [];

        const todosCamposComplementares = QuestionarioDinamicoFuncoes.obterTodosCamposComplementares(
          valorAtualSelecionado,
          questaoAtual
        );

        const camposSemEspaco = todosCamposComplementares.map(m => {
          return { ...m, nome: m.nome.trim() };
        });

        const camposDuplicados = QuestionarioDinamicoFuncoes.agruparCamposDuplicados(
          camposSemEspaco,
          'nome'
        );

        if (camposDuplicados?.length) {
          camposDuplicados.forEach(c => {
            // Na base vai ter somente 2 campos com mesmo nome para essa rotina 1 obrigatório e outro não!
            const campoRenderizar = c.questoesDuplicadas?.find(
              co => co.obrigatorio
            );

            if (campoRenderizar) {
              listaCamposRenderizar.push(campoRenderizar);
            }
          });
        }

        if (camposSemEspaco?.length && camposDuplicados?.length) {
          const camposNaoDuplicados = QuestionarioDinamicoFuncoes.obterCamposNaoDuplicados(
            camposSemEspaco,
            camposDuplicados
          );
          if (camposNaoDuplicados?.length) {
            camposNaoDuplicados.forEach(c => {
              listaCamposRenderizar.push(c);
            });
          }
        } else {
          camposSemEspaco.forEach(c => {
            listaCamposRenderizar.push(c);
          });
        }

        listaCamposRenderizar.forEach(q => {
          campoQuestaoComplementar.push(montarCampos(q, form, ordemLabel));
        });
      }
    } else if (valorAtualSelecionado) {
      montarCampoComplementarPadrao(valorAtualSelecionado, ordemLabel);
    }

    const params = {
      questaoAtual,
      form,
      label,
    };

    let campoAtual = null;
    switch (questaoAtual?.tipoQuestao) {
      case tipoQuestao.Radio:
        campoAtual = (
          <CampoDinamicoRadio
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valorAtual => {
              QuestionarioDinamicoFuncoes.onChangeCamposComOpcaoResposta(
                questaoAtual,
                form,
                valorAtual
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.Checkbox:
        campoAtual = (
          <CampoDinamicoCheckbox
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valorAtual => {
              QuestionarioDinamicoFuncoes.onChangeCampoCheckboxOuComboMultiplaEscolha(
                questaoAtual,
                form,
                valorAtual
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.Combo:
        campoAtual = (
          <CampoDinamicoCombo
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valorAtual => {
              QuestionarioDinamicoFuncoes.onChangeCamposComOpcaoResposta(
                questaoAtual,
                form,
                valorAtual
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.ComboMultiplaEscolha:
        campoAtual = (
          <CampoDinamicoComboMultiplaEscolha
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={valoresSelecionados => {
              QuestionarioDinamicoFuncoes.onChangeCampoCheckboxOuComboMultiplaEscolha(
                questaoAtual,
                form,
                valoresSelecionados
              );
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.Texto:
        campoAtual = (
          <CampoDinamicoTexto
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.InformacoesEscolares:
        campoAtual = (
          <div className="col-md-12 mb-3">
            <InformacoesEscolares
              codigoAluno={codigoAluno}
              codigoTurma={codigoTurma}
              anoLetivo={anoLetivo}
            />
          </div>
        );
        break;
      case tipoQuestao.AtendimentoClinico:
        campoAtual = campoAtendimentoClinico(params);
        break;
      case tipoQuestao.Upload:
        campoAtual = (
          <CampoDinamicoUploadArquivos
            dados={params}
            desabilitado={desabilitarCampos}
            urlUpload={urlUpload}
            funcaoRemoverArquivoCampoUpload={funcaoRemoverArquivoCampoUpload}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.Periodo:
        campoAtual = (
          <CampoDinamicoPeriodo
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
          />
        );
        break;
      case tipoQuestao.FrequenciaEstudanteAEE:
        campoAtual = (
          <div className="col-md-12 mb-3">
            <DiasHorariosTabela
              desabilitado={desabilitarCampos}
              label={label}
              form={form}
              questaoAtual={questaoAtual}
              onChange={() => {
                dispatch(setQuestionarioDinamicoEmEdicao(true));
                onChangeQuestionario();
              }}
            />
          </div>
        );
        break;
      case tipoQuestao.PeriodoEscolar:
        campoAtual = (
          <CampoDinamicoPeriodoEscolar
            questaoAtual={questaoAtual}
            form={form}
            label={label}
            desabilitado={desabilitarCampos}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
              onChangeQuestionario();
            }}
            turmaId={turmaId}
          />
        );
        break;
      default:
        break;
    }

    return (
      <>
        {campoAtual || ''}
        {campoQuestaoComplementar?.length ? campoQuestaoComplementar : ''}
      </>
    );
  };

  const montarQuestionarioAtual = (data, form) => {
    const campos = data.map(questaoAtual => {
      return (
        <div className="row" key={questaoAtual.id}>
          {montarCampos(questaoAtual, form, '')}
        </div>
      );
    });

    return campos;
  };

  return dados?.questionarioId > -1 &&
    dadosQuestionarioAtual?.length &&
    valoresIniciais ? (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={() =>
        QuestionarioDinamicoValidacoes.obterValidationSchema(
          dadosQuestionarioAtual,
          refForm
        )
      }
      validateOnChange
      validateOnBlur
      ref={refFormik => setRefForm(refFormik)}
    >
      {form => (
        <Form className="col-md-12">
          {montarQuestionarioAtual(dadosQuestionarioAtual, form)}
        </Form>
      )}
    </Formik>
  ) : (
    ''
  );
};

QuestionarioDinamico.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  dadosQuestionarioAtual: PropTypes.oneOfType([PropTypes.any]),
  desabilitarCampos: PropTypes.bool,
  codigoAluno: PropTypes.oneOfType([PropTypes.any]),
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  anoLetivo: PropTypes.oneOfType([PropTypes.any]),
  urlUpload: PropTypes.string,
  funcaoRemoverArquivoCampoUpload: PropTypes.func,
  onChangeQuestionario: PropTypes.func,
  turmaId: PropTypes.oneOfType([PropTypes.any]),
};

QuestionarioDinamico.defaultProps = {
  dados: {},
  dadosQuestionarioAtual: {},
  desabilitarCampos: false,
  codigoAluno: '',
  codigoTurma: '',
  anoLetivo: null,
  urlUpload: '',
  funcaoRemoverArquivoCampoUpload: () => {},
  onChangeQuestionario: () => {},
  turmaId: null,
};

export default QuestionarioDinamico;
