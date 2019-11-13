using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string usersJson = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\08.EXERCISE JSON PROCESSING\Product Shop - Skeleton\ProductShop\Datasets\users.json");

            string productsJson = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\08.EXERCISE JSON PROCESSING\Product Shop - Skeleton\ProductShop\Datasets\products.json");

            string categoryJson = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\08.EXERCISE JSON PROCESSING\Product Shop - Skeleton\ProductShop\Datasets\categories.json");

            string categoryProducts = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\08.EXERCISE JSON PROCESSING\Product Shop - Skeleton\ProductShop\Datasets\categories-products.json");
            

            using (var context = new ProductShopContext())
            {
                Console.WriteLine(GetUsersWithProducts(context));
            }
        }

        // 01.Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson)
                .Where(u => u.LastName != null && u.LastName.Length >= 3);

            context.Users.AddRange(users);

            int affectedRows = context.SaveChanges();

            return $"Successfully imported {affectedRows}";
        }

        // 02.Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson)
                .Where(x => x.Name != null && x.Name.Trim().Length >= 3)
                .ToArray();

            context.Products.AddRange(products);

            context.SaveChanges();


            return $"Successfully imported {products.Length}";
        }

        // 03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null && x.Name.Length >= 3)
                .ToArray();
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        // 04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson)
                .ToArray();
            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Length}";
        }

        // 05. Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToList();

            var convertedProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            return convertedProducts;
        }

        // 06. Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
                .Where(x => x.ProductsSold.Count > 0 &&
                x.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                    .Where(ps => ps.Buyer != null)
                    .Select(ps => new
                    {
                        name = ps.Name,
                        price = ps.Price,
                        buyerFirstName = ps.Buyer.FirstName,
                        buyerLastName = ps.Buyer.LastName
                    })
                    .ToArray()
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonResult = JsonConvert.SerializeObject(soldProducts, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });


            return jsonResult;
        }

        // 07. Export Categories by Product Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = $"{c.CategoryProducts.Average(x => x.Product.Price):f2}",
                    totalRevenue = $"{c.CategoryProducts.Count * c.CategoryProducts.Average(x => x.Product.Price):f2}"
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }

        // 08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersProducts = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,

                    soldProducts = (new
                    {
                        count = u.ProductsSold.Count(psd => psd.Buyer != null),

                        products = u.ProductsSold
                        .Where(prs => prs.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    })
                })
                .ToList();

            var resultusers = new
            {
                usersCount = usersProducts.Count,
                users = usersProducts
            };

            var jsonResult = JsonConvert.SerializeObject(resultusers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return jsonResult;
        }
    }
}