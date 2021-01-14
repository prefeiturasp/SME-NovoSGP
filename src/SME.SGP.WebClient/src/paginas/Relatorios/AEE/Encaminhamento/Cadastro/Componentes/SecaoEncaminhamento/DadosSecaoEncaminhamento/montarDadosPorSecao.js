import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { CampoTexto, RadioGroupButton, SelectComponent } from '~/componentes';
import tipoQuestao from '~/dtos/tipoQuestao';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import InformacoesEscolares from '../../IndicativosEstudante/indicativosEstudante';

const MontarDadosPorSecao = props => {
  const { dados, match } = props;

  const [dadosQuestionarioAtual, setDadosQuestionarioAtual] = useState();
  const [valoresIniciais, setValoresIniciais] = useState();

  const [refForm, setRefForm] = useState({});

  const obterQuestionario = useCallback(async questionarioId => {
    const encaminhamentoId = match?.params?.id;
    const resposta = await ServicoEncaminhamentoAEE.obterQuestionario(
      questionarioId,
      encaminhamentoId
    ).catch(e => erros(e));

    if (resposta?.data) {
      setDadosQuestionarioAtual(resposta.data);
    } else {
      setDadosQuestionarioAtual();
    }
  }, []);

  useEffect(() => {
    if (dados?.questionarioId) {
      obterQuestionario(dados?.questionarioId);
    } else {
      setDadosQuestionarioAtual();
    }
  }, [dados, obterQuestionario]);

  const obterForm = () => refForm;

  useEffect(() => {
    if (refForm) {
      ServicoEncaminhamentoAEE.addFormsSecoesEncaminhamentoAEE(
        () => obterForm(),
        dados.questionarioId,
        dadosQuestionarioAtual
      );
    }
  }, [refForm]);

  const montarValoresIniciais = useCallback(() => {
    const valores = {};
    dadosQuestionarioAtual.forEach(questaoAtual => {
      const resposta = questaoAtual?.resposta;
      if (resposta?.length) {
        switch (questaoAtual?.tipoQuestao) {
          case tipoQuestao.Radio:
            valores[questaoAtual.id] = resposta[0].opcaoRespostaId;
            break;
          case tipoQuestao.Combo:
            valores[questaoAtual.id] = String(resposta[0].texto || '');
            break;
          case tipoQuestao.Texto:
            valores[questaoAtual.id] = resposta[0].texto;
            break;
          default:
            break;
        }
      } else {
        valores[questaoAtual.id] = '';
      }
    });

    setValoresIniciais({ ...valores });
  }, [dadosQuestionarioAtual]);

  useEffect(() => {
    if (dadosQuestionarioAtual?.length) {
      montarValoresIniciais();
    }
  }, [dadosQuestionarioAtual, montarValoresIniciais]);

  const campoRadio = params => {
    const { questaoAtual, form, label } = params;

    const opcoes = questaoAtual?.opcaoResposta.map(item => {
      return { label: item.nome, value: item.id };
    });

    return (
      <div className="col-md-12 mb-3">
        <RadioGroupButton
          id={String(questaoAtual.id)}
          name={String(questaoAtual.id)}
          label={label}
          form={form}
          opcoes={opcoes}
        />
      </div>
    );
  };

  const campoCombo = params => {
    const { questaoAtual, form, label } = params;

    const lista = questaoAtual?.opcaoResposta.map(item => {
      return { desc: item.nome, valor: item.id };
    });

    return (
      <>
        <div className="col-sm-12 col-md-12 col-lg-6 col-xl-6 mb-3">
          <SelectComponent
            id={String(questaoAtual.id)}
            name={String(questaoAtual.id)}
            label={label}
            placeholder={questaoAtual.nome}
            form={form}
            lista={lista}
            valueOption="valor"
            valueText="desc"
          />
        </div>
      </>
    );
  };

  const campoTexto = params => {
    const { questaoAtual, form, label } = params;

    return (
      <div className="col-md-12 mb-3">
        <CampoTexto
          id={String(questaoAtual.id)}
          name={String(questaoAtual.id)}
          label={label}
          form={form}
          type="textarea"
        />
      </div>
    );
  };

  const montarCampos = (questaoAtual, form, ordemAnterior) => {
    const ordemLabel = ordemAnterior
      ? `${ordemAnterior}.${questaoAtual.ordem}`
      : questaoAtual.ordem;

    const label = `${ordemLabel} - ${questaoAtual.nome}`;

    let campoQuestaoComplementar = null;

    const valorAtualSelecionado = form.values[questaoAtual.id];

    if (valorAtualSelecionado) {
      const opcaoAtual = questaoAtual?.opcaoResposta.find(
        item => String(item.id) === String(valorAtualSelecionado)
      );

      if (opcaoAtual?.questaoComplementar) {
        campoQuestaoComplementar = montarCampos(
          opcaoAtual.questaoComplementar,
          form,
          ordemLabel
        );
      }
    }

    const params = {
      questaoAtual,
      form,
      label,
    };

    let campoAtual = null;
    switch (questaoAtual?.tipoQuestao) {
      case tipoQuestao.Radio:
        campoAtual = campoRadio(params);
        break;
      case tipoQuestao.Combo:
        campoAtual = campoCombo(params);
        break;
      case tipoQuestao.Texto:
        campoAtual = campoTexto(params);
        break;
      case tipoQuestao.InformacoesEscolares:
        campoAtual = (
          <div className="col-md-12 mb-3">
            <InformacoesEscolares dados={params} />
          </div>
        );
        break;
      default:
        break;
    }

    return (
      <>
        {campoAtual || ''}
        {campoQuestaoComplementar || ''}
      </>
    );
  };

  return dados?.questionarioId &&
    dadosQuestionarioAtual?.length &&
    valoresIniciais ? (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={{}}
      validateOnChange
      validateOnBlur
      ref={refFormik => setRefForm(refFormik)}
    >
      {form => (
        <Form className="col-md-12">
          {dadosQuestionarioAtual.map(questaoAtual => {
            return (
              <div className="row" key={questaoAtual.id}>
                {montarCampos(questaoAtual, form)}
              </div>
            );
          })}
        </Form>
      )}
    </Formik>
  ) : (
    ''
  );
};

MontarDadosPorSecao.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.object]),
  match: PropTypes.oneOfType([PropTypes.object]),
};

MontarDadosPorSecao.defaultProps = {
  dados: {},
  match: {},
};

export default MontarDadosPorSecao;
