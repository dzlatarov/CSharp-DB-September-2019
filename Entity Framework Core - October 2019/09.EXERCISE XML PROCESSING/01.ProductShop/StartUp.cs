namespace ProductShop
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ProductShop.Data;
    using ProductShop.Dtos.Export;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            string usersXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\ProductShop - Skeleton\ProductShop\Datasets\users.xml");
            string productsXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\ProductShop - Skeleton\ProductShop\Datasets\products.xml");
            string categoryXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\ProductShop - Skeleton\ProductShop\Datasets\categories.xml");
            string categoryProductXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\ProductShop - Skeleton\ProductShop\Datasets\categories-products.xml");

            Mapper.Initialize(cfg => cfg.AddProfile<ProductShopProfile>());

            using (var db = new ProductShopContext())
            {
                var result = GetUsersWithProducts(db);
                Console.WriteLine(result);
            }
        }

        // 01.Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportUserDto[]), new XmlRootAttribute("Users"));

            var userDtos = (ImportUserDto[])(serializer.Deserialize(new StringReader(inputXml)));
            var users = Mapper.Map<IEnumerable<ImportUserDto>, IEnumerable<User>>(userDtos);

            context.Users.AddRange(users);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 02.Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var productDtos = (ImportProductDto[])(serializer.Deserialize(new StringReader(inputXml)));
            var products = Mapper.Map<IEnumerable<ImportProductDto>, IEnumerable<Product>>(productDtos);

            context.Products.AddRange(products);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var categoryDtos = (ImportCategoryDto[])(serializer.Deserialize(new StringReader(inputXml)));
            var categories = Mapper.Map<IEnumerable<ImportCategoryDto>, IEnumerable<Category>>(categoryDtos);

            context.Categories.AddRange(categories);
            int count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 04.Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));

            var categoryProductsDtos = (ImportCategoryProductDto[])(serializer.Deserialize(new StringReader(inputXml)));
            var categoryProducts = Mapper.Map<IEnumerable<ImportCategoryProductDto>, IEnumerable<CategoryProduct>>(categoryProductsDtos);


            var categories = context.Categories
                .Select(c => c.Id);
            var products = context.Products
                .Select(p => p.Id);

            var validCategoryProducts = categoryProducts
                .Where(cp => categories.Contains(cp.CategoryId) &&
                products.Contains(cp.ProductId));

            context.CategoryProducts.AddRange(validCategoryProducts);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 05.Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ProductsInRangeDto>()
                .ToArray();

            var serializer = new XmlSerializer(typeof(ProductsInRangeDto[]), new XmlRootAttribute("Products"));
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), productsInRange, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 06.Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context
                .Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new SoldProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(ps => new ProductDto
                    {
                        Name = ps.Name,
                        Price = ps.Price
                    })
                    .ToArray()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SoldProductsDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), soldProducts, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 07.Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new ExportAllCategoriesDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(cp => cp.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportAllCategoriesDto[]), new XmlRootAttribute("Categories"));
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 08.Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(x => x.ProductsSold.Count)
                .Select(u => new ExportUserAndProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductDto = new SoldProductDto
                    {
                        Count = u.ProductsSold.Count,
                        ProductDto = u.ProductsSold.Select(p => new ProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }

                })
                .Take(10)
                .ToArray();

            var customExport = new ExportCustomUserProductDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                ExportUserAndProductDto = users
            };

            var serializer = new XmlSerializer(typeof(ExportCustomUserProductDto), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), customExport, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}