namespace CarDealer
{
    using AutoMapper;
    using CarDealer.Data;
    using CarDealer.Dtos.Export;
    using CarDealer.Dtos.Import;
    using CarDealer.Models;
    using System;
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
            string suppliersXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\CarDealer - Skeleton\CarDealer\Datasets\suppliers.xml");
            string partsXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\CarDealer - Skeleton\CarDealer\Datasets\parts.xml");
            string carsXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\CarDealer - Skeleton\CarDealer\Datasets\cars.xml");
            string customersXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\CarDealer - Skeleton\CarDealer\Datasets\customers.xml");
            string salesXml = File.ReadAllText(@"D:\Programming\C#DB\MS SQL Server\Entity Framework Core\09.EXERCISE XML PROCESSING\CarDealer - Skeleton\CarDealer\Datasets\sales.xml");

            Mapper.Initialize(cfg => cfg.AddProfile<CarDealerProfile>());

            using (var db = new CarDealerContext())
            {                
                string result = GetSalesWithAppliedDiscount(db);
                Console.WriteLine(result);
            }
        }

        // 09.Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSuppliersDto[]), new XmlRootAttribute("Suppliers"));

            var supplierDtos = (ImportSuppliersDto[])serializer.Deserialize(new StringReader(inputXml));
            var suppliers = Mapper.Map<IEnumerable<ImportSuppliersDto>, IEnumerable<Supplier>>(supplierDtos);

            context.Suppliers.AddRange(suppliers);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 10.Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartsDto[]), new XmlRootAttribute("Parts"));

            var partDtos = (ImportPartsDto[])serializer.Deserialize(new StringReader(inputXml));
            var parts = Mapper.Map<IEnumerable<ImportPartsDto>, IEnumerable<Part>>(partDtos);

            var validParts = new List<Part>();

            var suppliers = context.Suppliers.Select(s => s.Id);

            foreach (var part in parts)
            {
                if (suppliers.Contains(part.SupplierId))
                {
                    validParts.Add(part);
                }
            }

            context.Parts.AddRange(validParts);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 11.Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]),
                            new XmlRootAttribute("Cars"));

            var carDtos = (ImportCarDto[])(xmlSerializer.Deserialize(new StringReader(inputXml)));
            var cars = new List<Car>();

            foreach (var carDto in carDtos)
            {
                var car = Mapper.Map<Car>(carDto);

                foreach (var part in carDto.Parts)
                {
                    var partCarExists = car
                        .PartCars
                        .FirstOrDefault(p => p.PartId == part.PartId) != null;

                    if (!partCarExists && context.Parts.Any(p => p.Id == part.PartId))
                    {
                        var partCar = new PartCar
                        {
                            CarId = car.Id,
                            PartId = part.PartId
                        };

                        car.PartCars.Add(partCar);
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {context.Cars.ToList().Count}";
        }

        // 12.Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomersDto[]), new XmlRootAttribute("Customers"));

            var customerDtos = (ImportCustomersDto[])serializer.Deserialize(new StringReader(inputXml));
            var customers = Mapper.Map<IEnumerable<ImportCustomersDto>, IEnumerable<Customer>>(customerDtos);

            context.Customers.AddRange(customers);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 13.Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSalesDto[]), new XmlRootAttribute("Sales"));

            var salesDto = (ImportSalesDto[])serializer.Deserialize(new StringReader(inputXml));
           

            var carIds = context.Cars.Select(c => c.Id).ToList();            

            var validSales = new List<ImportSalesDto>();

            foreach (var sale in salesDto)
            {
                if(carIds.Contains(sale.CarId))
                {
                    validSales.Add(sale);
                }
            }
            var sales = Mapper.Map<IEnumerable<ImportSalesDto>, IEnumerable<Sale>>(validSales);

            context.Sales.AddRange(sales);
            var count = context.SaveChanges();

            return $"Successfully imported {count}";
        }

        // 14.Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsWithDistance = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new ExportCarsWithDistanceDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), carsWithDistance, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 15.Cars from make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportCarsFromMakeBMWDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsFromMakeBMWDto[]), new XmlRootAttribute("cars"));
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 16.Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), localSuppliers, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 17.Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context
                .Cars
                .Select(c => new ExportCarsWithTheirListOfPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars.Select(pc => new PartsDto
                    {
                        Name = pc.Part.Name,
                        Price = pc.Part.Price
                    })
                    .OrderByDescending(pc => pc.Price)
                    .ToArray()

                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithTheirListOfPartsDto[]), new XmlRootAttribute("cars"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), carsWithParts, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 18.Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSalesByCustomer = context
                .Customers
                .Where(c => c.Sales.Any())
                .Select(c => new TotalSalesByCustomerDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(TotalSalesByCustomerDto[]), new XmlRootAttribute("customers"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), totalSalesByCustomer, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 19.Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithAppliedDiscount = context
                .Sales
                .Select(s => new SalesWithAppliedDiscountDto
                {
                    CarDto = new CarDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                    PriceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price) - (s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100)
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(SalesWithAppliedDiscountDto[]), new XmlRootAttribute("sales"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), salesWithAppliedDiscount, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}