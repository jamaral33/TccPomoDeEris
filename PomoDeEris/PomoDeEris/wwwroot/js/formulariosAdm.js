const botaoAdicionar = document.getElementById("adicionarServico");
const container = document.getElementById("listaServicosSelecionados");
const datalist = document.getElementById("listaServicos");

botaoAdicionar.addEventListener("click", adicionarServico);

// Resolve o nome digitado -> IdServico e grava no hidden
container.addEventListener("input", function (e) {
    if (e.target.classList.contains("servico-input")) {
        resolverId(e.target);
    }
});

function resolverId(input) {
    const hidden = input.parentElement.querySelector(".servico-id");

    const opcao = Array.from(datalist.options)
        .find(o => o.value === input.value);

    hidden.value = opcao ? opcao.dataset.id : "";

    // Impede enviar um texto que não existe na lista
    input.setCustomValidity(opcao ? "" : "Selecione um serviço da lista.");
}

function adicionarServico() {
    const novoCampo = document.createElement("div");
    novoCampo.classList.add("field", "servico-item");

    novoCampo.innerHTML = `
        <input type="text"
               class="servico-input"
               list="listaServicos"
               placeholder="Digite para pesquisar..."
               required>

        <input type="hidden" name="ServicosIds" class="servico-id">

        <button type="button"
                class="btn btn-ghost btn-sm remover-servico"
                onclick="removerServico(this)">
            Remover
        </button>
    `;

    container.appendChild(novoCampo);
}

function removerServico(botao) {
    if (container.children.length > 1) {
        botao.closest(".servico-item").remove();
    }
}