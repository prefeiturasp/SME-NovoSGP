/* eslint-disable object-shorthand */
module.exports = {
  prompter: (cz, commit) => {
    function executeCommit(answers) {
      // parentheses are only needed when a scope is present
      let tipo = answers.tipo.trim();
      let estoria = answers.estoria.trim();

      estoria = `${estoria ? `[AB#${estoria}]` : ''}`;
      tipo = tipo ? `(${tipo})` : '';

      // Hard limit this line
      const head = `${tipo} ${estoria}: ${answers.subject.trim()}`;

      commit(head);
    }

    const promise = cz.prompt(
      [
        {
          type: 'rawlist',
          name: 'tipo',
          message: 'Selecione o tipo de commit:',
          choices: [
            {
              name: 'feat:     Uma nova funcionalidade',
              value: 'feat',
            },
            {
              name: 'fix:      Correçao de bug',
              value: 'fix',
            },
            {
              name: 'docs:     Documentaçao',
              value: 'docs',
            },
            {
              name: 'style:    Comportamento/Visual',
              value: 'style',
            },
            {
              name: 'refactor: Refatoraçao de código',
              value: 'refactor',
            },
            {
              name: 'perf:     Refatoraçao visando performance',
              value: 'perf',
            },
          ],
        },
        {
          type: 'input',
          name: 'estoria',
          message: 'Numero da estória:\n',
          validate(input) {
            const digitado = input || input.trim();
            if (!digitado.length) {
              return true;
            }
            if (/^[0-9]+$/g.test(input)) {
              return true;
            }
            return 'Digite somente os numeros';
          },
        },
        {
          type: 'input',
          name: 'subject',
          message: 'Digite um resumo deste commit:\n',
          validate(input) {
            if (input && input.length > 0) {
              return true;
            }
            return 'Voce precisa digitar uma descriçao';
          },
        },
      ],
      executeCommit
    );

    // when using commitizen 2.7.3+, the prompt method return a promise
    if (promise) promise.then(executeCommit);
  },
};
