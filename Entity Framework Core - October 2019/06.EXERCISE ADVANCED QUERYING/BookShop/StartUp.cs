namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                int result = RemoveBooks(db);
                Console.WriteLine(result);
            }
        }

        // 1. Age Restriction       
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {

            var ageParsed = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var books = context.Books
                .Where(b => b.AgeRestriction == ageParsed)
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        // 2. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldEdition = (EditionType)Enum.Parse(typeof(EditionType), "gold", true);
            var books = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == goldEdition)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        // 3.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    bookTitle = b.Title,
                    price = b.Price
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.bookTitle} - ${book.price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 4. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        // 5. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var listOfCategories = input
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => listOfCategories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            var result = string.Join(Environment.NewLine, books);

            return result;
        }

        // 6. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            string format = "dd-MM-yyyy";
            var parsedDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);


            var books = context.Books
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    title = b.Title,
                    editionType = b.EditionType,
                    price = b.Price
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.title} - {book.editionType} - ${book.price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 7. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                });

            foreach (var author in authors.OrderBy(f => f.FullName))
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        // 8.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            return string.Join(Environment.NewLine, books);
        }

        // 9. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    title = b.Title,
                    authorName = b.Author.FirstName + " " + b.Author.LastName
                });

            foreach (var book in books)
            {
                sb.AppendLine($"{book.title} ({book.authorName})");
            }

            return sb.ToString().TrimEnd();
        }

        // 10. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck);

            return books.Count();
        }

        // 11. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var countCopiesByAuthor = context.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    copies = a.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(b => b.copies);

            foreach (var author in countCopiesByAuthor)
            {
                sb.AppendLine($"{author.FullName} - {author.copies}");
            }

            return sb.ToString().TrimEnd();
        }

        // 12. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    totalProfit = c.CategoryBooks.Select(b => b.Book.Copies * b.Book.Price).Sum()
                })
                .OrderByDescending(c => c.totalProfit);

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.category} ${category.totalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var categories = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => new
                    {
                        title = b.Book.Title,
                        releasedDate = b.Book.ReleaseDate.Value.Year
                    })
                })
                .OrderBy(c => c.Name);

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.title} ({book.releasedDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // 14. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        // 15. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Count();
        }
    }
}
