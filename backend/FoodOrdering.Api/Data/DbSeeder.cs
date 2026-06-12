using FoodOrdering.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrdering.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User
                {
                    FullName = "Admin User",
                    Email = "admin@bites.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin"
                },
                new User
                {
                    FullName = "Demo Customer",
                    Email = "demo@bites.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo1234!"),
                    Role = "Customer"
                });
            await db.SaveChangesAsync();
        }

        await SeedRestaurantAsync(db, CreateMcDonalds(), SeedMcDonalds);
        await SeedRestaurantAsync(db, CreatePizzaHut(), SeedPizzaHut);
        await SeedRestaurantAsync(db, CreateLaPoire(), SeedLaPoire);
        await SeedRestaurantAsync(db, CreateKfc(), SeedKfc);
        await SeedRestaurantAsync(db, CreateHardees(), SeedHardees);
        await SeedRestaurantAsync(db, CreateCookDoor(), SeedCookDoor);
        await SeedRestaurantAsync(db, CreateGad(), SeedGad);
        await SeedRestaurantAsync(db, CreatePapaJohns(), SeedPapaJohns);
        await SeedRestaurantAsync(db, CreateShawermaElReem(), SeedShawermaElReem);
        await SeedRestaurantAsync(db, CreateCinnabon(), SeedCinnabon);
        await SeedRestaurantAsync(db, CreateCostaCoffee(), SeedCostaCoffee);
        await SeedRestaurantAsync(db, CreateKababgy(), SeedKababgy);
    }

    private static async Task SeedRestaurantAsync(AppDbContext db, Restaurant template, Action<AppDbContext, Restaurant> seedMenu)
    {
        var exists = await db.Restaurants.AnyAsync(r => r.Name == template.Name);
        if (exists) return;

        db.Restaurants.Add(template);
        await db.SaveChangesAsync();
        seedMenu(db, template);
        await db.SaveChangesAsync();
    }

    private static Restaurant CreateMcDonalds() => new()
    {
        Name = "McDonald's",
        Description = "World famous burgers, fries & more. I'm lovin' it!",
        ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=800&q=80",
        LogoEmoji = "🍔", BrandColor = "#FFC72C",
        DeliveryTime = "25-35 min", DeliveryFee = 20m, MinOrder = 60m, Rating = 4.6, SortOrder = 1
    };

    private static Restaurant CreatePizzaHut() => new()
    {
        Name = "Pizza Hut",
        Description = "No one outpizzas the Hut. Fresh pizzas & sides.",
        ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=800&q=80",
        LogoEmoji = "🍕", BrandColor = "#EE3124",
        DeliveryTime = "35-50 min", DeliveryFee = 25m, MinOrder = 80m, Rating = 4.4, SortOrder = 2
    };

    private static Restaurant CreateLaPoire() => new()
    {
        Name = "La Poire",
        Description = "Egypt's finest patisserie. Cakes, pastries & café.",
        ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=800&q=80",
        LogoEmoji = "🥐", BrandColor = "#6B3A2A",
        DeliveryTime = "30-45 min", DeliveryFee = 30m, MinOrder = 50m, Rating = 4.8, SortOrder = 3
    };

    private static Restaurant CreateKfc() => new()
    {
        Name = "KFC",
        Description = "Finger lickin' good fried chicken & buckets.",
        ImageUrl = "https://images.unsplash.com/photo-1626082927389-6dd097cdc6ec?w=800&q=80",
        LogoEmoji = "🍗", BrandColor = "#E4002B",
        DeliveryTime = "30-40 min", DeliveryFee = 20m, MinOrder = 70m, Rating = 4.5, SortOrder = 4
    };

    private static Restaurant CreateHardees() => new()
    {
        Name = "Hardee's",
        Description = "Thickburgers, chargrilled taste & big portions.",
        ImageUrl = "https://images.unsplash.com/photo-1550547660-d9450f859349?w=800&q=80",
        LogoEmoji = "🌟", BrandColor = "#C8102E",
        DeliveryTime = "30-45 min", DeliveryFee = 22m, MinOrder = 65m, Rating = 4.3, SortOrder = 5
    };

    private static Restaurant CreateCookDoor() => new()
    {
        Name = "Cook Door",
        Description = "Egyptian grilled chicken, kofta & home-style meals.",
        ImageUrl = "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=800&q=80",
        LogoEmoji = "🔥", BrandColor = "#D4380D",
        DeliveryTime = "35-50 min", DeliveryFee = 25m, MinOrder = 75m, Rating = 4.6, SortOrder = 6
    };

    private static Restaurant CreateGad() => new()
    {
        Name = "Gad",
        Description = "Classic Egyptian fast food — foul, falafel & taameya.",
        ImageUrl = "https://images.unsplash.com/photo-1601050690597-df0568f70950?w=800&q=80",
        LogoEmoji = "🫘", BrandColor = "#2E7D32",
        DeliveryTime = "20-35 min", DeliveryFee = 15m, MinOrder = 40m, Rating = 4.7, SortOrder = 7
    };

    private static Restaurant CreatePapaJohns() => new()
    {
        Name = "Papa John's",
        Description = "Better ingredients. Better pizza. Fresh dough daily.",
        ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=800&q=80",
        LogoEmoji = "🍕", BrandColor = "#2D5A27",
        DeliveryTime = "35-50 min", DeliveryFee = 25m, MinOrder = 85m, Rating = 4.4, SortOrder = 8
    };

    private static Restaurant CreateShawermaElReem() => new()
    {
        Name = "Shawerma El Reem",
        Description = "Legendary Syrian shawerma since 1979. A Cairo icon.",
        ImageUrl = "https://images.unsplash.com/photo-1529006557810-274dbfdd8edf?w=800&q=80",
        LogoEmoji = "🌯", BrandColor = "#B8860B",
        DeliveryTime = "25-40 min", DeliveryFee = 18m, MinOrder = 50m, Rating = 4.9, SortOrder = 9
    };

    private static Restaurant CreateCinnabon() => new()
    {
        Name = "Cinnabon",
        Description = "Warm cinnamon rolls, coffee & sweet treats.",
        ImageUrl = "https://images.unsplash.com/photo-1509368261977-43d4474e9774?w=800&q=80",
        LogoEmoji = "🍩", BrandColor = "#051D49",
        DeliveryTime = "25-35 min", DeliveryFee = 20m, MinOrder = 45m, Rating = 4.6, SortOrder = 10
    };

    private static Restaurant CreateCostaCoffee() => new()
    {
        Name = "Costa Coffee",
        Description = "Premium coffee, sandwiches & all-day breakfast.",
        ImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=800&q=80",
        LogoEmoji = "☕", BrandColor = "#6F2C2F",
        DeliveryTime = "20-30 min", DeliveryFee = 18m, MinOrder = 50m, Rating = 4.5, SortOrder = 11
    };

    private static Restaurant CreateKababgy() => new()
    {
        Name = "Kababgy",
        Description = "Authentic Egyptian grills — kofta, kebab & rice.",
        ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?w=800&q=80",
        LogoEmoji = "🥩", BrandColor = "#8B0000",
        DeliveryTime = "40-55 min", DeliveryFee = 28m, MinOrder = 90m, Rating = 4.5, SortOrder = 12
    };

    private static void SeedMcDonalds(AppDbContext db, Restaurant r)
    {
        var burgers = AddCat(db, r, "Burgers", 1);
        var chicken = AddCat(db, r, "Chicken", 2);
        var sides = AddCat(db, r, "Sides", 3);
        var drinks = AddCat(db, r, "Drinks & Desserts", 4);
        db.MenuItems.AddRange(
            Item(r, burgers, "Big Mac Meal", "Two beef patties, special sauce, fries & drink.", 145m, "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&q=80", true),
            Item(r, burgers, "Double Cheeseburger", "Two beef patties, double cheese, pickles & onions.", 95m, "https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=600&q=80", true),
            Item(r, burgers, "McRoyale", "Beef patty, cheese, lettuce, tomato & mayo.", 110m, "https://images.unsplash.com/photo-1550547660-d9450f859349?w=600&q=80"),
            Item(r, chicken, "McChicken", "Crispy chicken patty with lettuce & mayo.", 85m, "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80", true),
            Item(r, chicken, "Chicken McNuggets 6pc", "Golden crispy chicken nuggets.", 75m, "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80", true),
            Item(r, sides, "Large Fries", "World famous golden fries.", 35m, "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80", true),
            Item(r, drinks, "McFlurry Oreo", "Creamy vanilla soft serve with Oreo pieces.", 55m, "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=600&q=80", true)
        );
    }

    private static void SeedPizzaHut(AppDbContext db, Restaurant r)
    {
        var pizzas = AddCat(db, r, "Pizzas", 1);
        var sides = AddCat(db, r, "Sides", 2);
        var desserts = AddCat(db, r, "Desserts", 3);
        db.MenuItems.AddRange(
            Item(r, pizzas, "Margherita Medium", "Tomato sauce, mozzarella & fresh basil.", 165m, "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=600&q=80", true),
            Item(r, pizzas, "Pepperoni Medium", "Classic pepperoni with mozzarella.", 185m, "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=600&q=80", true),
            Item(r, pizzas, "Super Supreme", "Pepperoni, sausage, peppers, onions & mushrooms.", 220m, "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=600&q=80", true),
            Item(r, pizzas, "BBQ Chicken", "Grilled chicken, BBQ sauce & red onions.", 200m, "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=600&q=80"),
            Item(r, sides, "Garlic Bread", "Toasted bread with garlic butter.", 45m, "https://images.unsplash.com/photo-1619535860434-ba1d8fa12536?w=600&q=80", true),
            Item(r, sides, "Chicken Wings 8pc", "Spicy buffalo or BBQ wings.", 95m, "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80"),
            Item(r, desserts, "Chocolate Donut Bites", "Warm chocolate donut holes.", 65m, "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=600&q=80", true)
        );
    }

    private static void SeedLaPoire(AppDbContext db, Restaurant r)
    {
        var cakes = AddCat(db, r, "Cakes", 1);
        var pastries = AddCat(db, r, "Pastries", 2);
        var beverages = AddCat(db, r, "Beverages", 3);
        db.MenuItems.AddRange(
            Item(r, cakes, "Red Velvet Slice", "Classic red velvet with cream cheese frosting.", 55m, "https://images.unsplash.com/photo-1586788680434-30d324eff2bd?w=600&q=80", true),
            Item(r, cakes, "Chocolate Truffle", "Rich dark chocolate truffle cake.", 60m, "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=600&q=80", true),
            Item(r, pastries, "Chocolate Éclair", "Choux pastry filled with chocolate cream.", 35m, "https://images.unsplash.com/photo-1612202857178-61e8d7eb8bb1?w=600&q=80", true),
            Item(r, pastries, "Butter Croissant", "Flaky, buttery French croissant.", 25m, "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=600&q=80", true),
            Item(r, beverages, "Cappuccino", "Espresso with steamed milk foam.", 40m, "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=600&q=80", true),
            Item(r, beverages, "Fresh Orange Juice", "Freshly squeezed orange juice.", 35m, "https://images.unsplash.com/photo-1621506289937-a8e4df240d0e?w=600&q=80")
        );
    }

    private static void SeedKfc(AppDbContext db, Restaurant r)
    {
        var buckets = AddCat(db, r, "Buckets", 1);
        var chicken = AddCat(db, r, "Chicken", 2);
        var sides = AddCat(db, r, "Sides", 3);
        db.MenuItems.AddRange(
            Item(r, buckets, "8pc Chicken Bucket", "8 pieces original recipe chicken.", 280m, "https://images.unsplash.com/photo-1626082927389-6dd097cdc6ec?w=600&q=80", true),
            Item(r, buckets, "Strips Bucket 12pc", "12 crispy chicken strips with dips.", 260m, "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80"),
            Item(r, chicken, "Zinger Burger", "Spicy crispy chicken fillet burger.", 95m, "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80", true),
            Item(r, chicken, "Twister Wrap", "Crispy strips wrapped with veggies & sauce.", 85m, "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?w=600&q=80", true),
            Item(r, chicken, "3pc Chicken Meal", "3 pieces chicken with fries & drink.", 130m, "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80", true),
            Item(r, sides, "Coleslaw", "Fresh creamy coleslaw.", 25m, "https://images.unsplash.com/photo-1625944525533-473f1a3d54e7?w=600&q=80"),
            Item(r, sides, "Cheese Fries", "Fries topped with cheese sauce.", 45m, "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80", true)
        );
    }

    private static void SeedHardees(AppDbContext db, Restaurant r)
    {
        var burgers = AddCat(db, r, "Burgers", 1);
        var chicken = AddCat(db, r, "Chicken", 2);
        var sides = AddCat(db, r, "Sides", 3);
        db.MenuItems.AddRange(
            Item(r, burgers, "Super Star Burger", "Two chargrilled patties with special sauce.", 135m, "https://images.unsplash.com/photo-1550547660-d9450f859349?w=600&q=80", true),
            Item(r, burgers, "Mushroom Swiss Burger", "Beef patty, sautéed mushrooms & Swiss cheese.", 120m, "https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=600&q=80"),
            Item(r, burgers, "Thickburger", "Half-pound chargrilled Angus beef.", 155m, "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&q=80", true),
            Item(r, chicken, "Hand-Breaded Chicken Tenders", "5pc crispy chicken tenders.", 110m, "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80", true),
            Item(r, chicken, "Spicy Chicken Sandwich", "Crispy spicy chicken with pickles.", 100m, "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?w=600&q=80"),
            Item(r, sides, "Natural Cut Fries", "Thick-cut seasoned fries.", 35m, "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=600&q=80", true),
            Item(r, sides, "Onion Rings", "Crispy golden onion rings.", 40m, "https://images.unsplash.com/photo-1639024377883-141e6e9a32fe?w=600&q=80")
        );
    }

    private static void SeedCookDoor(AppDbContext db, Restaurant r)
    {
        var grills = AddCat(db, r, "Grills", 1);
        var meals = AddCat(db, r, "Meals", 2);
        var sides = AddCat(db, r, "Sides", 3);
        db.MenuItems.AddRange(
            Item(r, grills, "Half Grilled Chicken", "Charcoal grilled half chicken with rice.", 145m, "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=600&q=80", true),
            Item(r, grills, "Kofta Plate", "Seasoned beef kofta with rice & salad.", 130m, "https://images.unsplash.com/photo-1544025162-d76694265947?w=600&q=80", true),
            Item(r, grills, "Mixed Grill", "Kofta, kebab & shish tawook combo.", 195m, "https://images.unsplash.com/photo-1529042410759-befb1204b468?w=600&q=80", true),
            Item(r, meals, "Chicken Pane Meal", "Breaded chicken breast with pasta.", 120m, "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=600&q=80"),
            Item(r, meals, "Beef Shawerma Plate", "Sliced beef shawerma with tahini & bread.", 115m, "https://images.unsplash.com/photo-1529006557810-274dbfdd8edf?w=600&q=80", true),
            Item(r, sides, "Rice with Vermicelli", "Egyptian-style rice.", 30m, "https://images.unsplash.com/photo-1516684732162-798a0062be99?w=600&q=80"),
            Item(r, sides, "Green Salad", "Fresh mixed salad with lemon dressing.", 25m, "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=600&q=80")
        );
    }

    private static void SeedGad(AppDbContext db, Restaurant r)
    {
        var breakfast = AddCat(db, r, "Breakfast", 1);
        var sandwiches = AddCat(db, r, "Sandwiches", 2);
        var drinks = AddCat(db, r, "Drinks", 3);
        db.MenuItems.AddRange(
            Item(r, breakfast, "Foul Medames", "Slow-cooked fava beans with oil & cumin.", 25m, "https://images.unsplash.com/photo-1601050690597-df0568f70950?w=600&q=80", true),
            Item(r, breakfast, "Falafel Plate", "6 pieces crispy falafel with tahini.", 30m, "https://images.unsplash.com/photo-1606491956689-2ea866880f84?w=600&q=80", true),
            Item(r, breakfast, "Taameya Sandwich", "Egyptian falafel in baladi bread.", 15m, "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?w=600&q=80", true),
            Item(r, sandwiches, "Egg & Cheese Sandwich", "Scrambled eggs with white cheese.", 28m, "https://images.unsplash.com/photo-1525351484163-7529414344d8?w=600&q=80"),
            Item(r, sandwiches, "Liver Sandwich", "Spiced beef liver in fresh bread.", 35m, "https://images.unsplash.com/photo-1551782450-17144efb9c50?w=600&q=80"),
            Item(r, drinks, "Fresh Mango Juice", "Seasonal mango juice.", 30m, "https://images.unsplash.com/photo-1600271886742-f049cd451bba?w=600&q=80"),
            Item(r, drinks, "Mint Lemonade", "Cold lemonade with fresh mint.", 20m, "https://images.unsplash.com/photo-1523672890801-63fd961a0d49?w=600&q=80")
        );
    }

    private static void SeedPapaJohns(AppDbContext db, Restaurant r)
    {
        var pizzas = AddCat(db, r, "Pizzas", 1);
        var sides = AddCat(db, r, "Sides", 2);
        db.MenuItems.AddRange(
            Item(r, pizzas, "Super Papa's Large", "Pepperoni, sausage, mushrooms & peppers.", 240m, "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=600&q=80", true),
            Item(r, pizzas, "Chicken BBQ Large", "Grilled chicken with BBQ sauce.", 220m, "https://images.unsplash.com/photo-1593560708920-61dd98c46a4e?w=600&q=80", true),
            Item(r, pizzas, "Margherita Large", "Fresh mozzarella, tomato & basil.", 190m, "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=600&q=80"),
            Item(r, pizzas, "Pepperoni Large", "Classic pepperoni pizza.", 200m, "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=600&q=80", true),
            Item(r, sides, "Garlic Parmesan Breadsticks", "8 breadsticks with garlic butter.", 55m, "https://images.unsplash.com/photo-1619535860434-ba1d8fa12536?w=600&q=80", true),
            Item(r, sides, "Chicken Poppers", "Breaded chicken bites with dip.", 75m, "https://images.unsplash.com/photo-1562967962-3edc2d9d57f4?w=600&q=80"),
            Item(r, sides, "Cheesesticks", "Mozzarella sticks with marinara.", 65m, "https://images.unsplash.com/photo-1531741207018-63bd3314b1db?w=600&q=80")
        );
    }

    private static void SeedShawermaElReem(AppDbContext db, Restaurant r)
    {
        var shawerma = AddCat(db, r, "Shawerma", 1);
        var plates = AddCat(db, r, "Plates", 2);
        db.MenuItems.AddRange(
            Item(r, shawerma, "Chicken Shawerma Sandwich", "Classic Syrian-style chicken shawerma.", 45m, "https://images.unsplash.com/photo-1529006557810-274dbfdd8edf?w=600&q=80", true),
            Item(r, shawerma, "Meat Shawerma Sandwich", "Tender sliced beef shawerma.", 55m, "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?w=600&q=80", true),
            Item(r, shawerma, "Mixed Shawerma Sandwich", "Half chicken, half meat shawerma.", 50m, "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?w=600&q=80", true),
            Item(r, shawerma, "Shawerma with Cheese", "Chicken shawerma with melted cheese.", 55m, "https://images.unsplash.com/photo-1529006557810-274dbfdd8edf?w=600&q=80"),
            Item(r, plates, "Chicken Shawerma Plate", "Shawerma plate with fries & pickles.", 85m, "https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?w=600&q=80", true),
            Item(r, plates, "Meat Shawerma Plate", "Beef shawerma plate with tahini.", 95m, "https://images.unsplash.com/photo-1544025162-d76694265947?w=600&q=80"),
            Item(r, plates, "Family Box", "Mixed shawerma for 3-4 people.", 220m, "https://images.unsplash.com/photo-1529042410759-befb1204b468?w=600&q=80", true)
        );
    }

    private static void SeedCinnabon(AppDbContext db, Restaurant r)
    {
        var rolls = AddCat(db, r, "Cinnamon Rolls", 1);
        var drinks = AddCat(db, r, "Drinks", 2);
        db.MenuItems.AddRange(
            Item(r, rolls, "Classic Roll", "World famous warm cinnamon roll with icing.", 55m, "https://images.unsplash.com/photo-1509368261977-43d4474e9774?w=600&q=80", true),
            Item(r, rolls, "Minibon 3pc", "3 bite-size cinnamon rolls.", 45m, "https://images.unsplash.com/photo-1551024601-bec78aea704b?w=600&q=80"),
            Item(r, rolls, "Caramel Pecanbon", "Roll topped with caramel & pecans.", 70m, "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=600&q=80", true),
            Item(r, rolls, "Chocobon", "Chocolate cinnamon roll.", 60m, "https://images.unsplash.com/photo-1624353365286-3f8d62daad51?w=600&q=80"),
            Item(r, drinks, "Cinnabon Chillatta", "Frozen cinnamon coffee drink.", 65m, "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=600&q=80", true),
            Item(r, drinks, "Hot Chocolate", "Rich creamy hot chocolate.", 45m, "https://images.unsplash.com/photo-1542990253-0d0f5be5f0ed?w=600&q=80")
        );
    }

    private static void SeedCostaCoffee(AppDbContext db, Restaurant r)
    {
        var coffee = AddCat(db, r, "Coffee", 1);
        var food = AddCat(db, r, "Food", 2);
        db.MenuItems.AddRange(
            Item(r, coffee, "Flat White", "Double shot espresso with velvety milk.", 55m, "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=600&q=80", true),
            Item(r, coffee, "Caramel Latte", "Espresso with caramel & steamed milk.", 60m, "https://images.unsplash.com/photo-1572442388796-11668a67e53d?w=600&q=80", true),
            Item(r, coffee, "Iced Americano", "Chilled espresso over ice.", 50m, "https://images.unsplash.com/photo-1517701603779-8f7f1d0e8c0e?w=600&q=80"),
            Item(r, food, "Chicken & Avocado Wrap", "Grilled chicken with avocado & salad.", 85m, "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?w=600&q=80", true),
            Item(r, food, "Ham & Cheese Croissant", "Butter croissant with ham & cheese.", 55m, "https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=600&q=80"),
            Item(r, food, "Blueberry Muffin", "Fresh baked blueberry muffin.", 40m, "https://images.unsplash.com/photo-1607954436303-9a6b36d3ef76?w=600&q=80")
        );
    }

    private static void SeedKababgy(AppDbContext db, Restaurant r)
    {
        var grills = AddCat(db, r, "Grills", 1);
        var combos = AddCat(db, r, "Combos", 2);
        db.MenuItems.AddRange(
            Item(r, grills, "Kabab Halla", "Charcoal grilled minced beef kabab.", 140m, "https://images.unsplash.com/photo-1544025162-d76694265947?w=600&q=80", true),
            Item(r, grills, "Kofta Plate", "Spiced kofta skewers with rice.", 135m, "https://images.unsplash.com/photo-1529042410759-befb1204b468?w=600&q=80", true),
            Item(r, grills, "Shish Tawook", "Marinated grilled chicken skewers.", 130m, "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?w=600&q=80", true),
            Item(r, grills, "Lamb Chops", "Tender grilled lamb chops.", 220m, "https://images.unsplash.com/photo-1546833999-b9f581a1996d?w=600&q=80"),
            Item(r, combos, "Kababgy Mix Grill", "Kofta, kabab & tawook for 2.", 280m, "https://images.unsplash.com/photo-1529042410759-befb1204b468?w=600&q=80", true),
            Item(r, combos, "Family Feast", "Mixed grill platter for 4 people.", 450m, "https://images.unsplash.com/photo-1544025162-d76694265947?w=600&q=80"),
            Item(r, combos, "Rice & Salad Combo", "Add rice, bread & salad to any grill.", 45m, "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=600&q=80")
        );
    }

    private static Category AddCat(AppDbContext db, Restaurant r, string name, int order)
    {
        var cat = new Category { Name = name, Restaurant = r, SortOrder = order };
        db.Categories.Add(cat);
        return cat;
    }

    private static MenuItem Item(Restaurant r, Category c, string name, string desc, decimal price, string img, bool featured = false) =>
        new()
        {
            Restaurant = r,
            Category = c,
            Name = name,
            Description = desc,
            Price = price,
            ImageUrl = img,
            IsFeatured = featured
        };
}
