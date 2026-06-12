function renderRestaurantCard(r) {
  return `
    <a href="restaurant.html?id=${r.id}" class="restaurant-card" style="--brand:${r.brandColor}">
      <div class="restaurant-card-img">
        <img src="${r.imageUrl}" alt="${r.name}" loading="lazy">
        <span class="restaurant-emoji">${r.logoEmoji}</span>
      </div>
      <div class="restaurant-card-body">
        <div class="restaurant-card-top">
          <h3>${r.name}</h3>
          <span class="rating">★ ${r.rating}</span>
        </div>
        <p>${r.description}</p>
        <div class="restaurant-meta">
          <span>🕐 ${r.deliveryTime}</span>
          <span>🛵 ${Utils.formatPrice(r.deliveryFee)}</span>
          <span>${r.menuCount} items</span>
        </div>
      </div>
    </a>
  `;
}

let featuredItems = [];

function renderQuickItem(item) {
  return `
    <div class="quick-item">
      <img src="${item.imageUrl}" alt="${item.name}">
      <div class="quick-item-info">
        <span class="quick-item-rest">${item.restaurantName}</span>
        <strong>${item.name}</strong>
        <span>${Utils.formatPrice(item.price)}</span>
      </div>
      <button class="quick-add-btn" data-id="${item.id}">+</button>
    </div>
  `;
}

async function loadRestaurants() {
  const grid = document.getElementById('restaurants-grid');
  try {
    const restaurants = await Api.get('/restaurants');
    grid.innerHTML = restaurants.map(renderRestaurantCard).join('');
  } catch (err) {
    grid.innerHTML = `<div class="empty-state"><h2>Could not load restaurants</h2><p>${err.message}</p></div>`;
  }
}

async function loadFeatured() {
  const container = document.getElementById('quick-picks');
  try {
    featuredItems = await Api.get('/menu/featured');
    if (featuredItems.length === 0) {
      document.getElementById('quick-section').style.display = 'none';
      return;
    }
    container.innerHTML = featuredItems.map(renderQuickItem).join('');
    bindQuickAdd(container);
  } catch {
    document.getElementById('quick-section').style.display = 'none';
  }
}

function bindQuickAdd(container) {
  container.querySelectorAll('[data-id]').forEach(btn => {
    btn.addEventListener('click', async (e) => {
      e.stopPropagation();
      const item = { ...featuredItems.find(i => i.id === parseInt(btn.dataset.id)) };
      const restaurants = await Api.get('/restaurants');
      const rest = restaurants.find(r => r.id === item.restaurantId);
      if (rest) item.deliveryFee = rest.deliveryFee;
      Cart.addItem(item);
    });
  });
}

document.addEventListener('DOMContentLoaded', () => {
  loadRestaurants();
  loadFeatured();

  const search = document.getElementById('home-search');
  if (search) {
    search.addEventListener('keydown', (e) => {
      if (e.key === 'Enter' && search.value.trim()) {
        window.location.href = `restaurant.html?search=${encodeURIComponent(search.value.trim())}`;
      }
    });
  }
});
