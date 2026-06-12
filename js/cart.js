const Cart = {
  getItems() {
    return JSON.parse(localStorage.getItem('cart') || '[]');
  },

  saveItems(items) {
    localStorage.setItem('cart', JSON.stringify(items));
    this.updateStickyBar();
    if (typeof Auth !== 'undefined') Auth.updateNav();
  },

  getRestaurant() {
    const items = this.getItems();
    if (items.length === 0) return null;
    return {
      id: items[0].restaurantId,
      name: items[0].restaurantName,
      deliveryFee: items[0].deliveryFee || 25
    };
  },

  canAdd(menuItem) {
    const restaurant = this.getRestaurant();
    if (!restaurant) return true;
    if (restaurant.id !== menuItem.restaurantId) {
      const swap = confirm(
        `Your cart has items from ${restaurant.name}. Clear cart and add from ${menuItem.restaurantName}?`
      );
      if (swap) this.clear();
      return swap;
    }
    return true;
  },

  addItem(menuItem) {
    if (!this.canAdd(menuItem)) return;

    const items = this.getItems();
    const existing = items.find(i => i.id === menuItem.id);

    if (existing) {
      existing.quantity += 1;
    } else {
      items.push({
        id: menuItem.id,
        name: menuItem.name,
        price: menuItem.price,
        imageUrl: menuItem.imageUrl,
        restaurantId: menuItem.restaurantId,
        restaurantName: menuItem.restaurantName,
        deliveryFee: menuItem.deliveryFee,
        quantity: 1
      });
    }

    this.saveItems(items);
    this.showToast(`+ ${menuItem.name}`);
  },

  updateQuantity(id, quantity) {
    let items = this.getItems();
    if (quantity <= 0) {
      items = items.filter(i => i.id !== id);
    } else {
      const item = items.find(i => i.id === id);
      if (item) item.quantity = quantity;
    }
    this.saveItems(items);
  },

  removeItem(id) {
    this.saveItems(this.getItems().filter(i => i.id !== id));
  },

  clear() {
    localStorage.removeItem('cart');
    this.updateStickyBar();
    if (typeof Auth !== 'undefined') Auth.updateNav();
  },

  getSubtotal() {
    return this.getItems().reduce((sum, item) => sum + item.price * item.quantity, 0);
  },

  getDeliveryFee() {
    const restaurant = this.getRestaurant();
    return restaurant ? (restaurant.deliveryFee || 25) : 0;
  },

  getTotal() {
    return this.getSubtotal() + this.getDeliveryFee();
  },

  getItemCount() {
    return this.getItems().reduce((sum, item) => sum + item.quantity, 0);
  },

  updateStickyBar() {
    const bar = document.getElementById('sticky-cart');
    if (!bar) return;

    const count = this.getItemCount();
    if (count === 0) {
      bar.classList.remove('visible');
      return;
    }

    bar.classList.add('visible');
    const countEl = bar.querySelector('.sticky-count');
    const totalEl = bar.querySelector('.sticky-total');
    const restEl = bar.querySelector('.sticky-restaurant');
    const restaurant = this.getRestaurant();

    if (countEl) countEl.textContent = count;
    if (totalEl) totalEl.textContent = Utils.formatPrice(this.getTotal());
    if (restEl && restaurant) restEl.textContent = restaurant.name;
  },

  showToast(message) {
    let toast = document.getElementById('toast');
    if (!toast) {
      toast = document.createElement('div');
      toast.id = 'toast';
      toast.className = 'toast';
      document.body.appendChild(toast);
    }
    toast.textContent = message;
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 1800);
  },

  injectStickyBar() {
    if (document.getElementById('sticky-cart')) return;

    const bar = document.createElement('a');
    bar.id = 'sticky-cart';
    bar.href = 'cart.html';
    bar.className = 'sticky-cart';
    bar.innerHTML = `
      <div class="sticky-cart-left">
        <span class="sticky-count">0</span> items · <span class="sticky-restaurant"></span>
      </div>
      <div class="sticky-cart-right">
        View cart <span class="sticky-total">0 EGP</span> →
      </div>
    `;
    document.body.appendChild(bar);
    this.updateStickyBar();
  }
};

document.addEventListener('DOMContentLoaded', () => {
  Cart.injectStickyBar();
});
