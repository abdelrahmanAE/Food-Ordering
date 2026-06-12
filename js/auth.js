const Auth = {
  saveSession(data) {
    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify({
      fullName: data.fullName,
      email: data.email,
      role: data.role
    }));
  },

  getUser() {
    const raw = localStorage.getItem('user');
    return raw ? JSON.parse(raw) : null;
  },

  isLoggedIn() {
    return !!localStorage.getItem('token');
  },

  isAdmin() {
    const user = this.getUser();
    return user?.role === 'Admin';
  },

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = 'index.html';
  },

  requireAuth(redirectTo = 'login.html') {
    if (!this.isLoggedIn()) {
      window.location.href = redirectTo;
      return false;
    }
    return true;
  },

  requireAdmin() {
    if (!this.requireAuth()) return false;
    if (!this.isAdmin()) {
      window.location.href = 'index.html';
      return false;
    }
    return true;
  },

  updateNav() {
    const user = this.getUser();
    const authLinks = document.getElementById('auth-links');
    const userMenu = document.getElementById('user-menu');
    const userName = document.getElementById('user-name');
    const adminLink = document.getElementById('admin-link');
    const cartCount = document.getElementById('cart-count');

    if (cartCount) {
      const total = typeof Cart !== 'undefined'
        ? Cart.getItemCount()
        : JSON.parse(localStorage.getItem('cart') || '[]').reduce((s, i) => s + i.quantity, 0);
      cartCount.textContent = total;
      cartCount.style.display = total > 0 ? 'inline-flex' : 'none';
    }

    if (!authLinks || !userMenu) return;

    if (user) {
      authLinks.style.display = 'none';
      userMenu.style.display = 'flex';
      if (userName) userName.textContent = user.fullName.split(' ')[0];
      if (adminLink) adminLink.style.display = user.role === 'Admin' ? 'inline' : 'none';
    } else {
      authLinks.style.display = 'flex';
      userMenu.style.display = 'none';
    }
  }
};

document.addEventListener('DOMContentLoaded', () => {
  Auth.updateNav();

  const logoutBtn = document.getElementById('logout-btn');
  if (logoutBtn) {
    logoutBtn.addEventListener('click', (e) => {
      e.preventDefault();
      Auth.logout();
    });
  }
});
