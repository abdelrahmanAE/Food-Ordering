let restaurant = null;
let allItems = [];
let activeCategory = null;

function renderMenuRow(item) {
  const qty = Cart.getItems().find(i => i.id === item.id)?.quantity || 0;
  return `
    <div class="menu-row" data-id="${item.id}">
      <img class="menu-row-img" src="${item.imageUrl}" alt="${item.name}" loading="lazy">
      <div class="menu-row-info">
        <h3>${item.name}</h3>
        <p>${item.description}</p>
        <span class="menu-row-price">${Utils.formatPrice(item.price)}</span>
      </div>
      <div class="menu-row-action">
        ${qty > 0 ? `
          <div class="inline-qty">
            <button class="qty-btn-sm" data-qty="dec" data-id="${item.id}">−</button>
            <span>${qty}</span>
            <button class="qty-btn-sm" data-qty="inc" data-id="${item.id}">+</button>
          </div>
        ` : `
          <button class="add-btn-round" data-add="${item.id}">+</button>
        `}
      </div>
    </div>
  `;
}

function bindMenuActions(container) {
  container.querySelectorAll('[data-add]').forEach(btn => {
    btn.addEventListener('click', () => {
      const item = allItems.find(i => i.id === parseInt(btn.dataset.add));
      if (item) {
        item.deliveryFee = restaurant.deliveryFee;
        Cart.addItem(item);
        renderMenu(allItems);
      }
    });
  });

  container.querySelectorAll('[data-qty]').forEach(btn => {
    btn.addEventListener('click', () => {
      const id = parseInt(btn.dataset.id);
      const item = Cart.getItems().find(i => i.id === id);
      if (!item) return;
      const qty = btn.dataset.qty === 'inc' ? item.quantity + 1 : item.quantity - 1;
      Cart.updateQuantity(id, qty);
      renderMenu(allItems);
    });
  });
}

function renderMenu(items) {
  const list = document.getElementById('menu-list');
  if (items.length === 0) {
    list.innerHTML = '<div class="empty-state"><h2>Nothing here</h2><p>Try another category.</p></div>';
    return;
  }
  list.innerHTML = items.map(renderMenuRow).join('');
  bindMenuActions(list);
}

async function loadRestaurant(id) {
  restaurant = await Api.get(`/restaurants/${id}`);
  document.title = `${restaurant.name} — Bites`;

  const header = document.getElementById('restaurant-header');
  header.style.setProperty('--brand', restaurant.brandColor);
  header.innerHTML = `
    <a href="index.html" class="back-link">← Back</a>
    <div class="restaurant-hero">
      <img src="${restaurant.imageUrl}" alt="${restaurant.name}">
      <div class="restaurant-hero-overlay">
        <span class="restaurant-hero-emoji">${restaurant.logoEmoji}</span>
        <h1>${restaurant.name}</h1>
        <div class="restaurant-hero-meta">
          <span>★ ${restaurant.rating}</span>
          <span>🕐 ${restaurant.deliveryTime}</span>
          <span>Min ${Utils.formatPrice(restaurant.minOrder)}</span>
        </div>
      </div>
    </div>
  `;
}

async function loadCategories(restaurantId) {
  const categories = await Api.get(`/menu/categories?restaurantId=${restaurantId}`);
  const container = document.getElementById('category-filters');

  container.innerHTML = `
    <button class="filter-pill active" data-cat="">All</button>
    ${categories.map(c => `<button class="filter-pill" data-cat="${c.id}">${c.name}</button>`).join('')}
  `;

  container.querySelectorAll('.filter-pill').forEach(btn => {
    btn.addEventListener('click', () => {
      container.querySelectorAll('.filter-pill').forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      activeCategory = btn.dataset.cat ? parseInt(btn.dataset.cat) : null;
      filterAndRender();
    });
  });
}

async function loadMenu(restaurantId) {
  const list = document.getElementById('menu-list');
  list.innerHTML = '<div class="loading"><div class="spinner"></div></div>';

  try {
    allItems = await Api.get(`/menu?restaurantId=${restaurantId}`);
    filterAndRender();
  } catch (err) {
    list.innerHTML = `<div class="empty-state"><h2>Error</h2><p>${err.message}</p></div>`;
  }
}

function filterAndRender() {
  const search = document.getElementById('menu-search')?.value.trim().toLowerCase() || '';
  let items = [...allItems];

  if (activeCategory) items = items.filter(i => i.categoryId === activeCategory);
  if (search) items = items.filter(i =>
    i.name.toLowerCase().includes(search) ||
    i.description.toLowerCase().includes(search)
  );

  renderMenu(items);
}

document.addEventListener('DOMContentLoaded', async () => {
  const id = Utils.getQueryParam('id');
  const globalSearch = Utils.getQueryParam('search');

  if (!id && !globalSearch) {
    window.location.href = 'index.html';
    return;
  }

  if (globalSearch && !id) {
    document.getElementById('restaurant-header').innerHTML = `
      <a href="index.html" class="back-link">← Back</a>
      <h1 style="padding:16px 0">Search: "${globalSearch}"</h1>
    `;
    allItems = await Api.get(`/menu?search=${encodeURIComponent(globalSearch)}`);
    document.getElementById('category-filters').style.display = 'none';
    renderMenu(allItems);
    return;
  }

  await loadRestaurant(id);
  await loadCategories(id);
  await loadMenu(id);

  const searchInput = document.getElementById('menu-search');
  if (searchInput) {
    let timeout;
    searchInput.addEventListener('input', () => {
      clearTimeout(timeout);
      timeout = setTimeout(filterAndRender, 250);
    });
  }
});
