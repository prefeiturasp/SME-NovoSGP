import React, { useState, useEffect, useRef, memo } from 'react';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';

import { ModalConteudoHtml, SelectComponent } from '~/componentes';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { sucesso, erros } from '~/servicos/alertas';

const CopiarConteudo = ({
  visivel,
  turmaId,
  componenteCurricularEolId,
  onCloseCopiarConteudo,
  listaBimestresPreenchidos,
  planoAnual,
  onChangePossuiTurmasDisponiveisParaCopia,
}) => {
  const [turmasSelecionadas, setTurmasSelecionadas] = useState([]);
  const [bimestresSelecionados, setBimestresSelecionados] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);
  const [loader, setLoader] = useState(false);
  const [confirmacaoTurmasComPlano, setConfirmacaoTurmasComPlano] = useState(
    ''
  );
  const refForm = useRef();

  useEffect(() => {
    if (componenteCurricularEolId && turmaId) {
      servicoPlanoAnual
        .obterTurmasParaCopia(turmaId, componenteCurricularEolId)
        .then(c => {
          onChangePossuiTurmasDisponiveisParaCopia(
            !!(c.data && c.data.length > 0)
          );
          setListaTurmas(c.data);
        })
        .catch(e => {
          erros(e);
        });
    }
  }, [componenteCurricularEolId, turmaId]);

  const fecharCopiarConteudo = () => {
    setTurmasSelecionadas([]);
    setBimestresSelecionados([]);
    onCloseCopiarConteudo();
    setConfirmacaoTurmasComPlano('');
    refForm.current.handleReset();
  };

  const copiar = async () => {
    const plano = {
      ...planoAnual,
      bimestres: planoAnual.bimestres.filter(c =>
        bimestresSelecionados.includes(c.bimestre.toString())
      ),
    };

    setLoader(true);
    servicoPlanoAnual
      .copiarConteudo({
        planoAnual: plano,
        idsTurmasDestino: turmasSelecionadas,
        bimestresDestino: bimestresSelecionados,
      })
      .then(() => {
        sucesso('Planejamento copiado com sucesso.');
        fecharCopiarConteudo();
      })
      .catch(e => {
        erros(e);
        onCloseCopiarConteudo();
      })
      .finally(() => {
        setLoader(false);
      });
  };

  const onChangeTurmasSelecionadas = turmas => {
    setTurmasSelecionadas(turmas);
    const turmasComPlano = listaTurmas.filter(
      c => turmas.includes(c.codTurma.toString()) && c.possuiPlano
    );
    if (turmasComPlano && turmasComPlano.length > 0) {
      setConfirmacaoTurmasComPlano(
        `As turmas: ${turmasComPlano
          .map(c => c.nomeTurma)
          .join(
            ','
          )} já possuem plano anual que serão sobrescritos ao realizar a cópia. Deseja continuar?`
      );
    } else {
      setConfirmacaoTurmasComPlano('');
    }
  };

  const validacoes = Yup.object({
    turmas: Yup.string().required('Selecione ao menos uma turma.'),
    bimestres: Yup.string().required('Selecione ao menos um bimestre.'),
  });

  return (
    <Formik
      enableReinitialize
      initialValues={{
        turmas: [],
        bimestres: [],
      }}
      validationSchema={validacoes}
      onSubmit={values => copiar(values)}
      validateOnChange
      validateOnBlur
      ref={refForm}
    >
      {form => (
        <Form>
          <ModalConteudoHtml
            key="copiarConteudo"
            visivel={visivel}
            onConfirmacaoPrincipal={e => {
              form.validateForm().then(() => {
                form.handleSubmit(e);
              });
            }}
            onConfirmacaoSecundaria={fecharCopiarConteudo}
            onClose={fecharCopiarConteudo}
            labelBotaoPrincipal="Copiar"
            tituloAtencao={confirmacaoTurmasComPlano && 'Atenção'}
            perguntaAtencao={confirmacaoTurmasComPlano}
            labelBotaoSecundario="Cancelar"
            titulo="Copiar Conteúdo"
            closable={false}
            loader={loader}
            desabilitarBotaoPrincipal={false}
          >
            <div>
              <label>Copiar para a(s) turma(s)</label>
              <SelectComponent
                id="turmas"
                name="turmas"
                lista={listaTurmas}
                valueOption="codTurma"
                valueText="nomeTurma"
                valueSelect={turmasSelecionadas}
                multiple
                placeholder="Selecione uma ou mais turmas"
                onChange={onChangeTurmasSelecionadas}
                form={form}
              />
              <label className="mt-3">Copiar para o(s) bimestre(s)</label>
              <SelectComponent
                id="bimestres"
                name="bimestres"
                lista={listaBimestresPreenchidos}
                valueOption="valor"
                valueText="nome"
                valueSelect={bimestresSelecionados}
                multiple
                placeholder="Selecione um ou mais bimestres"
                onChange={turmas => setBimestresSelecionados(turmas)}
                form={form}
              />
            </div>
          </ModalConteudoHtml>
        </Form>
      )}
    </Formik>
  );
};
export default memo(CopiarConteudo);
