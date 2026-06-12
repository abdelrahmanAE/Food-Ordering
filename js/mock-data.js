const MockData = {
  restaurants: [
    {
      id: 1,
      name: "McDonald's",
      description: "World famous burgers, fries & more. I'm lovin' it!",
      imageUrl:
        "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=800&q=80",
      logoEmoji: "🍔",
      brandColor: "#FFC72C",
      deliveryTime: "25-35 min",
      deliveryFee: 20,
      minOrder: 60,
      rating: 4.6,
      menuCount: 4,
    },
    {
      id: 2,
      name: "Pizza Hut",
      description: "No one outpizzas the Hut. Fresh pizzas & sides.",
      imageUrl:
        "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=800&q=80",
      logoEmoji: "🍕",
      brandColor: "#EE3124",
      deliveryTime: "35-50 min",
      deliveryFee: 25,
      minOrder: 80,
      rating: 4.4,
      menuCount: 4,
    },
    {
      id: 3,
      name: "La Poire",
      description: "Egypt's finest patisserie. Cakes, pastries & café.",
      imageUrl:
        "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=800&q=80",
      logoEmoji: "🥐",
      brandColor: "#6B3A2A",
      deliveryTime: "30-45 min",
      deliveryFee: 30,
      minOrder: 50,
      rating: 4.8,
      menuCount: 4,
    },
    {
      id: 4,
      name: "KFC",
      description: "Finger lickin' good fried chicken & buckets.",
      imageUrl:
        "https://images.unsplash.com/photo-1626082927389-6dd097cdc6ec?w=800&q=80",
      logoEmoji: "🍗",
      brandColor: "#E4002B",
      deliveryTime: "30-40 min",
      deliveryFee: 20,
      minOrder: 70,
      rating: 4.5,
      menuCount: 4,
    },
    {
      id: 5,
      name: "Hardee's",
      description: "Thickburgers, chargrilled taste & big portions.",
      imageUrl:
        "https://images.unsplash.com/photo-1550547660-d9450f859349?w=800&q=80",
      logoEmoji: "🌟",
      brandColor: "#C8102E",
      deliveryTime: "30-45 min",
      deliveryFee: 22,
      minOrder: 65,
      rating: 4.3,
      menuCount: 4,
    },
    {
      id: 6,
      name: "Cook Door",
      description: "Egyptian grilled chicken, kofta & home-style meals.",
      imageUrl:
        "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=800&q=80",
      logoEmoji: "🔥",
      brandColor: "#D4380D",
      deliveryTime: "35-50 min",
      deliveryFee: 25,
      minOrder: 75,
      rating: 4.6,
      menuCount: 4,
    },
    {
      id: 7,
      name: "Gad",
      description: "Classic Egyptian fast food — foul, falafel & taameya.",
      imageUrl:
        "https://images.unsplash.com/photo-1601050690597-df0568f70950?w=800&q=80",
      logoEmoji: "🫘",
      brandColor: "#2E7D32",
      deliveryTime: "20-35 min",
      deliveryFee: 15,
      minOrder: 40,
      rating: 4.7,
      menuCount: 4,
    },
    {
      id: 8,
      name: "Papa John's",
      description: "Better ingredients. Better pizza. Fresh dough daily.",
      imageUrl:
        "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=800&q=80",
      logoEmoji: "🍕",
      brandColor: "#2D5A27",
      deliveryTime: "35-50 min",
      deliveryFee: 25,
      minOrder: 85,
      rating: 4.4,
      menuCount: 4,
    },
  ],

  categories: [
    { id: 1, restaurantId: 1, name: "Burgers" },
    { id: 2, restaurantId: 1, name: "Chicken" },
    { id: 3, restaurantId: 1, name: "Sides" },
    { id: 4, restaurantId: 2, name: "Pizzas" },
    { id: 5, restaurantId: 2, name: "Sides" },
    { id: 6, restaurantId: 2, name: "Desserts" },
    { id: 7, restaurantId: 3, name: "Cakes" },
    { id: 8, restaurantId: 3, name: "Pastries" },
    { id: 9, restaurantId: 3, name: "Beverages" },
    { id: 10, restaurantId: 4, name: "Buckets" },
    { id: 11, restaurantId: 4, name: "Chicken" },
    { id: 12, restaurantId: 4, name: "Sides" },
    { id: 13, restaurantId: 5, name: "Burgers" },
    { id: 14, restaurantId: 5, name: "Chicken" },
    { id: 15, restaurantId: 5, name: "Sides" },
    { id: 16, restaurantId: 6, name: "Grills" },
    { id: 17, restaurantId: 6, name: "Meals" },
    { id: 18, restaurantId: 6, name: "Sides" },
    { id: 19, restaurantId: 7, name: "Egyptian Favorites" },
    { id: 20, restaurantId: 7, name: "Sandwiches" },
    { id: 21, restaurantId: 7, name: "Drinks" },
    { id: 22, restaurantId: 8, name: "Pizzas" },
    { id: 23, restaurantId: 8, name: "Sides" },
    { id: 24, restaurantId: 8, name: "Desserts" },
  ],

  menuItems: [
    {
      id: 1,
      restaurantId: 1,
      categoryId: 1,
      name: "Big Mac Meal",
      description: "Two beef patties, special sauce, fries & drink.",
      price: 145,
      imageUrl:
        "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&q=80",
      isAvailable: true,
      isFeatured: true,
    },
    {
      id: 2,
      restaurantId: 1,
      categoryId: 1,
      name: "Double Cheeseburger",
      description: "Two beef patties, double cheese, pickles & onions.",
      price: 95,
      imageUrl:
        "https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=600&q=80",
      isAvailable: true,
      isFeatured: true,
    },
    {
      id: 3,
      restaurantId: 1,
      categoryId: 2,
      name: "McChicken",
      description: "Crispy chicken patty with lettuce & mayo.",
      price: 85,
      imageUrl:
        "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 4,
      restaurantId: 1,
      categoryId: 3,
      name: "Large Fries",
      description: "World famous golden fries.",
      price: 35,
      imageUrl:
        "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 5,
      restaurantId: 2,
      categoryId: 4,
      name: "Pepperoni Medium",
      description: "Classic pepperoni with mozzarella.",
      price: 185,
      imageUrl:
        "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=600&q=80",
      isAvailable: true,
      isFeatured: true,
    },
    {
      id: 6,
      restaurantId: 2,
      categoryId: 4,
      name: "Super Supreme",
      description: "Pepperoni, sausage, peppers, onions & mushrooms.",
      price: 220,
      imageUrl:
        "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 7,
      restaurantId: 2,
      categoryId: 5,
      name: "Garlic Bread",
      description: "Toasted bread with garlic butter.",
      price: 45,
      imageUrl:
        "https://images.unsplash.com/photo-1619535860434-ba1d8fa12536?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 8,
      restaurantId: 2,
      categoryId: 6,
      name: "Chocolate Donut Bites",
      description: "Warm chocolate donut holes.",
      price: 65,
      imageUrl:
        "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 9,
      restaurantId: 3,
      categoryId: 7,
      name: "Red Velvet Slice",
      description: "Classic red velvet with cream cheese frosting.",
      price: 55,
      imageUrl:
        "https://images.unsplash.com/photo-1586788680434-30d324eff2bd?w=600&q=80",
      isAvailable: true,
      isFeatured: true,
    },
    {
      id: 10,
      restaurantId: 3,
      categoryId: 8,
      name: "Butter Croissant",
      description: "Flaky, buttery French croissant.",
      price: 25,
      imageUrl:
        "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 11,
      restaurantId: 3,
      categoryId: 9,
      name: "Cappuccino",
      description: "Espresso with steamed milk foam.",
      price: 40,
      imageUrl:
        "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 12,
      restaurantId: 3,
      categoryId: 9,
      name: "Fresh Orange Juice",
      description: "Freshly squeezed orange juice.",
      price: 35,
      imageUrl:
        "https://images.unsplash.com/photo-1621506289937-a8e4df240d0e?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 13,
      restaurantId: 4,
      categoryId: 10,
      name: "8pc Chicken Bucket",
      description: "8 pieces original recipe chicken.",
      price: 280,
      imageUrl:
        "https://images.unsplash.com/photo-1626082927389-6dd097cdc6ec?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 14,
      restaurantId: 4,
      categoryId: 11,
      name: "Zinger Burger",
      description: "Spicy crispy chicken fillet burger.",
      price: 95,
      imageUrl:
        "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 15,
      restaurantId: 4,
      categoryId: 11,
      name: "Twister Wrap",
      description: "Crispy strips wrapped with veggies & sauce.",
      price: 85,
      imageUrl:
        "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 16,
      restaurantId: 4,
      categoryId: 12,
      name: "Cheese Fries",
      description: "Fries topped with cheese sauce.",
      price: 45,
      imageUrl:
        "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 17,
      restaurantId: 5,
      categoryId: 13,
      name: "Super Star Burger",
      description: "Two chargrilled patties with special sauce.",
      price: 135,
      imageUrl:
        "https://images.unsplash.com/photo-1550547660-d9450f859349?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 18,
      restaurantId: 5,
      categoryId: 14,
      name: "Hand-Breaded Chicken Tenders",
      description: "5pc crispy chicken tenders.",
      price: 110,
      imageUrl:
        "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 19,
      restaurantId: 5,
      categoryId: 15,
      name: "Natural Cut Fries",
      description: "Thick-cut seasoned fries.",
      price: 35,
      imageUrl:
        "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 20,
      restaurantId: 5,
      categoryId: 15,
      name: "Onion Rings",
      description: "Crispy golden onion rings.",
      price: 40,
      imageUrl:
        "https://images.unsplash.com/photo-1639024377883-141e6e9a32fe?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 21,
      restaurantId: 6,
      categoryId: 16,
      name: "Half Grilled Chicken",
      description: "Charcoal grilled half chicken with rice.",
      price: 145,
      imageUrl:
        "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 22,
      restaurantId: 6,
      categoryId: 17,
      name: "Beef Shawerma Plate",
      description: "Sliced beef shawerma with tahini & bread.",
      price: 115,
      imageUrl:
        "https://images.unsplash.com/photo-1529006557810-274dbfdd8edf?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 23,
      restaurantId: 6,
      categoryId: 18,
      name: "Rice with Vermicelli",
      description: "Egyptian-style rice.",
      price: 30,
      imageUrl:
        "https://images.unsplash.com/photo-1516684732162-798a0062be99?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 24,
      restaurantId: 6,
      categoryId: 18,
      name: "Green Salad",
      description: "Fresh mixed salad with lemon dressing.",
      price: 25,
      imageUrl:
        "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 25,
      restaurantId: 7,
      categoryId: 19,
      name: "Taameya Sandwich",
      description: "Crunchy falafel with tahini sauce.",
      price: 40,
      imageUrl:
        "https://images.unsplash.com/photo-1516684732162-798a0062be99?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 26,
      restaurantId: 7,
      categoryId: 20,
      name: "Foul Medammes",
      description: "Slow-cooked fava beans with olive oil.",
      price: 35,
      imageUrl:
        "https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 27,
      restaurantId: 7,
      categoryId: 21,
      name: "Mint Lemonade",
      description: "Refreshing mint lemonade.",
      price: 25,
      imageUrl:
        "https://images.unsplash.com/photo-1504754524776-8f4f37790ca0?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 28,
      restaurantId: 8,
      categoryId: 22,
      name: "Margherita Medium",
      description: "Tomato sauce, mozzarella & fresh basil.",
      price: 165,
      imageUrl:
        "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 29,
      restaurantId: 8,
      categoryId: 23,
      name: "Garlic Bread",
      description: "Toasted garlic bread with herbs.",
      price: 45,
      imageUrl:
        "https://images.unsplash.com/photo-1619535860434-ba1d8fa12536?w=600&q=80",
      isAvailable: true,
    },
    {
      id: 30,
      restaurantId: 8,
      categoryId: 24,
      name: "Chocolate Donut Bites",
      description: "Warm chocolate donut holes.",
      price: 65,
      imageUrl:
        "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=600&q=80",
      isAvailable: true,
    },
  ],

  featuredItems: [
    {
      id: 1,
      restaurantId: 1,
      restaurantName: "McDonald's",
      name: "Big Mac Meal",
      description: "Two beef patties, special sauce, fries & drink.",
      price: 145,
      imageUrl:
        "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&q=80",
    },
    {
      id: 5,
      restaurantId: 2,
      restaurantName: "Pizza Hut",
      name: "Pepperoni Medium",
      description: "Classic pepperoni with mozzarella.",
      price: 185,
      imageUrl:
        "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=600&q=80",
    },
    {
      id: 9,
      restaurantId: 3,
      restaurantName: "La Poire",
      name: "Red Velvet Slice",
      description: "Classic red velvet with cream cheese frosting.",
      price: 55,
      imageUrl:
        "https://images.unsplash.com/photo-1586788680434-30d324eff2bd?w=600&q=80",
    },
    {
      id: 13,
      restaurantId: 4,
      restaurantName: "KFC",
      name: "8pc Chicken Bucket",
      description: "8 pieces original recipe chicken.",
      price: 280,
      imageUrl:
        "https://images.unsplash.com/photo-1626082927389-6dd097cdc6ec?w=600&q=80",
    },
    {
      id: 21,
      restaurantId: 6,
      restaurantName: "Cook Door",
      name: "Half Grilled Chicken",
      description: "Charcoal grilled half chicken with rice.",
      price: 145,
      imageUrl:
        "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=600&q=80",
    },
    {
      id: 28,
      restaurantId: 8,
      restaurantName: "Papa John's",
      name: "Margherita Medium",
      description: "Tomato sauce, mozzarella & fresh basil.",
      price: 165,
      imageUrl:
        "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=600&q=80",
    },
  ],
};

