const button = document.getElementById("adicionarServico")

button.addEventListener("click", adicionarServico);

function adicionarServico() {

    const container = document.getElementById("listaServicosSelecionados");

    const novoCampo = document.createElement("div");

    novoCampo.classList.add("field", "servico-item");

    novoCampo.innerHTML = `
        <input type="text"
               name="Servicos"
               list="listaServicos"
               placeholder="Digite para pesquisar..."
               name="Servicos"
               required>

        <button type="button"
                class="btn btn-ghost btn-sm remover-servico"
                onclick="removerServico(this)">
            Remover
        </button>
    `;

    container.appendChild(novoCampo);
}

function removerServico(botao) {

    const container = document.getElementById("listaServicosSelecionados");

    if (container.children.length > 1) {
        botao.parentElement.remove();
    }
}