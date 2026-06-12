const Utils = {
  formatPrice(amount) {
    return `${Number(amount).toFixed(0)} EGP`;
  },

  getQueryParam(name) {
    return new URLSearchParams(window.location.search).get(name);
  },

  paymentLabel(method) {
    const labels = {
      CashOnDelivery: 'Cash on Delivery',
      Visa: 'Visa / Mastercard',
      Fawry: 'Fawry',
      VodafoneCash: 'Vodafone Cash'
    };
    return labels[method] || method;
  }
};