const MockApi = {
  async request(endpoint) {
    const url = new URL(endpoint, "https://example.com");
    const path = url.pathname;
    const params = url.searchParams;

    if (path === "/restaurants" && !params.toString()) {
      return this.getRestaurants();
    }

    if (path.startsWith("/restaurants/")) {
      const id = parseInt(path.split("/").pop(), 10);
      return this.getRestaurantById(id);
    }

    if (path === "/menu/featured") {
      return this.getFeatured();
    }

    if (path === "/menu/categories") {
      const restaurantId = parseInt(params.get("restaurantId"), 10);
      return this.getMenuCategories(restaurantId);
    }

    if (path === "/menu") {
      const restaurantId = params.has("restaurantId")
        ? parseInt(params.get("restaurantId"), 10)
        : null;
      const search = params.get("search") || "";
      return this.getMenu(restaurantId, search);
    }

    return Promise.reject(
      new Error("Mock API route not implemented: " + endpoint),
    );
  },

  async getRestaurants() {
    return MockData.restaurants;
  },

  async getRestaurantById(id) {
    const restaurant = MockData.restaurants.find((r) => r.id === id);
    if (!restaurant) throw new Error("Restaurant not found");
    return restaurant;
  },

  async getMenuCategories(restaurantId) {
    return MockData.categories.filter((c) => c.restaurantId === restaurantId);
  },

  async getMenu(restaurantId, search) {
    let items = MockData.menuItems.filter((item) => item.isAvailable);
    if (restaurantId) {
      items = items.filter((item) => item.restaurantId === restaurantId);
    }
    if (search) {
      const normalized = search.toLowerCase();
      items = items.filter(
        (item) =>
          item.name.toLowerCase().includes(normalized) ||
          item.description.toLowerCase().includes(normalized),
      );
    }
    return items;
  },

  async getFeatured() {
    return MockData.featuredItems;
  },
};
