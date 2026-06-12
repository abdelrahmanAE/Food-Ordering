const Payment = {
  method: 'CashOnDelivery',
  vodafoneSession: null,
  fawrySession: null,
  demoInfo: null,

  async loadDemoInfo() {
    this.demoInfo = await Api.get('/payments/demo-info');
    return this.demoInfo;
  },

  getCheckoutDraft() {
    const raw = sessionStorage.getItem('checkoutDraft');
    if (!raw) return null;
    return JSON.parse(raw);
  },

  saveCheckoutDraft(draft) {
    sessionStorage.setItem('checkoutDraft', JSON.stringify(draft));
  },

  formatCardNumber(value) {
    const digits = value.replace(/\D/g, '').slice(0, 16);
    return digits.replace(/(.{4})/g, '$1 ').trim();
  },

  formatExpiry(value) {
    const digits = value.replace(/\D/g, '').slice(0, 4);
    if (digits.length >= 3) return `${digits.slice(0, 2)}/${digits.slice(2)}`;
    return digits;
  },

  async requestVodafoneOtp(phone, amount) {
    return Api.post('/payments/vodafone/request-otp', { phone, amount });
  },

  async initiateFawry(amount) {
    return Api.post('/payments/fawry/initiate', { amount });
  },

  async simulateFawryPay(sessionId) {
    return Api.post(`/payments/fawry/simulate-pay/${sessionId}`, {});
  },

  async checkout(payload) {
    return Api.post('/payments/checkout', payload);
  },

  showProcessing(title, subtitle) {
    const overlay = document.getElementById('payment-overlay');
    if (!overlay) return;
    overlay.querySelector('.processing-title').textContent = title;
    overlay.querySelector('.processing-sub').textContent = subtitle || '';
    overlay.classList.add('active');
  },

  hideProcessing() {
    document.getElementById('payment-overlay')?.classList.remove('active');
  }
};
