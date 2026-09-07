document.addEventListener('DOMContentLoaded', () => {

  /* ---------- ESTADO DO CARRINHO ---------- */
  const CART_KEY = 'pomoDeEris.cart';
  let cart = loadCart();

  function loadCart() {
    try {
      const raw = sessionStorage.getItem(CART_KEY);
      return raw ? JSON.parse(raw) : [];
    } catch (e) {
      return [];
    }
  }

  function saveCart() {
    try {
      sessionStorage.setItem(CART_KEY, JSON.stringify(cart));
    } catch (e) { /* ignora se storage indisponível */ }
  }

  /* ---------- TOAST ---------- */
  const toast = document.getElementById('toast');
  function showToast(message, duration = 2200) {
    toast.textContent = message;
    toast.classList.add('show');
    clearTimeout(showToast._t);
    showToast._t = setTimeout(() => toast.classList.remove('show'), duration);
  }

  /* ---------- RENDERIZAÇÃO DOS TRATAMENTOS ---------- */
  const grid = document.getElementById('treatment-grid');
  const filterButtons = document.querySelectorAll('.filter-btn');
  let currentFilter = 'todos';

  function formatPrice(value) {
    return 'R$ ' + value.toLocaleString('pt-BR');
  }

  function renderTreatments() {
    const list = currentFilter === 'todos'
      ? TREATMENTS
      : TREATMENTS.filter(t => t.tags.includes(currentFilter));

    grid.innerHTML = list.map(t => `
      <article class="treatment-card" data-id="${t.id}">
        <div class="treatment-card__image-wrap">
          <img src="${t.image}" alt="${t.name}">
          ${t.badge ? `<span class="treatment-card__badge">${t.badge}</span>` : ''}
        </div>
        <div class="treatment-card__body">
          <h3 class="treatment-card__name">${t.name}</h3>
          <p class="treatment-card__desc">${t.description}</p>
          <span class="treatment-card__duration">⏱ ${t.duration} min</span>
          <div class="treatment-card__footer">
            <div class="treatment-card__price-wrap">
              <span class="treatment-card__price-label">${t.oldPrice ? '' : 'A partir de'}</span>
              <span class="treatment-card__price">
                ${t.oldPrice ? `<span class="treatment-card__old-price">${formatPrice(t.oldPrice)}</span>` : ''}${formatPrice(t.price)}
              </span>
            </div>
            <button class="add-btn" data-id="${t.id}">+ Agendar</button>
          </div>
        </div>
      </article>
    `).join('');

    grid.querySelectorAll('.add-btn').forEach(btn => {
      btn.addEventListener('click', () => addToCart(btn.dataset.id, btn));
    });
  }

  filterButtons.forEach(btn => {
    btn.addEventListener('click', () => {
      filterButtons.forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      currentFilter = btn.dataset.filter;
      renderTreatments();
    });
  });

  /* ---------- CARRINHO LATERAL ---------- */
  const cartPanel = document.getElementById('cart-panel');
  const cartOverlay = document.getElementById('cart-overlay');
  const cartToggle = document.getElementById('cart-toggle');
  const cartClose = document.getElementById('cart-close');
  const cartCount = document.getElementById('cart-count');
  const cartEmpty = document.getElementById('cart-empty');
  const cartItemsEl = document.getElementById('cart-items');
  const cartFooter = document.getElementById('cart-footer');
  const cartTotalValue = document.getElementById('cart-total-value');

  function openCart() {
    cartPanel.classList.add('open');
    cartOverlay.classList.add('show');
  }
  function closeCart() {
    cartPanel.classList.remove('open');
    cartOverlay.classList.remove('show');
  }

  cartToggle.addEventListener('click', openCart);
  cartClose.addEventListener('click', closeCart);
  cartOverlay.addEventListener('click', closeCart);

  function addToCart(id, btnEl) {
    const treatment = TREATMENTS.find(t => t.id === id);
    if (!treatment) return;

    cart.push(treatment);
    saveCart();
    renderCart();
    showToast(`"${treatment.name}" adicionado ao agendamento.`);

    if (btnEl) {
      btnEl.classList.add('added');
      btnEl.textContent = '✓ Adicionado';
      setTimeout(() => {
        btnEl.classList.remove('added');
        btnEl.textContent = '+ Agendar';
      }, 1400);
    }

    openCart();
  }

  function removeFromCart(index) {
    cart.splice(index, 1);
    saveCart();
    renderCart();
  }

  function renderCart() {
    cartCount.textContent = cart.length;

    if (cart.length === 0) {
      cartEmpty.style.display = 'block';
      cartItemsEl.style.display = 'none';
      cartFooter.hidden = true;
      return;
    }

    cartEmpty.style.display = 'none';
    cartItemsEl.style.display = 'block';
    cartFooter.hidden = false;

    cartItemsEl.innerHTML = cart.map((item, index) => `
      <li class="cart-item">
        <img src="${item.image}" alt="" class="cart-item__thumb">
        <div class="cart-item__info">
          <p class="cart-item__name">${item.name}</p>
          <span class="cart-item__price">${formatPrice(item.price)}</span>
        </div>
        <button class="cart-item__remove" data-index="${index}" aria-label="Remover">✕</button>
      </li>
    `).join('');

    cartItemsEl.querySelectorAll('.cart-item__remove').forEach(btn => {
      btn.addEventListener('click', () => removeFromCart(Number(btn.dataset.index)));
    });

    const total = cart.reduce((sum, item) => sum + item.price, 0);
    cartTotalValue.textContent = formatPrice(total);
  }

  /* ---------- CONTADOR REGRESSIVO DA PROMOÇÃO ---------- */
  function startCountdown() {
    // Data alvo: 67 dias, 2h, 6min, 48s a partir de agora (valor ilustrativo do protótipo).
    const target = new Date();
    target.setDate(target.getDate() + 67);
    target.setHours(target.getHours() + 2);
    target.setMinutes(target.getMinutes() + 6);
    target.setSeconds(target.getSeconds() + 48);

    const daysEl = document.getElementById('cd-days');
    const hoursEl = document.getElementById('cd-hours');
    const minEl = document.getElementById('cd-min');
    const secEl = document.getElementById('cd-sec');

    function pad(n) { return String(n).padStart(2, '0'); }

    function tick() {
      const diff = target - new Date();
      if (diff <= 0) {
        daysEl.textContent = hoursEl.textContent = minEl.textContent = secEl.textContent = '00';
        return;
      }
      const days = Math.floor(diff / (1000 * 60 * 60 * 24));
      const hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
      const minutes = Math.floor((diff / (1000 * 60)) % 60);
      const seconds = Math.floor((diff / 1000) % 60);

      daysEl.textContent = pad(days);
      hoursEl.textContent = pad(hours);
      minEl.textContent = pad(minutes);
      secEl.textContent = pad(seconds);
    }

    tick();
    setInterval(tick, 1000);
  }

  /* ---------- BUSCA NO HERO ---------- */
  document.getElementById('hero-search').addEventListener('submit', (event) => {
    event.preventDefault();
    document.getElementById('tratamentos').scrollIntoView({ behavior: 'smooth' });
  });

  document.getElementById('search-toggle').addEventListener('click', () => {
    document.querySelector('.hero__search input').focus();
    document.getElementById('tratamentos').scrollIntoView({ behavior: 'smooth' });
  });

  /* ---------- INICIALIZAÇÃO ---------- */
  renderTreatments();
  renderCart();
  startCountdown();
});
