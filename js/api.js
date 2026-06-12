const Api = {
  async request(endpoint, options = {}) {
    const token = localStorage.getItem('token');
    const headers = {
      'Content-Type': 'application/json',
      ...options.headers
    };

    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      ...options,
      headers
    });

    if (response.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      if (!window.location.pathname.includes('login.html')) {
        window.location.href = 'login.html';
      }
      throw new Error('Session expired. Please log in again.');
    }

    const text = await response.text();
    const data = text ? JSON.parse(text) : null;

    if (!response.ok) {
      const message = data?.message || data?.title || 'Something went wrong';
      throw new Error(message);
    }

    return data;
  },

  get(endpoint) {
    return this.request(endpoint);
  },

  post(endpoint, body) {
    return this.request(endpoint, { method: 'POST', body: JSON.stringify(body) });
  },

  put(endpoint, body) {
    return this.request(endpoint, { method: 'PUT', body: JSON.stringify(body) });
  },

  patch(endpoint, body) {
    return this.request(endpoint, { method: 'PATCH', body: JSON.stringify(body) });
  },

  delete(endpoint) {
    return this.request(endpoint, { method: 'DELETE' });
  }
};
