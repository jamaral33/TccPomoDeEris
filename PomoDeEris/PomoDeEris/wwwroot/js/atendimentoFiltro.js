// wwwroot/js/filtroAtendimento.js

function preencherProcedimentoId() {
    const input = document.getElementById("procedimentoInput");
    const lista = document.getElementById("listaPacotes");
    const opcao = Array.from(lista.options).find(o => o.value === input.value);

    document.querySelector('input[name="ProcedimentoInputId"]').value = opcao ? opcao.dataset.id : "";
    input.setCustomValidity(opcao ? "" : "Selecione um procedimento da lista.");
}

function preencherClienteId() {
    const input = document.getElementById("ClienteInput");
    const lista = document.getElementById("listaClientes");
    const opcao = Array.from(lista.options).find(o => o.value === input.value);

    document.querySelector('input[name="clienteInputId"]').value = opcao ? opcao.dataset.id : "";
    input.setCustomValidity(opcao ? "" : "Selecione um cliente da lista.");
}

function preencherFuncionarioId() {
    const input = document.getElementById("funcionarioInput");
    const lista = document.getElementById("listaFuncionarios");
    const opcao = Array.from(lista.options).find(o => o.value === input.value);

    document.querySelector('input[name="funcionarioInputId"]').value = opcao ? opcao.dataset.id : "";
    input.setCustomValidity(opcao ? "" : "Selecione um funcionário da lista.");
}



document.getElementById("procedimentoInput").addEventListener("input", preencherProcedimentoId);
document.getElementById("ClienteInput").addEventListener("input", preencherClienteId);
document.getElementById("funcionarioInput").addEventListener("input", preencherFuncionarioId);