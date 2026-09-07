document.addEventListener('DOMContentLoaded', () => {

  const CART_KEY = 'pomoDeEris.cart';

  // Itens de exemplo, usados apenas se o carrinho estiver vazio
  // (por exemplo, ao abrir esta página diretamente, sem vir da home).
  const SAMPLE_CART = [
    {
      id: 'massagem-relaxante',
      name: 'Pacote Massagem Relaxante',
      duration: 60,
      price: 280,
      oldPrice: 350,
      image: '../home/massagem-relaxante.jpg'
    },
    {
      id: 'pacote-manicure',
      name: 'Procedimento, Pacote manicure',
      duration: 30,
      price: 890,
      oldPrice: null,
      image: '../home/pacote-manicure.jpg'
    },
    {
      id: 'pelos-faciais',
      name: 'Procedimentos de pelos faciais',
      duration: 40,
      price: 1200,
      oldPrice: null,
      image: '../home/pelos-faciais.jpg'
    }
  ];

  // Desconto fixo aplicado sobre o subtotal, para fins de demonstração.
  const FLAT_DISCOUNT = 150;

  function loadCart() {
    try {
      const raw = sessionStorage.getItem(CART_KEY);
      if (raw) {
        const parsed = JSON.parse(raw);
        if (Array.isArray(parsed) && parsed.length > 0) return parsed;
      }
    } catch (e) { /* segue para o exemplo */ }
    return SAMPLE_CART.slice();
  }

  function saveCart() {
    try {
      sessionStorage.setItem(CART_KEY, JSON.stringify(cart));
    } catch (e) { /* ignora se storage indisponível */ }
  }

  let cart = loadCart();

  const listEl = document.getElementById('cart-list');
  const emptyStateEl = document.getElementById('cart-empty-state');
  const summaryCount = document.getElementById('summary-count');
  const summarySubtotal = document.getElementById('summary-subtotal');
  const summaryDiscount = document.getElementById('summary-discount');
  const summaryDuration = document.getElementById('summary-duration');
  const summaryTotal = document.getElementById('summary-total');
  const confirmBtn = document.getElementById('confirm-btn');
  const toast = document.getElementById('toast');

  function formatPrice(value) {
    return 'R$ ' + value.toLocaleString('pt-BR');
  }

  function showToast(message, duration = 2400) {
    toast.textContent = message;
    toast.classList.add('show');
    clearTimeout(showToast._t);
    showToast._t = setTimeout(() => toast.classList.remove('show'), duration);
  }

  function render() {
    if (cart.length === 0) {
      listEl.innerHTML = '';
      emptyStateEl.hidden = false;
      confirmBtn.disabled = true;
    } else {
      emptyStateEl.hidden = true;
      confirmBtn.disabled = false;

      listEl.innerHTML = cart.map((item, index) => `
        <li class="cart-list-item" data-index="${index}">
          <img src="${item.image}" alt="" class="cart-list-item__thumb">
          <div class="cart-list-item__info">
            <p class="cart-list-item__name">${item.name}</p>
            <span class="cart-list-item__duration">${item.duration} min</span>
          </div>
          <div class="cart-list-item__price">
            ${item.oldPrice ? `<span class="cart-list-item__old-price">${formatPrice(item.oldPrice)}</span>` : ''}
            ${formatPrice(item.price)}
          </div>
          <button class="cart-list-item__remove" data-index="${index}" aria-label="Remover item">✕</button>
        </li>
      `).join('');

      listEl.querySelectorAll('.cart-list-item__remove').forEach(btn => {
        btn.addEventListener('click', () => removeItem(Number(btn.dataset.index)));
      });
    }

    const subtotal = cart.reduce((sum, item) => sum + (item.oldPrice || item.price), 0);
    const total = cart.reduce((sum, item) => sum + item.price, 0);
    const discount = Math.max(subtotal - total, 0) + (cart.length ? 0 : 0);
    const duration = cart.reduce((sum, item) => sum + item.duration, 0);

    summaryCount.textContent = `${cart.length} tratamento${cart.length === 1 ? '' : 's'}`;
    summarySubtotal.textContent = formatPrice(subtotal);
    summaryDiscount.textContent = `− ${formatPrice(discount)}`;
    summaryDuration.textContent = `${duration} min`;
    summaryTotal.textContent = formatPrice(total);
  }

  function removeItem(index) {
    const removed = cart[index];
    cart.splice(index, 1);
    saveCart();
    render();
    if (removed) showToast(`"${removed.name}" removido do carrinho.`);
  }

  confirmBtn.addEventListener('click', () => {
    if (cart.length === 0) return;
    confirmBtn.disabled = true;
    confirmBtn.textContent = 'Confirmando...';

    setTimeout(() => {
      showToast('Agendamento confirmado com sucesso!');
      cart = [];
      saveCart();
      render();
      confirmBtn.textContent = 'Confirmar agendamento';
    }, 1000);
  });

  render();
});
